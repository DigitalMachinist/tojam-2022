using Board;
using Exceptions;
using Managers;
using UnityEngine;

namespace States
{
    public class MovePieceState : State
    {
        public MovePieceState(GameManager gameManager) : base(gameManager)
        {
        }
        
        private void OnCancelClicked()
        {
            var manager = GameManager.Get();
            manager.ClearSelectedPiece();
            manager.StateMachine.ChangeState(StateType.SelectPiece);
        }
        
        public override void Enter()
        {
            base.Enter();
            
            var manager = GameManager.Get();
            manager.InstructionText.text = "Make a move";
            if (GameManager.CurrentPhase == 1)
            {
                manager.CancelMoveButton.gameObject.SetActive(false);
                manager.CancelMoveButtonSensible.gameObject.SetActive(true);
                manager.CancelMoveButtonSensible.onClick.AddListener(OnCancelClicked);
            }
            else
            {
                manager.CancelMoveButton.gameObject.SetActive(true);
                manager.CancelMoveButtonSensible.gameObject.SetActive(false);
                manager.CancelMoveButton.onClick.AddListener(OnCancelClicked);
            }

            
            // Clear all hover and selection states.
            foreach (var tile in manager.Board.Tiles)
            {
                tile.IsHovering = false;
                tile.SelectionState = Tile.SelectionStateTypes.None;
            }

            // Highlight the selected piece.
            manager.SelectedPiece.Tile.SelectionState = Tile.SelectionStateTypes.Selected;
            
            // Highlight the tiles the piece can move to.
            foreach (var tile in manager.SelectedPiece.GetValidMoves())
            {
                tile.SelectionState = tile.Piece == null
                    ? Tile.SelectionStateTypes.Available
                    : Tile.SelectionStateTypes.Danger;
            }
        }
        
        public override void Update()
        {
            base.Update();

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnCancelClicked();
            }
            
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
            
            // TODO: Hover state on tiles that are valid moves.
            var direction = manager.Board.GetDirection(manager.SelectedPiece.Tile, hoveredTile);
            var distance = manager.Board.GetDistance(manager.SelectedPiece.Tile, hoveredTile);
            hoveredTile.IsHovering = manager.SelectedPiece.ValidateMove(manager.CurrentPlayer, manager.SelectedPiece.Tile, hoveredTile, direction, distance);

            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }

            Debug.Log(hoveredTile.name);
            Debug.Log(manager.SelectedPiece);
            try
            {
                manager.CurrentPlayer.MovePiece(manager.SelectedPiece, hoveredTile);
            }
            catch (MovementException e)
            {
                Debug.LogException(e);
                return;
            }
            
            manager.ClearSelectedPiece();
            // TODO: Check for if the piece stopped on a hidden event. If it did, go to the event state.
            manager.StateMachine.ChangeState(StateType.DrawCard);
        }
        
        public override void Exit()
        {
            base.Exit();
            
            var manager = GameManager.Get();
            manager.CancelMoveButton.gameObject.SetActive(false);
            manager.CancelMoveButton.onClick.RemoveListener(OnCancelClicked);
            manager.CancelMoveButtonSensible.gameObject.SetActive(false);
            manager.CancelMoveButtonSensible.onClick.RemoveListener(OnCancelClicked);
            
            // Clear all hover and selection states.
            foreach (var tile in manager.Board.Tiles)
            {
                tile.HasPiece = tile.Piece != null;
                tile.IsHovering = false;
                tile.SelectionState = Tile.SelectionStateTypes.None;
            }
        }
    }
}
