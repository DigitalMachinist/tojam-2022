using Pieces;
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
    }
}
