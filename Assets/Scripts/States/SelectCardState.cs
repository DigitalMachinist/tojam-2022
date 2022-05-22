using System.Collections.Generic;
using Managers;

namespace States
{
    public class SelectCardState : State
    {
        private List<Card> cards;
        
        public SelectCardState(GameManager gameManager) : base(gameManager)
        {
        }
        
        private void OnCardClicked(Card card)
        {
            GameManager.SelectedCard = card;
            GameManager.StateMachine.ChangeState(StateType.ConfirmCard);
        }
        
        private void OnCardHovered(Card card)
        {
            // The hover target *MUST* be the 1st child in the hierarchy.
            var hoverTarget = card.transform.parent.GetChild(1);

            LeanTween
                .move(card.gameObject, hoverTarget, 0.2f)
                .setEaseInOutCubic();
        }
        
        private void OnCardUnhovered(Card card)
        {
            // The home target *MUST* be the 0th child in the hierarchy.
            var homeTarget = card.transform.parent.GetChild(0);

            LeanTween
                .move(card.gameObject, homeTarget, 0.2f)
                .setEaseInOutCubic();
        }
        
        public override void Enter()
        {
            base.Enter();
            
            GameManager.InstructionText.text = "Play a card";
            cards = new List<Card>(GameManager.CurrentPlayerHand.playerHand);
            foreach (var card in cards)
            {
                card.Clicked += OnCardClicked;
                card.Hovered += OnCardHovered;
                card.Unhovered += OnCardUnhovered;
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
                card.Hovered -= OnCardHovered;
                card.Unhovered -= OnCardUnhovered;
            }
        }
    }
}
