using Board;
using Exceptions;
using Players;
using UnityEngine;

namespace Pieces
{
    public class Knight : Piece
    {
        public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool throwExceptions = false)
        {
            var baseResult = base.ValidateMove(player, startTile, endTile, direction, distance, throwExceptions);
            if (!baseResult)
            {
                return false;
            }
            
            // Knights move in a 2x1 L-shape.
            var rowDiff = endTile.Row - startTile.Row;
            var colDiff = endTile.Col - startTile.Col;
            if ((Mathf.Abs(rowDiff) == 2 && Mathf.Abs(colDiff) == 1) || (Mathf.Abs(rowDiff) == 1 && Mathf.Abs(colDiff) == 2))
            {
                return true;
            }

            if (throwExceptions)
            {
                throw new MovementException("Invalid move.");
            }
            
            return false;
        }
    }
}
