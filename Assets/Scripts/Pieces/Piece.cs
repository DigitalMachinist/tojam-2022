using System;
using System.Collections.Generic;
using Board;
using Exceptions;
using Players;
using UnityEngine;

namespace Pieces
{
    public class Piece : MonoBehaviour
    {
        public Player Player;
        public Tile Tile;
        public bool IsTaken;
        public bool HasMovedThisTurn;
        public bool IsFinishedMoving;

        public virtual void CalculateIsFinishedMoving()
        {
            // By default, if something has moved this turn it can't move again.
            // Checkers pieces (for example) will require more complicated logic here.
            IsFinishedMoving = true;
        }

        public virtual List<Piece> Move(Player player, Tile endTile)
        {
            var direction = Tile.Board.GetDirection(Tile, endTile);
            var distance = Tile.Board.GetDistance(Tile, endTile);
            Debug.Log($"Direction: {direction}");
            Debug.Log($"Distance: {distance}");
            ValidateMove(player, Tile, endTile, direction, distance, true);

            // Keep track of pieces to return as taken.
            var pieces = new List<Piece>();
            if (endTile.Piece != null)
            {
                pieces.Add(endTile.Piece);
            }

            Tile.Piece = null;
            Tile = endTile;
            Tile.Piece = this;
            HasMovedThisTurn = true;
            
            // TODO: Tween this instead?
            transform.position = Tile.transform.position;
            
            CalculateIsFinishedMoving();

            return pieces;
        }
        
        public virtual bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool throwExceptions = false)
        {
            try
            {
                startTile.Board.GetTile(startTile, direction, distance);
            }
            catch (IndexOutOfRangeException e)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Can't move off the edge of the board.");
                }

                return false;
            }
            
            if (IsTaken)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Can't move a taken piece.");
                }
                
                return false;
            }

            if (player != Player)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Can't move a piece that isn't yours.");
                }

                return false;
            }

            if (IsFinishedMoving)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Can't move this piece again until next turn.");
                }

                return false;
            }

            if (endTile.IsDestroyed)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Can't move into a destroyed tile.");
                }

                return false;
            }

            if (endTile.Piece != null && endTile.Piece.Player == Player)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Can't move into the same tile as one of your own pieces.");
                }

                return false;
            }

            return true;
        }

        public virtual void Place(Player player, Tile tile)
        {
            ValidatePlace(player, tile, true);
            
            Player = player;
            Tile = tile;
            Tile.Piece = this;
            IsTaken = false;
            HasMovedThisTurn = false;
            IsFinishedMoving = false;
            transform.position = Tile.transform.position;
        }

        public virtual bool ValidatePlace(Player player, Tile tile, bool throwExceptions = false)
        {
            if (tile.IsDestroyed)
            {
                if (throwExceptions)
                {
                    throw new PlacementException("Can't place onto a destroyed tile.");
                }

                return false;
            }

            if (tile.Piece != null)
            {
                if (throwExceptions)
                {
                    throw new PlacementException("Can't place onto the same space as another piece.");
                }

                return false;
            }

            return true;
        }
        
        public void Take()
        {
            Tile = null;
            IsTaken = true;
            
            // TODO: Animate the piece being destroyed somehow?
            transform.position = Vector3.zero;
        }
        
        public void NextTurn()
        {
            HasMovedThisTurn = false;
            IsFinishedMoving = false;
        }

    }
}
