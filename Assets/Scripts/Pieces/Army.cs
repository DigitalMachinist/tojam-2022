using Board;
using Exceptions;
using Players;

namespace Pieces
{
    public class Army : Piece
    {
        public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool ignoreTurn = false, bool throwExceptions = false)
        {
            var baseResult = base.ValidateMove(player, startTile, endTile, direction, distance, ignoreTurn, throwExceptions);
            if (!baseResult)
            {
                return false;
            }

            // Army man can move cardinally or diagonally.
            if (direction == Direction.None)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Army man can only move cardinally or diagonally.");
                }

                return false;
            }

            // Army man can move only 1 tile at a time.
            if (distance > 2)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Army man can only move 1 tile at a time.");
                }
                
                return false;
            }

            // Army man can't move through other pieces.
            for (var d = 1; d < distance; d++)
            {
                var tile = Tile.Board.GetTile(startTile, direction, d);
                if (tile.Piece != null)
                {
                    if (throwExceptions)
                    {
                        throw new MovementException("Army man can't move through other pieces.");
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
