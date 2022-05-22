using Managers;
using UnityEngine;

namespace States
{
    public class State
    {
        public GameManager GameManager { get; private set; }

        public State(GameManager gameManager)
        {
            GameManager = gameManager;
        }
        
        public virtual void Enter()
        {
            Debug.Log($"State enter: {this.GetType().Name}");
        }

        public virtual void Update()
        {
            
        }
        
        public virtual void Exit()
        {
            // Debug.Log($"State exit: {this.GetType().Name}");
        }
    }
}
