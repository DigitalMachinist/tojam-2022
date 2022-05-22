using Managers;
using UnityEngine;

namespace States
{
    public class ConfirmCardState : State
    {
        public ConfirmCardState(GameManager gameManager) : base(gameManager)
        {
        }
        
        private void OnConfirmClicked()
        {
            var manager = GameManager.Get();
            manager.StateMachine.ChangeState(StateType.PlaceCard);
        }
        
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
            manager.InstructionText.text = "Play a card";
            manager.ConfirmCardDialog.SetActive(true);
            manager.ConfirmCardDialogCard.PopulateCardFields(manager.SelectedCard._cardSO, manager.CurrentPlayer.Colour);
            manager.ConfirmCardDialogCard.DisplayCard();
            manager.ConfirmCardDialogCard.Show();
            manager.ConfirmCardButton.gameObject.SetActive(true);
            manager.CancelCardButton.gameObject.SetActive(true);
            manager.ConfirmCardButton.onClick.AddListener(OnConfirmClicked);
            manager.CancelCardButton.onClick.AddListener(OnCancelClicked);
        }
        
        public override void Update()
        {
            base.Update();

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnCancelClicked();
            }
        }
        
        public override void Exit()
        {
            base.Exit();

            var manager = GameManager.Get();
            manager.ConfirmCardDialog.SetActive(false);
            manager.ConfirmCardButton.gameObject.SetActive(false);
            manager.CancelCardButton.gameObject.SetActive(false);
            manager.ConfirmCardButton.onClick.RemoveListener(OnConfirmClicked);
            manager.CancelCardButton.onClick.RemoveListener(OnCancelClicked);
        }
    }
}
