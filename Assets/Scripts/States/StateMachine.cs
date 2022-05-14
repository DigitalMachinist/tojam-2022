using System;

namespace States
{
    public class StateMachine
    {
        public StateType CurrentStateType;
        public State State;
        
        public BeginTurnState BeginTurn;
        public SelectCardState SelectCard;
        public ConfirmCardState ConfirmCard;
        public PlaceCardState PlaceCard;
        public SelectPieceState SelectPiece;
        public MovePieceState MovePiece;
        public EventState Event;
        public EndTurnState EndTurn;
        public GameOverState GameOver;

        public StateMachine()
        {
            BeginTurn = new BeginTurnState();
            SelectCard = new SelectCardState();
            ConfirmCard = new ConfirmCardState();
            PlaceCard = new PlaceCardState();
            SelectPiece = new SelectPieceState();
            MovePiece = new MovePieceState();
            Event = new EventState();
            EndTurn = new EndTurnState();
            GameOver = new GameOverState();

            CurrentStateType = StateType.None;
            State = null;
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
                StateType.PlayCards   => PlaceCard,
                StateType.ConfirmCard => ConfirmCard,
                StateType.PlaceCard   => PlaceCard,
                StateType.SelectPiece => SelectPiece,
                StateType.MovePiece   => MovePiece,
                StateType.Event       => Event,
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
