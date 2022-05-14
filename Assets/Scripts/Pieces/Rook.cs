using Board;
using Exceptions;
using Players;

namespace Pieces
{
    public class Rook : Piece
    {
        public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool throwExceptions = false)
        {
            var baseResult = base.ValidateMove(player, startTile, endTile, direction, distance, throwExceptions);
            if (!baseResult)
            {
                return false;
            }
            
            // Rooks can only move cardinally.
            if (direction == Direction.None || direction == Direction.NW || direction == Direction.NE || direction == Direction.SW || direction == Direction.SE)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Rooks can only move cardinally.");
                }

                return false;
            }

            // Rooks can't move through other pieces.
            for (var d = 1; d < distance; d++)
            {
                var tile = Tile.chessboard.GetTile(startTile, direction, d);
                if (tile.Piece != null)
                {
                    if (throwExceptions)
                    {
                        throw new MovementException("Rooks can't move through other pieces.");
                    }

                    return false;
                }
            }

            return true;
        }
    }
}
