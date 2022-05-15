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
            manager.ClearSelectedCard();
            manager.StateMachine.ChangeState(StateType.SelectCard);
        }
        
        public override void Enter()
        {
            base.Enter();
            
            var manager = GameManager.Get();
            manager.InstructionText.text = "Choose a space";
            manager.PlaceCardDisplayCard.PopulateCardFields(manager.SelectedCard._cardSO);
            manager.PlaceCardDisplayCard.DisplayCard();
            manager.PlaceCardDisplayCard.Show();
            manager.PlaceCardDisplay.SetActive(true);
            manager.ConfirmCardButton.gameObject.SetActive(false);
            manager.CancelCardButton.gameObject.SetActive(false);
            manager.CancelPlaceButton.gameObject.SetActive(true);
            manager.CancelPlaceButton.onClick.AddListener(OnCancelClicked);
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
            manager.CurrentPlayer.PlaceCard(manager.SelectedCard._cardSO, tile);
            manager.ClearSelectedCard();
            manager.StateMachine.ChangeState(StateType.SelectPiece);
        }
        
        public override void Exit()
        {
            base.Exit();
            
            var manager = GameManager.Get();
            manager.ClearSelectedPiece();
            manager.PlaceCardDisplay.SetActive(false);
            manager.ConfirmCardButton.gameObject.SetActive(false);
            manager.CancelCardButton.gameObject.SetActive(false);
            manager.CancelPlaceButton.gameObject.SetActive(false);
            manager.CancelPlaceButton.onClick.RemoveListener(OnCancelClicked);
        }
    }
}
