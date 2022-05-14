using Exceptions;
using Pieces;
using Players;
using UnityEngine;

namespace Board
{
    public class Tile : MonoBehaviour
    {
        public Board Board;
        public int Row;
        public int Col;
        public Piece Piece;
        public bool IsDestroyed;
        public int TurnsUntilDestroyed;

        public Piece SelectPiece(Player player)
        {
            if (Piece == null)
            {
                throw new SelectionException("There are no pieces on this tile.");
            }
            
            if (player.Colour != Piece.Player.Colour)
            {
                throw new SelectionException("Cannot select a piece that is not yours.");
            }

            return Piece;
        }
    }
}
