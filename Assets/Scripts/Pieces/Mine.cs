using Board;
using Exceptions;
using Pieces;
using Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Piece
{

    private void Start()
    {
        Player.TurnAdvanced += OnPlayerTurnAdvanced;
    }

    private void OnPlayerTurnAdvanced()
    {
        //Debug.Log("GO BOOM");
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
        this.Take();
    }

    public override List<Piece> Move(Player player, Tile endTile)
    {
        return base.Move(player, endTile);
    }

    public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool ignoreTurn = false, bool throwExceptions = false)
    {
        if (throwExceptions)
        {
            throw new MovementException("Mines cannot move");
        }

        return false;
    }
}
