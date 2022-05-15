using Board;
using Managers;
using UnityEngine;

namespace States
{
    public class PlaceCardState : State
    {
        private void OnCancelClicked()
        {
            var manager = GameManager.Get();
            manager.SelectedCard = null;
            manager.StateMachine.ChangeState(StateType.SelectCard);
        }
        
        public override void Enter()
        {
            base.Enter();
            
            var manager = GameManager.Get();
            manager.InstructionText.text = "Choose a space";
            // TODO: Make this display the current card.
            manager.PlaceCardDisplay.SetActive(true);
            manager.CancelCardButton.onClick.AddListener(OnCancelClicked);
        }
        
        public override void Update()
        {
            base.Update();

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnCancelClicked();
            }
            
            var manager = GameManager.Get();
            Tile tile = manager.Board.MouseSelectTile();
            if (tile == null)
            {
                return;
            }
            
            // TODO: Enable tile hover state
            
            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }
            
            Debug.Log($"Play {manager.SelectedCard.Title} at {tile.name}");
            manager.SelectedCard.Play();
            // TODO: Need a factory to convert card type into a piece or a board effect to actually place something here.
            manager.StateMachine.ChangeState(StateType.SelectPiece);
        }
        
        public override void Exit()
        {
            base.Exit();
            
            var manager = GameManager.Get();
            manager.PlaceCardDisplay.SetActive(false);
            manager.ClearSelectedPiece();
            manager.CancelCardButton.onClick.RemoveListener(OnCancelClicked);
        }
    }
}
