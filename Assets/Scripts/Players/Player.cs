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

        public void PlacePiece(Piece piece, Tile tile)
        {
            piece.Place(this, tile);
            Pieces.Add(piece);
        }

        public void MovePiece(Piece piece, Tile tile)
        {
            var takenPieces = piece.Move(this, tile);
            foreach (var takenPiece in takenPieces)
            {
                Debug.Log(takenPiece);
                TakePiece(takenPiece);
            }
        }
        
        public void TakePiece(Piece piece)
        {
            piece.Take();
            TakenPieces.Add(piece);
            
            // TODO: Move the piece to some kind of graveyard area?
        }

        public void NextTurn()
        {
            foreach (var piece in Pieces)
            {
                piece.NextTurn();
            }
        }
    }
}
