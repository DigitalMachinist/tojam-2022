using Board;
using Exceptions;
using Managers;
using Pieces;
using Players;
using System;
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


        


        // Keep track of pieces to return as taken.
        var pieces = new List<Piece>();
        if (endTile.Piece != null)
        {
            pieces.Add(endTile.Piece);
            //tile.Destroy();
        }

        Tile.Piece = null;
        Tile = endTile;
        Tile.Piece = this;
        HasMovedThisTurn = true;

        // TODO: Tween this instead?
        transform.position = Tile.transform.position;

        CalculateIsFinishedMoving();

        Stomp();

        return pieces;
    }
    public void Stomp()
    {
        bool destroyedAnyPiece = false;                                      
        destroyedAnyPiece = StompTake(Direction.N)   || destroyedAnyPiece;
        destroyedAnyPiece = StompTake(Direction.NE)  || destroyedAnyPiece;
        destroyedAnyPiece = StompTake(Direction.E)   || destroyedAnyPiece;
        destroyedAnyPiece = StompTake(Direction.SE)  || destroyedAnyPiece;
        destroyedAnyPiece = StompTake(Direction.S)   || destroyedAnyPiece;
        destroyedAnyPiece = StompTake(Direction.SW)  || destroyedAnyPiece;
        destroyedAnyPiece = StompTake(Direction.W)   || destroyedAnyPiece;
        destroyedAnyPiece =  StompTake(Direction.NW) || destroyedAnyPiece;

        if (destroyedAnyPiece)
        {
            Debug.Log("Dino stomped a piece.");
            Tile.Destroy();
            Take();
            GameManager.Get().Audio_DinoRoar.Play();
        }
    }

    private bool StompTake(Direction direction)
    {
        Tile tile;

        try
        {
            tile = Tile.Board.GetTile(Tile, direction, 1);
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }

        if (tile == null || tile.IsDestroyed)
        {
            return false;
        }

        var piece = tile.Piece;
        if (piece == null || piece.IsTaken)
        {
            return false;
        }

        piece.Take();
        tile.Destroy();

        return true;
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
