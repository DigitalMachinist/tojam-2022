using Board;
using Managers;
using UnityEngine;

namespace States
{
    public class PlaceCardState : State
    {
        public override void Enter()
        {
            base.Enter();
            
            var manager = GameManager.Get();
            
            // TODO: Make this display the current card.
            manager.PlaceCardDisplay.SetActive(true);
        }
        
        public override void Update()
        {
            base.Update();
            
            var manager = GameManager.Get();
            Tile tile = manager.Board.MouseSelectTile();
            if (tile == null)
            {
                return;
            }
            
            // TODO: Enable tile hover state
            
            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }
            
            Debug.Log($"Play {manager.SelectedCard.Title} at {tile.name}");
            manager.SelectedCard.Play();
            // TODO: Need a factory to convert card type into a piece or a board effect to actually place something here.
            manager.StateMachine.ChangeState(StateType.SelectPiece);
        }
        
        public override void Exit()
        {
            base.Exit();
            
            var manager = GameManager.Get();
            manager.PlaceCardDisplay.SetActive(false);
            manager.SelectedPiece = null;
        }
    }
}
