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
        }
        
        public override void Update()
        {
            var manager = GameManager.Get();
            Tile tile = manager.Board.MouseSelectTile();
            if (tile == null)
            {
                return;
            }

            if (tile.Piece != null && tile.Piece.Player == manager.CurrentPlayer)
            {
                // TODO: Hover state on tiles that contain selectable pieces.
            }
            
            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }

            if (tile.Piece == null)
            {
                return;
            }

            Debug.Log(tile.name);
            Debug.Log(tile.Piece);
            try
            {
                manager.SelectedPiece = tile.Piece;
                manager.StateMachine.ChangeState(StateType.MovePiece);
            }
            catch (SelectionException e)
            {
                Debug.LogException(e);
                return;
            }
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}
