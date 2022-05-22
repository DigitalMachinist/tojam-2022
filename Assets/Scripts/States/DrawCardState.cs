using Managers;

namespace States
{
    public class DrawCardState : State
    {
        public DrawCardState(GameManager gameManager) : base(gameManager)
        {
        }
        
        private void OnDrawClicked()
        {
            GameManager.CurrentPlayerHand.NewTurn(GameManager.CurrentPlayer.Colour);
            GameManager.CurrentPlayerHand.RefreshHandCards();
            GameManager.CurrentPlayerHand.DrawnCard.Hide();
            GameManager.DrawCardButton.gameObject.SetActive(true);
            GameManager.StateMachine.ChangeState(StateType.EndTurn);
        }

        private void OnDeckHovered()
        {
            GameManager.Deck.AnimateDeckNonTwisty(0.5f);
        }

        private void OnDeckUnhovered()
        {
            GameManager.Deck.AnimateDeckTwisty(0.5f);
        }
        
        public override void Enter()
        {
            base.Enter();
            
            if (GameManager.TurnNumber < GameManager.TurnPhase2)
            {
                // Only draw new cards after cards are in the game.
                GameManager.StateMachine.ChangeState(StateType.EndTurn);
                return;
            }
            
            GameManager.InstructionText.text = "Draw a card";
            GameManager.DrawCardButton.gameObject.SetActive(true);
            GameManager.DrawCardButton.onClick.AddListener(OnDrawClicked);

            GameManager.Deck.Hovered += OnDeckHovered;
            GameManager.Deck.Unhovered += OnDeckUnhovered;
            
            GameManager.Deck.AnimateDeckTwisty(1f);
        }
        
        public override void Update()
        {
            base.Update();
        }
        
        public override void Exit()
        {
            base.Exit();
            
            GameManager.Deck.RenderReset();
            GameManager.DrawCardButton.gameObject.SetActive(false);
            GameManager.DrawCardButton.onClick.RemoveListener(OnDrawClicked);
            
            GameManager.Deck.Hovered -= OnDeckHovered;
            GameManager.Deck.Unhovered -= OnDeckUnhovered;
        }
    }
}
