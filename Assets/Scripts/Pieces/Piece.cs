using System.Collections.Generic;
using System.Diagnostics;
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

        public virtual bool ValidateMove(Player player, Direction direction, Tile tile, bool throwExceptions = false)
        {
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

            if (tile.IsDestroyed)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Can't move into a destroyed tile.");
                }

                return false;
            }

            if (tile.Piece != null && tile.Piece.Player.Colour == Player.Colour)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Can't move into the same tile as a piece of your own colour.");
                }

                return false;
            }

            return true;
        }
        
        public virtual void Move(Player player, Direction direction, Tile tile)
        {
            ValidateMove(player, direction, tile, true);

            if (tile.Piece != null)
            {
                tile.Piece.Take();
            }

            var startTile = Tile;
            Tile = tile;
            Tile.Piece = this;
            HasMovedThisTurn = true;
            transform.position = Tile.transform.position;

            CalculateIsFinishedMoving();
            AfterMove(player, direction, startTile, Tile);
        }

        public virtual void AfterMove(Player player, Direction direction, Tile startTile, Tile endTile)
        {
             // TODO
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
        
        public virtual void Place(Player player, Tile tile)
        {
            ValidatePlace(player, tile, true);
            
            Player = player;
            Tile = tile;
            Tile.Piece = this;
            IsTaken = false;
            HasMovedThisTurn = true;
            IsFinishedMoving = true;
            transform.position = Tile.transform.position;
        }

        public void Take()
        {
            Tile = null;
            IsTaken = true;
            
            // TODO: Animate the piece being destroyed somehow?
        }
        
        public void NextTurn()
        {
            HasMovedThisTurn = false;
            IsFinishedMoving = false;
        }

    }
}
