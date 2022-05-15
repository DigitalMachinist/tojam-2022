using Board;
using Exceptions;
using Managers;
using Pieces;
using Players;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Pieces
{
    public class Car : Piece
    {
        public override List<Piece> Move(Player player, Tile endTile)
        {

            var direction = Tile.Board.GetDirection(Tile, endTile);
            var distance = Tile.Board.GetDistance(Tile, endTile);
            Debug.Log($"Direction: {direction}");
            Debug.Log($"Distance: {distance}");



            int newDistance = 0;
            Tile newTile = null;
            Tile oldNewTile = null;

            for (int i = 1; i < 100; i++)
            {
                bool bob = false;
                try
                {
                    var tile = Tile.Board.GetTile(Tile, direction, i);
                    newTile = tile;
                }
                catch (IndexOutOfRangeException e)
                {
                    bob = true;
                }

                if (bob)
                {
                    break;
                }
                else
                {
                    oldNewTile = newTile;
                }
                newDistance++;
            }

            //Debug.Log("newDistance " + newDistance);
            endTile = oldNewTile;

            ValidateMove(player, Tile, endTile, direction, newDistance, true);

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

            this.Take();
            CalculateIsFinishedMoving();

            return pieces;
        }
        


        public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool ignoreTurn = false, bool throwExceptions = false)
        {
            // Cars can move cardinally or diagonally.
            if (direction == Direction.None)
            {
                if (throwExceptions)
                {
                    throw new MovementException("Cars can only move cardinally or diagonally.");
                }

                return false;
            }



            if (GameManager.Get().PlayerTurn != player.Colour)
            {
                if (throwExceptions)
                {
                    throw new PlacementException("Can't move a piece when it isn't your turn.");
                }

                return false;
            }

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

            Debug.Log("Pre delete");

            for (var d = 1; d < distance; d++)
            {
                var tile = Tile.Board.GetTile(startTile, direction, d);
                Debug.Log("tile " + tile.name);
                if (tile.Piece != null)
                {
                    Debug.Log("Take piece "+tile.Piece+" on " +tile.name);
                    player.TakePiece(tile.Piece); 
                }
            }
            //player.TakePiece(this);
            return true;
        }

    }
}
