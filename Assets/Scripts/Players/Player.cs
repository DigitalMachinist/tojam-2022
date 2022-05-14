using System;
using System.Collections.Generic;
using Board;
using Pieces;
using UnityEngine;

namespace Players
{
    public class Player : MonoBehaviour
    {
        public PlayerColour Colour;
        public List<Piece> Pieces;
        public List<Piece> TakenPieces;
        public int TurnNumber = 1;

        public event Action TurnAdvanced;

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