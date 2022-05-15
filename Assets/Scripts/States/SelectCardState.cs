using System.Collections.Generic;
using Managers;

namespace States
{
    public class SelectCardState : State
    {
        private List<Card> cards;
        
        private void OnCardClicked(Card card)
        {
            var manager = GameManager.Get();
            manager.SelectedCard = card;
            manager.StateMachine.ChangeState(StateType.ConfirmCard);
        }
        
        public override void Enter()
        {
            base.Enter();
            
            var manager = GameManager.Get();
            manager.InstructionText.text = "Play a card";
            cards = new List<Card>(manager.CurrentPlayerHand.playerHand);
            foreach (var card in cards)
            {
                card.Clicked += OnCardClicked;
            }
        }
        
        public override void Update()
        {
            base.Update();
        }
        
        public override void Exit()
        {
            base.Exit();
            
            foreach (var card in cards)
            {
                card.Clicked -= OnCardClicked;
            }
        }
    }
}
