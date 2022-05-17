using System.Collections.Generic;
using Board;
using Exceptions;
using Managers;
using Players;

namespace Pieces
{
    public class Pawn : Piece
    {
        public int DoubleMoveTurnNumber;
        
        public override List<Piece> Move(Player player, Tile endTile)
        {
            var direction = Tile.Board.GetDirection(Tile, endTile);
            var distance = Tile.Board.GetDistance(Tile, endTile);
            if (distance == 2)
            {
                DoubleMoveTurnNumber = GameManager.Get().TurnNumber;
            }

            var takenPieces = base.Move(player, endTile);
            if (direction == Direction.NE || direction == Direction.NW)
            {
                var passantTile = Tile.Board.GetTile(Tile, Direction.S, 1);
                if (passantTile.Piece != null && passantTile.Piece.Player != player && passantTile.Piece is Pawn otherPawn && otherPawn.DoubleMoveTurnNumber >= (GameManager.Get().TurnNumber - 1))
                {
                    takenPieces.Add(passantTile.Piece);
                }
            }
            else if (direction == Direction.SE || direction == Direction.SW)
            {
                var passantTile = Tile.Board.GetTile(Tile, Direction.N, 1);
                if (passantTile.Piece != null && passantTile.Piece.Player != player && passantTile.Piece is Pawn otherPawn && otherPawn.DoubleMoveTurnNumber >= (GameManager.Get().TurnNumber - 1))
                {
                    takenPieces.Add(passantTile.Piece);
                }
            }

            return takenPieces;
        }

        public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool ignoreTurn = false, bool throwExceptions = false)
        {
            var baseResult = base.ValidateMove(player, startTile, endTile, direction, distance, ignoreTurn, throwExceptions);
            if (!baseResult)
            {
                return false;
            }

            // Pawns can only move forward.
            if (player.Colour == PlayerColour.White && direction == Direction.N
                || player.Colour == PlayerColour.Black && direction == Direction.S)
            {
                if (NumMovesPerformed == 0)
                {
                    if (distance > 2)
                    {
                        if (throwExceptions)
                        {
                            throw new MovementException("Pawns can only move 2 tiles on the first turn.");
                        }

                        return false;
                    }
                    else if (Tile.Board.GetTile(Tile, direction, 1).Piece != null)
                    {
                        if (throwExceptions)
                        {
                            throw new MovementException("Pawns can't move through other pieces.");
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
                }
                else if (NumMovesPerformed > 0)
                {
                    if (distance > 1)
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
                    if (direction == Direction.NE || direction == Direction.SE)
                    {
                        var passantTile = Tile.Board.GetTile(Tile, Direction.E, 1);
                        if (passantTile.Piece != null && passantTile.Piece.Player != player && passantTile.Piece is Pawn otherPawn && otherPawn.DoubleMoveTurnNumber >= (GameManager.Get().TurnNumber - 1))
                        {
                            return true;
                        }
                    }
                    else if (direction == Direction.NW || direction == Direction.SW)
                    {
                        var passantTile = Tile.Board.GetTile(Tile, Direction.W, 1);
                        if (passantTile.Piece != null && passantTile.Piece.Player != player && passantTile.Piece is Pawn otherPawn && otherPawn.DoubleMoveTurnNumber >= (GameManager.Get().TurnNumber - 1))
                        {
                            return true;
                        }
                    }

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
