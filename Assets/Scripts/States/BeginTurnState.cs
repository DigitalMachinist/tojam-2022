using Managers;

namespace States
{
    public class BeginTurnState : State
    {
        public override void Enter()
        {
            base.Enter();

            var manager = GameManager.Get();
            if (manager.CheckGameOverCondition())
            {
                return;
            }
            
            if (manager.TurnNumber < manager.TurnPhase2)
            {
                // Go straight to piece selection in the early game.
                manager.PlayerHandBlackCanvas.SetActive(false);
                manager.PlayerHandWhiteCanvas.SetActive(false);
                manager.Deck.gameObject.SetActive(false);
                
                manager.StateMachine.ChangeState(StateType.SelectPiece);
            }
            else
            {
                // Once cards are in the game, go to card select next.
                manager.PlayerHandBlackCanvas.SetActive(true);
                manager.PlayerHandWhiteCanvas.SetActive(true);
                manager.Deck.gameObject.SetActive(true);
                
                manager.StateMachine.ChangeState(StateType.SelectCard);
            }
        }
        
        public override void Update()
        {
            base.Update();
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}
