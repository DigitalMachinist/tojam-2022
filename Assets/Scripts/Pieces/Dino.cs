using Board;
using Exceptions;
using Managers;
using Pieces;
using Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : Piece
{

    public override List<Piece> Move(Player player, Tile endTile)
    {
        var direction = Tile.Board.GetDirection(Tile, endTile);
        var distance = Tile.Board.GetDistance(Tile, endTile);
        Debug.Log($"Direction: {direction}");
        Debug.Log($"Distance: {distance}");
        ValidateMove(player, Tile, endTile, direction, distance, false, true);


        try
        {
            Tile.Board.GetTile(Tile, Direction.E, 1).Piece?.Take();
        }
        catch
        {
        }
        try
        {
            Tile.Board.GetTile(Tile, Direction.N, 1).Piece?.Take();
        }
        catch
        {
        }
        try
        {
            Tile.Board.GetTile(Tile, Direction.NE, 1).Piece?.Take();
        }
        catch
        {
        }
        try
        {
            Tile.Board.GetTile(Tile, Direction.NW, 1).Piece?.Take();
        }
        catch
        {
        }
        try
        {
            Tile.Board.GetTile(Tile, Direction.S, 1).Piece?.Take();
        }
        catch
        {
        }
        try
        {
            Tile.Board.GetTile(Tile, Direction.SE, 1).Piece?.Take();
        }
        catch
        {
        }
        try
        {
            Tile.Board.GetTile(Tile, Direction.SW, 1).Piece?.Take();
        }
        catch
        {
        }
        try
        {
            Tile.Board.GetTile(Tile, Direction.W, 1).Piece?.Take();
        }
        catch
        {
        }


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

        GameManager.Get().Audio_PieceSelect.Play();

        return pieces;
    }

    public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool ignoreTurn, bool throwExceptions = false)
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
                throw new MovementException("Dinos can only move cardinally or diagonally.");
            }

            return false;
        }

        // Kings can move only 1 tile at a time.
        if (distance > 3)
        {
            if (throwExceptions)
            {
                throw new MovementException("Dinos can only move up to 3 tile at a time.");
            }

            return false;
        }

        //// Queens can't move through other pieces.
        //for (var d = 1; d < distance; d++)
        //{
        //    var tile = Tile.Board.GetTile(startTile, direction, d);
        //    if (tile.Piece != null)
        //    {
        //        //if (throwExceptions)
        //        //{
        //        //    throw new MovementException(" can't move through other pieces.");
        //        //}

        //        return false;
        //    }
        //}


        return true;
    }
}
