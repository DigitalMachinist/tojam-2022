using System;
using Board;
using Exceptions;
using Managers;
using Pieces;
using Players;
using UnityEngine;

public class Mine : Piece
{
    public GameObject explosion;

    public override bool ValidateSelect(Player player, bool ignoreTurn = false, bool throwExceptions = false)
    {
        if (throwExceptions)
        {
            throw new SelectionException("Mines cannot move");
        }

        return false;
    }

    public override bool ValidatePlace(Player player, Tile endTile, bool ignoreTurn = false, bool throwExceptions = false)
    {
        if (!ignoreTurn && GameManager.Get().PlayerTurn != player.Colour)
        {
            if (throwExceptions)
            {
                throw new PlacementException("Can't place a piece when it isn't your turn.");
            }

            return false;
        }


        if (endTile.IsDestroyed)
        {
            if (throwExceptions)
            {
                throw new PlacementException("Can't place onto a destroyed tile.");
            }

            return false;
        }

        if (endTile.Piece != null)
        {
            if (throwExceptions)
            {
                throw new PlacementException("Can't place onto the same space as another piece.");
            }

            return false;
        }

        return true;
    }

    public override bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool ignoreTurn = false, bool throwExceptions = false)
    {
        if (throwExceptions)
        {
            throw new MovementException("Mines cannot move");
        }

        return false;
    }
    
    public void Explode()
    {
        ExplodeTake(Direction.N);
        ExplodeTake(Direction.NE);
        ExplodeTake(Direction.E);
        ExplodeTake(Direction.SE);
        ExplodeTake(Direction.S);
        ExplodeTake(Direction.SW);
        ExplodeTake(Direction.W);
        ExplodeTake(Direction.NW);
        Instantiate(explosion, transform.position, explosion.transform.rotation);
        Take();
        GameManager.Get().Audio_PieceDestroy.Play();
    }

    private void ExplodeTake(Direction direction)
    {
        Tile tile;
        
        try
        {
            tile = Tile.Board.GetTile(Tile, direction, 1);
        }
        catch (IndexOutOfRangeException e)
        {
            return;
        }
        
        if (tile == null || tile.IsDestroyed)
        {
            return;
        }
        
        var piece = tile.Piece;
        if (piece == null || piece.IsTaken)
        {
            return;
        }
        
        piece.Take();
    }
}
