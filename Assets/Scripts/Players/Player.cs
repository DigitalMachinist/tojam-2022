using System;
using System.Collections.Generic;
using Board;
using Pieces;
using UnityEngine;

namespace Players
{
    public class Player : MonoBehaviour
    {
        public event Action TurnAdvanced;
        
        public PlayerColour Colour;
        public List<Piece> Pieces;
        public List<Piece> TakenPieces;
        public int TurnNumber = 1;

        public bool HasLost => !HasPieces || !CanMove();

        public bool HasPieces => Pieces.Count != 0;

        public bool CanMove()
        {
            foreach (var piece in Pieces)
            {
                if (piece.GetValidMoves().Count > 0)
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
    }
}
