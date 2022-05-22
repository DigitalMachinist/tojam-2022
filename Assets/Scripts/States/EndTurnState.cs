using Board;
using Managers;
using UnityEngine;

namespace States
{
    public class EndTurnState : State
    {
        public EndTurnState(GameManager gameManager) : base(gameManager)
        {
        }
        
        public override void Enter()
        {
            base.Enter();

            var manager = GameManager.Get();
            if ( manager.TurnNumber >= manager.TurnPhase2)
            {
                // Only draw new cards after cards are in the game.
                manager.CurrentPlayerHand.NewTurn(manager.CurrentPlayer.Colour);
            }
            
            manager.BeginOtherPlayerTurn();
            manager.StateMachine.ChangeState(StateType.BeginTurn);
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
