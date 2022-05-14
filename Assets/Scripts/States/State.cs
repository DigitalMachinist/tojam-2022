using Managers;
using UnityEngine;

namespace States
{
    public class State
    {
        public virtual void Enter()
        {
            Debug.Log($"State enter: {this.GetType().Name}");
        }

        public virtual void Update()
        {
            
        }
        
        public virtual void Exit()
        {
            Debug.Log($"State exit: {this.GetType().Name}");
        }
    }
}
