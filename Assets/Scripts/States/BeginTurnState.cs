using Managers;

namespace States
{
    public class BeginTurnState : State
    {
        public override void Enter()
        {
            base.Enter();

            var manager = GameManager.Get();
            if (manager.CurrentPlayer.HasLost)
            {
                // Detect lose condition and switch to the game over state.
                manager.Winner = manager.GetOtherPlayer(manager.CurrentPlayer.Colour);
                manager.Deck.SetActive(false);
                manager.PlayerHandBlack.gameObject.SetActive(false);
                manager.PlayerHandWhite.gameObject.SetActive(false);
                
                manager.StateMachine.ChangeState(StateType.GameOver);
            }
            else if (manager.TurnNumber < manager.TurnShowCards)
            {
                // Go straight to piece selection in the early game.
                // manager.Deck.SetActive(false);
                // manager.PlayerHandBlack.gameObject.SetActive(false);
                // manager.PlayerHandWhite.gameObject.SetActive(false);
                
                manager.StateMachine.ChangeState(StateType.SelectPiece);
            }
            else
            {
                // Once cards are in the game, go to card select next.
                manager.Deck.SetActive(true);
                manager.PlayerHandBlack.gameObject.SetActive(true);
                manager.PlayerHandWhite.gameObject.SetActive(true);
                
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
