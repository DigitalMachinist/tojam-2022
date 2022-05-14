using Managers;

namespace States
{
    public class ConfirmCardState : State
    {
        private void OnConfirmClicked()
        {
            var manager = GameManager.Get();
            manager.StateMachine.ChangeState(StateType.PlaceCard);
        }
        
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
            // TODO: Make this display the current card.
            manager.ConfirmCardDialog.SetActive(true);
            manager.ConfirmCardButton.clicked += OnConfirmClicked;
            manager.CancelCardButton.clicked += OnCancelClicked;
        }
        
        public override void Update()
        {
            base.Update();
        }
        
        public override void Exit()
        {
            base.Exit();

            var manager = GameManager.Get();
            manager.ConfirmCardDialog.SetActive(false);
            manager.ConfirmCardButton.clicked -= OnConfirmClicked;
            manager.CancelCardButton.clicked -= OnCancelClicked;
        }
    }
}
