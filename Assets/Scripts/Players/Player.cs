using System;
using System.Collections.Generic;
using Board;
using Pieces;
using UnityEngine;
using Managers;
using UnityEngine.Networking;

namespace Players
{
    public class Player : MonoBehaviour
    {
        public event Action TurnAdvanced;
        
        public PlayerColour Colour;
        public List<Piece> Pieces;
        public List<Piece> TakenPieces;
        public int TurnNumber = 0;

        public bool HasLost => !HasPieces || !CanMove(true) || !CanPlace(true);

        public bool HasPieces => Pieces.Count != 0;

        public bool CanMove(bool ignoreTurn = true)
        {
            foreach (var piece in Pieces)
            {
                if (piece.GetValidMoves(ignoreTurn).Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanPlace(bool ignoreTurn = true)
        {
            // This is hacky and really slow but it will have to do.
            // I'm temporarily instantiating a piece for each card and hacking the temporary piece to have a reference
            // to any tile (tile [1, 1] is arbitrary) and a reference to this player, because I don't want to place the
            // piece on the board, but it needs these references to properly do placement validation.
            foreach (var card in GameManager.Get().GetPlayerHand(Colour).playerHand)
            {
                var result = false;
                var go = Instantiate(card._cardSO.prefab, transform, false);
                var piece = go.GetComponent<Piece>();
                piece.Player = this;
                piece.Tile = GameManager.Get().Board.GetTile(1, 1);
                if (piece.GetValidPlaces(ignoreTurn).Count > 0)
                {
                    Destroy(go);
                    return true;
                }
                else
                {
                    Destroy(go);
                }
            }

            return false;
        }
        
        public void PlaceCard(CardScriptableObject cardObj, Tile tile, bool ignoreTurn = false)
        {
            var go = Instantiate(cardObj.prefab, transform, false);
            var piece = go.GetComponent<Piece>();
            if (piece != null)
            {
                PlacePiece(piece, tile, ignoreTurn);
            }
            
            // TODO: Handle board effect cards.
        }

        public void PlacePiece(Piece piece, Tile tile, bool ignoreTurn = false)
        {
            piece.Place(this, tile, ignoreTurn);
            Pieces.Add(piece);
        }

        public void MovePiece(Piece piece, Tile tile)
        {
            var takenPieces = piece.Move(this, tile);
            foreach (var takenPiece in takenPieces)
            {
                // Debug.Log(takenPiece);
                TakePiece(takenPiece);
            }

            if(piece.isActiveAndEnabled)
                tile.Piece = piece;
        }
        
        public void TakePiece(Piece piece)
        {
            TakenPieces.Add(piece);
            piece.Take();

            GameManager.Get().Audio_PieceDestroy.Play();
            // TODO: Move the piece to some kind of graveyard area?
        }

        public void AdvanceTurn()
        {
            foreach (var piece in Pieces)
            {
                piece.NextTurn();
            }
            TurnNumber++;
            TurnAdvanced?.Invoke();
        }

        public void Reset()
        {
            foreach (var piece in Pieces)
            {
                Destroy(piece.gameObject);
            }
            Pieces.Clear();
            TakenPieces.Clear();
            TurnNumber = 0;
        }
    }
}
