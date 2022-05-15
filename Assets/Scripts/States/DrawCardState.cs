using Managers;

namespace States
{
    public class DrawCardState : State
    {
        private void OnDrawClicked()
        {
            var manager = GameManager.Get();
            manager.CurrentPlayerHand.NewTurn(manager.CurrentPlayer.Colour);
            manager.CurrentPlayerHand.RefreshHandCards();
            manager.CurrentPlayerHand.DrawnCard.Hide();
            manager.DrawCardButton.gameObject.SetActive(true);
            manager.StateMachine.ChangeState(StateType.EndTurn);
        }
        
        public override void Enter()
        {
            base.Enter();
            
            var manager = GameManager.Get();
            if (manager.TurnNumber < manager.TurnPhase2)
            {
                // Only draw new cards after cards are in the game.
                manager.StateMachine.ChangeState(StateType.EndTurn);
                return;
            }
            
            manager.InstructionText.text = "Draw a card";
            manager.DrawCardButton.gameObject.SetActive(true);
            manager.DrawCardButton.onClick.AddListener(OnDrawClicked);
        }
        
        public override void Update()
        {
            base.Update();
            
            var manager = GameManager.Get();
            if (manager.Deck.IsHoveringDeck())
            {
                manager.Deck.RenderHovered();
            }
            else
            {
                manager.Deck.RenderReset();
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            
            var manager = GameManager.Get();
            manager.Deck.RenderReset();
            manager.DrawCardButton.gameObject.SetActive(false);
            manager.DrawCardButton.onClick.RemoveListener(OnDrawClicked);
        }
    }
}
