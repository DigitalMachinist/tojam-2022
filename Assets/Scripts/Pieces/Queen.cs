using Board;
using Exceptions;
using Players;

namespace Pieces
{
    public class Queen : Piece
    {
        public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool ignoreTurn = false, bool throwExceptions = false)
        {
            var baseResult = base.ValidateMove(player, startTile, endTile, direction, distance, ignoreTurn, throwExceptions);
            if (!baseResult)
            {
                return false;
            }
            
            // Queens can move cardinally or diagonally.
            if (direction == Direction.None)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Queens can only move cardinally or diagonally.");
                }

                return false;
            }

            // Queens can't move through other pieces.
            for (var d = 1; d < distance; d++)
            {
                var tile = Tile.Board.GetTile(startTile, direction, d);
                if (tile.Piece != null)
                {
                    if (throwExceptions)
                    {
                        throw new MovementException("Queens can't move through other pieces.");
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
