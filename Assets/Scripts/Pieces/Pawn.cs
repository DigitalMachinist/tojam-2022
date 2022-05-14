using System.Collections.Generic;
using Board;
using Exceptions;
using Players;

namespace Pieces
{
    public class Pawn : Piece
    {
        int thisPawnMoves = 0;

        public override List<Piece> Move(Player player, Tile endTile)
        {

            List < Piece > takenPieces = base.Move(player, endTile);

            thisPawnMoves++;

            return takenPieces;


        }
        public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool throwExceptions = false)
        {
            var baseResult = base.ValidateMove(player, startTile, endTile, direction, distance, throwExceptions);
            if (!baseResult)
            {
                return false;
            }

            // Pawns can only move forward.
            if (player.Colour == PlayerColour.White && direction == Direction.N
                || player.Colour == PlayerColour.Black && direction == Direction.S)
            {
                if (thisPawnMoves == 0 && distance > 2)
                {
                    if (throwExceptions)
                    {
                        throw new MovementException("Pawns can only move 2 tiles on the first turn.");
                    }

                    return false;
                }
                else if (thisPawnMoves > 0 && distance > 1)
                {
                    if (throwExceptions)
                    {
                        throw new MovementException("Pawns can only move 1 tile at a time.");
                    }

                    return false;
                }
                else if (endTile.Piece != null)
                {
                    if (throwExceptions)
                    {
                        throw new MovementException("Pawns cannot attack while moving directly forward.");
                    }

                    return false;
                }

                return true;
            }

            // Pawns can only attack on diagonals.
            if ((player.Colour == PlayerColour.White && (direction == Direction.NE || direction == Direction.NW))
                || (player.Colour == PlayerColour.Black && (direction == Direction.SE || direction == Direction.SW)))
            {
                if (distance > 1)
                {
                    if (throwExceptions)
                    {
                        throw new MovementException("Pawns can only move 1 tile at a time.");
                    }

                    return false;
                }
                else if (endTile.Piece == null)
                {
                    if (throwExceptions)
                    {
                        throw new MovementException("Pawns must be attacking to move diagonally.");
                    }

                    return false;
                }

                return true;
            }

            // Any other move is invalid.
            if (throwExceptions)
            {
                throw new MovementException("Invalid move.");
            }

            return false;
        }
    }
}