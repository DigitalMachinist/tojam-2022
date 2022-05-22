using System;
using Managers;

namespace States
{
    public class StateMachine
    {
        public GameManager GameManager;
        public StateType CurrentStateType;
        public State State;
        
        public BeginTurnState BeginTurn;
        public SelectCardState SelectCard;
        public ConfirmCardState ConfirmCard;
        public PlaceCardState PlaceCard;
        public SelectPieceState SelectPiece;
        public MovePieceState MovePiece;
        public EventState Event;
        public DrawCardState DrawCard;
        public EndTurnState EndTurn;
        public GameOverState GameOver;

        public StateMachine(GameManager gameManager)
        {
            GameManager = gameManager;
            CurrentStateType = StateType.None;
            State = null;
            
            BeginTurn = new BeginTurnState(gameManager);
            SelectCard = new SelectCardState(gameManager);
            ConfirmCard = new ConfirmCardState(gameManager);
            PlaceCard = new PlaceCardState(gameManager);
            SelectPiece = new SelectPieceState(gameManager);
            MovePiece = new MovePieceState(gameManager);
            Event = new EventState(gameManager);
            DrawCard = new DrawCardState(gameManager);
            EndTurn = new EndTurnState(gameManager);
            GameOver = new GameOverState(gameManager);
        }

        public void Update()
        {
            State?.Update();
        }

        public void ChangeState(StateType type)
        {
            State newState = type switch
            {
                StateType.BeginTurn   => BeginTurn,
                StateType.SelectCard  => SelectCard,
                StateType.ConfirmCard => ConfirmCard,
                StateType.PlaceCard   => PlaceCard,
                StateType.SelectPiece => SelectPiece,
                StateType.MovePiece   => MovePiece,
                StateType.Event       => Event,
                StateType.DrawCard    => DrawCard,
                StateType.EndTurn     => EndTurn,
                StateType.GameOver    => GameOver,
                StateType.None        => null,
                _                     => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            State?.Exit();
            CurrentStateType = type;
            State = newState;
            State?.Enter();
        }
    }
}
