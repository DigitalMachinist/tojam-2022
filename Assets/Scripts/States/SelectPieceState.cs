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

            Debug.Log(tile.name);
            try
            {
                manager.SelectedPiece = tile.Piece;
                Debug.Log(manager.SelectedPiece);
                manager.StateMachine.ChangeState(StateType.MovePiece);
            }
            catch (SelectionException e)
            {
                Debug.LogException(e);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}
