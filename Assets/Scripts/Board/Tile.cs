using System;
using Exceptions;
using Pieces;
using UnityEngine;

namespace Board
{
    public class Tile : MonoBehaviour
    {
        public Chessboard Board;
        public int Row;
        public int Col;
        public Piece Piece;
        public bool IsDestroyed;

        [SerializeField] private bool debug;
        [SerializeField] private bool debug_hovering;
        [SerializeField] private SelectionStateTypes debug_selection;
        [SerializeField] private TileStateTypes debug_tile;
        [SerializeField] private CrumblingStateTypes debug_crumbling;
        
        [SerializeField] private GameObject hoverState;
        [SerializeField] private GameObject availableState;
        [SerializeField] private GameObject selectedState;
        [SerializeField] private GameObject dangerState;
        [SerializeField] private GameObject crumblingParent;
        [SerializeField] private GameObject crumblingState1;
        [SerializeField] private GameObject crumblingState2;
        [SerializeField] private GameObject crumblingState3;
        
        private bool isHovering;
        /// <summary>
        /// Update this to turn hovering effect on/off
        /// </summary>
        public bool IsHovering
        {
            get => isHovering;
            set
            {
                if (isHovering == value) return;
                isHovering = value;
                hoverState.SetActive(isHovering);
            }
        }

        private SelectionStateTypes selectionState;
        /// <summary>
        /// Manipulate selection state here: None/Available/Selected/Danger
        /// </summary>
        public SelectionStateTypes SelectionState
        {
            get => selectionState;
            set
            {
                if (selectionState == value) return;
                selectionState = value;
                switch (selectionState)
                {
                    // judge me not.
                    case SelectionStateTypes.None:
                        availableState.SetActive(false);
                        selectedState.SetActive(false);
                        dangerState.SetActive(false);
                        break;
                    case SelectionStateTypes.Available:
                        availableState.SetActive(true);
                        selectedState.SetActive(false);
                        dangerState.SetActive(false);
                        break;
                    case SelectionStateTypes.Selected:
                        availableState.SetActive(false);
                        selectedState.SetActive(true);
                        dangerState.SetActive(false);
                        break;
                    case SelectionStateTypes.Danger:
                        availableState.SetActive(false);
                        selectedState.SetActive(false);
                        dangerState.SetActive(true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private TileStateTypes tileState;
        /// <summary>
        /// Manipulate tile state here! (Normal, Crumbling, or Destroy)
        /// </summary>
        public TileStateTypes TileState
        {
            get => tileState;
            set
            {
                if (tileState == value) return;
                tileState = value;
                switch (tileState)
                {
                    case TileStateTypes.Normal:
                        crumblingParent.SetActive(false);
                        break;
                    case TileStateTypes.Crumbling:
                        crumblingParent.SetActive(true);
                        break;
                    case TileStateTypes.Destroyed:
                        // nothing right now. maybe some visual spice later?
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        private CrumblingStateTypes crumblingState = CrumblingStateTypes.Level1;
        /// <summary>
        /// Call this to manipulate the crumbling state! Will only show if the tile state is crumbling.
        /// </summary>
        public CrumblingStateTypes CrumblingState
        {
            get => crumblingState;
            set
            {
                if (crumblingState == value) return;
                crumblingState = value;
                switch (crumblingState)
                {
                    case CrumblingStateTypes.Level1:
                        crumblingState1.SetActive(true);
                        crumblingState2.SetActive(false);
                        crumblingState3.SetActive(false);
                        break;
                    case CrumblingStateTypes.Level2:
                        crumblingState1.SetActive(false);
                        crumblingState2.SetActive(true);
                        crumblingState3.SetActive(false);
                        break;
                    case CrumblingStateTypes.Level3:
                        crumblingState1.SetActive(false);
                        crumblingState2.SetActive(false);
                        crumblingState3.SetActive(true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void Update()
        {
            if (!debug) return;

            IsHovering = debug_hovering;
            TileState = debug_tile;
            CrumblingState = debug_crumbling;
            SelectionState = debug_selection;
        }

        // public void RenderReset()
        // {
        //     Debug.Log("TODO: Return tile to normal render mode.");
        // }
        //
        // public void RenderMovable()
        // {
        //     Debug.Log("TODO: Render the tile highlighted as a valid move.");
        // }
        //
        // public void RenderPlaceable()
        // {
        //     Debug.Log("TODO: Render the tile as a valid placement.");
        // }
        //
        // public void RenderSelectable()
        // {
        //     Debug.Log("TODO: Render the tile as selectable.");
        // }
        //
        // public void RenderSelected()
        // {
        //     Debug.Log("TODO: Render the tile as currently selected.");
        // }

        public enum SelectionStateTypes
        {
            None,
            Available,
            Selected,
            Danger
        }
        
        public enum TileStateTypes
        {
            Normal,
            Crumbling,
            Destroyed
        }
        
        public enum CrumblingStateTypes
        {
            Level1,
            Level2,
            Level3
        }
    }
}
