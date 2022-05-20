using System;
using Board;
using Exceptions;
using Pieces;
using Players;

public class Mine : Piece
{
    public override bool ValidateSelect(Player player, bool ignoreTurn = false, bool throwExceptions = false)
    {
        if (throwExceptions)
        {
            throw new SelectionException("Mines cannot move");
        }

        return false;
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
        Take();
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
