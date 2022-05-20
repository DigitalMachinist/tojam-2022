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
        //Debug.Log("GO BOOM");
        try
        {
            Tile.Board.GetTile(Tile, Direction.E, 1).Piece?.Take();
        }
        catch
        {
            // ignored
        }

        try
        {
            Tile.Board.GetTile(Tile, Direction.N, 1).Piece?.Take();
        }
        catch
        {
            // ignored
        }

        try
        {
            Tile.Board.GetTile(Tile, Direction.NE, 1).Piece?.Take();
        }
        catch
        {
            // ignored
        }

        try
        {
            Tile.Board.GetTile(Tile, Direction.NW, 1).Piece?.Take();
        }
        catch
        {
            // ignored
        }

        try
        {
            Tile.Board.GetTile(Tile, Direction.S, 1).Piece?.Take();
        }
        catch
        {
            // ignored
        }

        try
        {
            Tile.Board.GetTile(Tile, Direction.SE, 1).Piece?.Take();
        }
        catch
        {
            // ignored
        }

        try
        {
            Tile.Board.GetTile(Tile, Direction.SW, 1).Piece?.Take();
        }
        catch
        {
            // ignored
        }

        try
        {
                Tile.Board.GetTile(Tile, Direction.W, 1).Piece?.Take();
        }
        catch
        {
            // ignored
        }

        // Player.TurnAdvanced -= Explode;
        this.Take();
    }
}
