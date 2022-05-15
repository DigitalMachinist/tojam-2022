using Board;
using Exceptions;
using Players;

namespace Pieces
{
    public class King : Piece
    {
        public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool throwExceptions = false)
        {
            var baseResult = base.ValidateMove(player, startTile, endTile, direction, distance, throwExceptions);
            if (!baseResult)
            {
                return false;
            }

            // King can move cardinally or diagonally.
            if (direction == Direction.None)
            {
                if (throwExceptions)
                {
                    throw new MovementException("King can only move cardinally or diagonally.");
                }

                return false;
            }

            // Kings can move only 1 tile at a time.
            if (distance > 1)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Kings can only move 1 tile at a time.");
                }
                
                return false;
            }

            return true;
        }
    }
}
