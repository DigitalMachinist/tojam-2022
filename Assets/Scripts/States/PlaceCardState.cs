using Board;
using Managers;
using Pieces;
using UnityEngine;
using UnityEngine.Timeline;

namespace States
{
    public class PlaceCardState : State
    {
        private Piece tempPiece;
        
        private void OnCancelClicked()
        {
            var manager = GameManager.Get();
            manager.ClearSelectedCard();
            manager.StateMachine.ChangeState(StateType.SelectCard);
        }
        
        public override void Enter()
        {
            base.Enter();
            
            var manager = GameManager.Get();
            manager.InstructionText.text = "Choose a space";
            manager.PlaceCardDisplayCard.PopulateCardFields(manager.SelectedCard._cardSO, manager.CurrentPlayer.Colour);
            manager.PlaceCardDisplayCard.DisplayCard();
            manager.PlaceCardDisplayCard.Show();
            manager.PlaceCardDisplay.SetActive(true);
            manager.ConfirmCardButton.gameObject.SetActive(false);
            manager.CancelCardButton.gameObject.SetActive(false);
            manager.CancelPlaceButton.gameObject.SetActive(true);
            manager.CancelPlaceButton.onClick.AddListener(OnCancelClicked);
            
            var go = GameObject.Instantiate(manager.SelectedCard._cardSO.prefab, new Vector3(1000, 0, 0), Quaternion.identity);
            tempPiece = go.GetComponent<Piece>();
            
            // Clear all hover and selection states AND signal where the piece can be placed.
            foreach (var tile in manager.Board.Tiles)
            {
                tile.IsHovering = false;
                tile.SelectionState = tempPiece.ValidatePlace(manager.CurrentPlayer, tile)
                    ? Tile.SelectionStateTypes.Available
                    : Tile.SelectionStateTypes.None;
            }
        }
        
        public override void Update()
        {
            base.Update();

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                OnCancelClicked();
            }
            
            var manager = GameManager.Get();
            
            // Clear all hover states.
            foreach (var tile in manager.Board.Tiles)
            {
                tile.IsHovering = false;
            }

            Tile hoveredTile = manager.Board.MouseSelectTile();
            if (hoveredTile == null)
            {
                return;
            }

            hoveredTile.IsHovering = tempPiece.ValidatePlace(manager.CurrentPlayer, hoveredTile);
            
            // TODO: Enable tile hover state
            
            if (!Input.GetMouseButtonUp(0))
            {
                return;
            }
            
            Debug.Log($"Play {manager.SelectedCard.Title} at {hoveredTile.name}");
            manager.SelectedCard.Play();
            manager.CurrentPlayer.PlaceCard(manager.SelectedCard._cardSO, hoveredTile);
            manager.ClearSelectedCard();
            manager.StateMachine.ChangeState(StateType.SelectPiece);
        }
        
        public override void Exit()
        {
            base.Exit();
            
            var manager = GameManager.Get();
            manager.ClearSelectedPiece();
            manager.PlaceCardDisplay.SetActive(false);
            manager.ConfirmCardButton.gameObject.SetActive(false);
            manager.CancelCardButton.gameObject.SetActive(false);
            manager.CancelPlaceButton.gameObject.SetActive(false);
            manager.CancelPlaceButton.onClick.RemoveListener(OnCancelClicked);
            
            // Clear all selection states.
            foreach (var tile in manager.Board.Tiles)
            {
                tile.IsHovering = false;
                tile.SelectionState = Tile.SelectionStateTypes.None;
            }
            
            GameObject.Destroy(tempPiece.gameObject);
        }
    }
}
