using Board;
using Exceptions;
using Players;

namespace Pieces
{
    public class Bishop : Piece
    {
        public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool throwExceptions = false)
        {
            var baseResult = base.ValidateMove(player, startTile, endTile, direction, distance, throwExceptions);
            if (!baseResult)
            {
                return false;
            }
            
            // Bishops can only move diagonally.
            if (direction == Direction.None || direction == Direction.N || direction == Direction.E || direction == Direction.S || direction == Direction.W)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Bishops can only move diagonally.");
                }

                return false;
            }

            // Bishops can't move through other pieces.
            for (var d = 1; d < distance; d++)
            {
                var tile = Tile.Board.GetTile(startTile, direction, d);
                if (tile.Piece != null)
                {
                    if (throwExceptions)
                    {
                        throw new MovementException("Bishops can't move through other pieces.");
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
