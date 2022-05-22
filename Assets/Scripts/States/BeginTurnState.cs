using System.Collections.Generic;
using System.Linq;
using Board;
using Managers;
using UnityEngine;

namespace States
{
    public class BeginTurnState : State
    {
        private ICollection<Mine> minesToExplode;

        public BeginTurnState(GameManager gameManager) : base(gameManager)
        {
        }
        
        public override void Enter()
        {
            base.Enter();

            if (GameManager.CheckGameOverCondition())
            {
                return;
            }
            
            if (GameManager.CurrentPlayer.Colour == Players.PlayerColour.White)
            {
                if (GameManager.TurnNumber == GameManager.TurnPhase2)
                {
                    GameManager.BeginPhase(2);
                }
                else if (GameManager.TurnNumber == GameManager.TurnPhase3)
                {
                    GameManager.BeginPhase(3);
                }
                
                // Handle apocalypse progress
                if (GameManager.CurrentPhase == 3)
                {
                    if (GameManager.ApocalypseTurnsLeft <= 0)
                    {
                        GameManager.DoApocalypseEvent();
                        return;
                    }
                    
                    // Clear the entire board's tile state.
                    foreach (var tile in GameManager.Board.Tiles)
                    {
                        tile.TileState = tile.IsDestroyed
                            ? Tile.TileStateTypes.Destroyed
                            : Tile.TileStateTypes.Normal;
                    }
                    
                    // Mark tiles that are crumbling as such.
                    foreach (var tile in GameManager.ApocalypseTiles)
                    {
                        tile.TileState = Tile.TileStateTypes.Crumbling;
                        tile.CrumblingState = (Tile.CrumblingStateTypes) Mathf.Max(0, 2 - GameManager.ApocalypseTurnsLeft);
                    }
                    
                    GameManager.ApocalypseTurnsLeft--;
                }
            }

            minesToExplode = GameManager
                .CurrentPlayer
                .Pieces
                .Select(piece => piece.GetComponent<Mine>())
                .Where(mine => mine != null && !mine.IsTaken)
                .ToList();
        }
        
        public override void Update()
        {
            base.Update();

            if (GameManager.IsPlayingApocalypseEvent)
            {
                return;
            }

            // Explode any mines placed last turn.
            if (minesToExplode.Any())
            {
                foreach (var mine in minesToExplode)
                {
                    mine.Explode();
                }

                minesToExplode.Clear();
            }
            
            // This should only run once after the apocalypse event ends.
            if (GameManager.TurnNumber < GameManager.TurnPhase2)
            {
                // Go straight to piece selection in the early game.
                GameManager.PlayerHandBlackCanvas.SetActive(false);
                GameManager.PlayerHandWhiteCanvas.SetActive(false);
                GameManager.Deck.gameObject.SetActive(false);
                
                GameManager.StateMachine.ChangeState(StateType.SelectPiece);
            }
            else
            {
                // Once cards are in the game, go to card select next.
                GameManager.PlayerHandBlackCanvas.SetActive(true);
                GameManager.PlayerHandWhiteCanvas.SetActive(true);
                GameManager.Deck.gameObject.SetActive(true);
                
                GameManager.StateMachine.ChangeState(StateType.SelectCard);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}
