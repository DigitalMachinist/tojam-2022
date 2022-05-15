using Board;
using Exceptions;
using Managers;
using UnityEngine;

namespace States
{
    public class SelectPieceState : State
    {
        public override void Enter()
        {
            base.Enter();
            
            var manager = GameManager.Get();
            manager.InstructionText.text = "Make a move";
            
            // Clear all hover and selection states.
            foreach (var tile in manager.Board.Tiles)
            {
                tile.IsHovering = false;
                tile.SelectionState = Tile.SelectionStateTypes.None;
            }
            
            // Highlight tiles where selectable pieces are.
            foreach (var piece in manager.CurrentPlayer.Pieces)
            {
                if (piece.ValidateSelect(manager.CurrentPlayer))
                {
                    piece.Tile.SelectionState = Tile.SelectionStateTypes.Selected;
                }
            }
        }
        
        public override void Update()
        {
            base.Update();
            
            var manager = GameManager.Get();
            
            // Clear all hover states.
            foreach (var tile in manager.Board.Tiles)
            {
                tile.IsHovering = false;
            }

            Tile hoveredTile = manager.Board.MouseSelectTile();
            if (hoveredTile == null)
            {
                return;
            }

            hoveredTile.IsHovering = hoveredTile.Piece != null && hoveredTile.Piece.ValidateSelect(manager.CurrentPlayer);
            
            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }

            if (hoveredTile.Piece == null)
            {
                return;
            }

            Debug.Log(hoveredTile.name);
            Debug.Log(hoveredTile.Piece);
            try
            {
                hoveredTile.Piece.Select(manager.CurrentPlayer);
            }
            catch (SelectionException e)
            {
                Debug.LogException(e);
                return;
            }
            
            manager.StateMachine.ChangeState(StateType.MovePiece);
        }
        
        public override void Exit()
        {
            base.Exit();
            
            var manager = GameManager.Get();
            
            // Clear all selection states.
            foreach (var tile in manager.Board.Tiles)
            {
                tile.IsHovering = false;
                tile.SelectionState = Tile.SelectionStateTypes.None;
            }
        }
    }
}
