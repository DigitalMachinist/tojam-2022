using System;
using System.Collections.Generic;
using Board;
using Exceptions;
using Managers;
using Players;
using UnityEngine;

namespace Pieces
{
    public class Piece : MonoBehaviour
    {
        public Player Player;
        public Tile Tile;
        public bool IsTaken;
        public int NumMovesPerformed;
        public bool HasMovedThisTurn;
        public bool IsFinishedMoving;
        public GameObject Content;
        public GameObject ContentPrefabBlack;
        public GameObject ContentPrefabWhite;

        public event Action<Piece> FinishedMove;

        public virtual void Select(Player player, bool ignoreTurn = false)
        {
            ValidateSelect(player, ignoreTurn, true);
            
            GameManager.Get().SelectedPiece = this;

            GameManager.Get().Audio_PieceSelect.Play();
        }
        
        public virtual bool ValidateSelect(Player player, bool ignoreTurn = false, bool throwExceptions = false)
        {
            if (!ignoreTurn && GameManager.Get().PlayerTurn != player.Colour)
            {
                if (throwExceptions)
                {
                    throw new SelectionException("Can't select a piece when it isn't your turn.");
                }

                return false;
            }
            
            if (GameManager.Get().SelectedPiece != null)
            {
                if (throwExceptions)
                {
                    throw new SelectionException("Can't select a piece when there is already a selected piece.");
                }

                return false;
            }
            
            if (IsTaken)
            {
                if (throwExceptions)
                {
                    throw new SelectionException("Can't select a taken piece.");
                }
                
                return false;
            }

            if (player != Player)
            {
                if (throwExceptions)
                {
                    throw new SelectionException("Can't select a piece that isn't yours.");
                }

                return false;
            }

            if (IsFinishedMoving)
            {
                if (throwExceptions)
                {
                    throw new SelectionException("Can't select this piece again until next turn.");
                }

                return false;
            }

            return true;
        }
        
        public virtual void Place(Player player, Tile tile, bool ignoreTurn = false)
        {
            ValidatePlace(player, tile, ignoreTurn, true);
            
            Player = player;
            Tile = tile;
            Tile.Piece = this;
            IsTaken = false;
            NumMovesPerformed = 0;
            HasMovedThisTurn = false;
            IsFinishedMoving = false;
            transform.position = Tile.transform.position;
            Content = (player.Colour == PlayerColour.Black)
                ? Instantiate(ContentPrefabBlack, transform, false)
                : Instantiate(ContentPrefabWhite, transform, false);

            GameManager.Get().Audio_PieceSelect.Play();
        }

        public virtual bool ValidatePlace(Player player, Tile endTile, bool ignoreTurn = false, bool throwExceptions = false)
        {
            if (!ignoreTurn && GameManager.Get().PlayerTurn != player.Colour)
            {
                if (throwExceptions)
                {
                    throw new PlacementException("Can't place a piece when it isn't your turn.");
                }

                return false;
            }

            if (player.Colour == PlayerColour.White && endTile.Row > 4 || player.Colour == PlayerColour.Black && endTile.Row < 5)
            {
                if (throwExceptions)
                {
                    throw new PlacementException("Can't place into the other player's board area.");
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
        
        public virtual List<Piece> Move(Player player, Tile endTile)
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
            }

            Tile.Piece = null;
            Tile = endTile;
            Tile.Piece = this;
            HasMovedThisTurn = true;
            NumMovesPerformed++;
            
            // TODO: Tween this instead?
            transform.position = Tile.transform.position;
            
            CalculateIsFinishedMoving();

            GameManager.Get().Audio_PieceSelect.Play();

            return pieces;
        }
        
        public virtual bool ValidateMove(Player player, Tile startTile, Tile endTile, Direction direction, int distance, bool ignoreTurn = false, bool throwExceptions = false)
        {
            if (!ignoreTurn && GameManager.Get().PlayerTurn != player.Colour)
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

            if (!ignoreTurn && IsFinishedMoving)
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

        public void Take()
        {
            if (Tile != null)
            {
                if(Tile.Piece != null)
                    Tile.Piece = null;

                Tile.HasPiece = false;
                Tile = null;
                IsTaken = true;
            }

            if (Player.Pieces.Contains(this)) 
                Player.Pieces.Remove(this);
            
            // TODO: Animate the piece being destroyed somehow?
            // transform.position = Vector3.zero;
            gameObject.SetActive(false);
        }
        
        public void NextTurn()
        {
            HasMovedThisTurn = false;
            IsFinishedMoving = false;
        }

        public virtual void CalculateIsFinishedMoving()
        {
            // By default, if something has moved this turn it can't move again.
            // Checkers pieces (for example) will require more complicated logic here.
            IsFinishedMoving = true;

            FinishedMove?.Invoke(this);
        }

        public virtual List<Tile> GetValidMoves(bool ignoreTurn = false)
        {
            var tiles = new List<Tile>();
            foreach (var endTile in GameManager.Get().Board.Tiles)
            {
                var direction = Tile.Board.GetDirection(Tile, endTile);
                var distance = Tile.Board.GetDistance(Tile, endTile);
                if (ValidateMove(Player, Tile, endTile, direction, distance, ignoreTurn))
                {
                    tiles.Add(endTile);
                }
            }

            return tiles;
        }

        public virtual List<Tile> GetValidPlaces(bool ignoreTurn = false)
        {
            var tiles = new List<Tile>();
            foreach (var endTile in GameManager.Get().Board.Tiles)
            {
                if (ValidatePlace(Player, endTile, ignoreTurn))
                {
                    tiles.Add(endTile);
                }
            }

            return tiles;
        }
    }
}
