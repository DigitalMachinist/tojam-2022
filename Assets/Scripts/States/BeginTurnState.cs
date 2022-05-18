using Board;
using Managers;
using UnityEngine;

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
            
            if (manager.CurrentPlayer.Colour == Players.PlayerColour.White)
            {
                if (manager.TurnNumber == manager.TurnPhase2)
                {
                    manager.BeginPhase(2);
                }
                else if (manager.TurnNumber == manager.TurnPhase3)
                {
                    manager.BeginPhase(3);
                }
                
                // Handle apocalypse progress
                if (manager.CurrentPhase == 3)
                {
                    if (manager.ApocalypseTurnsLeft <= 0)
                    {
                        manager.DoApocalypseEvent();
                        return;
                    }
                    
                    // Clear the entire board's tile state.
                    foreach (var tile in manager.Board.Tiles)
                    {
                        tile.TileState = tile.IsDestroyed
                            ? Tile.TileStateTypes.Destroyed
                            : Tile.TileStateTypes.Normal;
                    }
                    
                    // Mark tiles that are crumbling as such.
                    foreach (var tile in manager.ApocalypseTiles)
                    {
                        tile.TileState = Tile.TileStateTypes.Crumbling;
                        tile.CrumblingState = (Tile.CrumblingStateTypes) Mathf.Max(0, 2 - manager.ApocalypseTurnsLeft);
                    }
                    
                    manager.ApocalypseTurnsLeft--;
                }
            }
            
        }
        
        public override void Update()
        {
            base.Update();

            var manager = GameManager.Get();
            if (manager.IsPlayingApocalypseEvent)
            {
                return;
            }
            
            // This should only run once after the apocalypse event ends.
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
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}
