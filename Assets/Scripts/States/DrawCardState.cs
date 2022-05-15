using Managers;

namespace States
{
    public class DrawCardState : State
    {
        private void OnDrawClicked()
        {
            var manager = GameManager.Get();
            manager.CurrentPlayerHand.NewTurn(manager.CurrentPlayer.Colour);
            manager.StateMachine.ChangeState(StateType.SelectPiece);
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
            manager.DrawCardButton.onClick.AddListener(OnDrawClicked);
        }
        
        public override void Update()
        {
            base.Update();
        }
        
        public override void Exit()
        {
            base.Exit();
            
            var manager = GameManager.Get();
            manager.DrawCardButton.onClick.RemoveListener(OnDrawClicked);
        }
    }
}
