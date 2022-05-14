using Board;
using Exceptions;
using Managers;
using UnityEngine;

namespace States
{
    public class MovePieceState : State
    {
        public override void Enter()
        {
            base.Enter();
            
        }
        
        public override void Update()
        {
            base.Update();
            
            var manager = GameManager.Get();
            Tile tile = manager.Board.MouseSelectTile();
            if (tile == null)
            {
                return;
            }

            // TODO: Highlight tiles that the piece can move to.
            // TODO: Hover state on tiles that are valid moves.

            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }

            Debug.Log(tile.name);
            try
            {
                Debug.Log(manager.SelectedPiece);
                manager.CurrentPlayer.MovePiece(manager.SelectedPiece, tile);
                manager.SelectedPiece = null;
                // TODO: Check for if the piece stopped on a hidden event. If it did, go to the event state.
                manager.StateMachine.ChangeState(StateType.EndTurn);
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
