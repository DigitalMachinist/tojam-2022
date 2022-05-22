using Managers;

namespace States
{
    public class EventState : State
    {
        public EventState(GameManager gameManager) : base(gameManager)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            
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
