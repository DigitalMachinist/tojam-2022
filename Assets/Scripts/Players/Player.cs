using System;
using System.Collections.Generic;
using Board;
using Pieces;
using UnityEngine;
using Managers;

namespace Players
{
    public class Player : MonoBehaviour
    {
        public event Action TurnAdvanced;
        
        public PlayerColour Colour;
        public List<Piece> Pieces;
        public List<Piece> TakenPieces;
        public int TurnNumber = 0;

        public bool HasLost => !HasPieces || !CanMove(true);

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
        
        public void PlaceCard(CardScriptableObject cardObj, Tile tile, bool ignoreTurn = false)
        {
            var instance = Instantiate(cardObj.prefab, tile.transform, false);
            var piece = instance.GetComponent<Piece>();
            if (instance.GetComponent<Piece>())
            {
                PlacePiece(piece, tile, ignoreTurn);
            }
            
            // TODO: Handle board effect cards.
        }

        public void PlacePiece(Piece piece, Tile tile, bool ignoreTurn = false)
        {
            Pieces.Add(piece);
            piece.Place(this, tile, ignoreTurn);
        }

        public void MovePiece(Piece piece, Tile tile)
        {
            var takenPieces = piece.Move(this, tile);
            foreach (var takenPiece in takenPieces)
            {
                // Debug.Log(takenPiece);
                TakePiece(takenPiece);
            }

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
