using Managers;

namespace States
{
    public class EndTurnState : State
    {
        public override void Enter()
        {
            base.Enter();

            var manager = GameManager.Get();
            manager.CurrentPlayerHand.NewTurn();
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
