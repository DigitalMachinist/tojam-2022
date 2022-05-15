using Board;
using Managers;

namespace States
{
    public class EndTurnState : State
    {
        public override void Enter()
        {
            base.Enter();

            var manager = GameManager.Get();
            if ( manager.CurrentPlayer.Colour == Players.PlayerColour.Black )
            {
                if ( manager.TurnNumber == manager.TurnPhase2 - 1 )
                {
                    manager.BeginPhase( 2 );
                }
                else if ( manager.TurnNumber == manager.TurnPhase3 - 1 )
                {
                    manager.BeginPhase( 3 );
                }
                
                // Handle apocalypse progress
                if (manager.CurrentPhase == 3)
                {
                    if (manager.ApocalypseTurnsLeft <= 0)
                    {
                        manager.DoApocalypseEvent();
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
                        tile.CrumblingState = (Tile.CrumblingStateTypes) manager.ApocalypseTurnsLeft;
                    }
                    
                    manager.ApocalypseTurnsLeft--;
                    return;
                }
            }

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

            var manager = GameManager.Get();
            if (!manager.IsPlayingApocalypseEvent)
            {
                manager.BeginOtherPlayerTurn();
                manager.StateMachine.ChangeState(StateType.BeginTurn);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
        }
    }
}
