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

        public void RenderReset()
        {
            Debug.Log("TODO: Return tile to normal render mode.");
        }
        
        public void RenderMovable()
        {
            Debug.Log("TODO: Render the tile highlighted as a valid move.");
        }
        
        public void RenderPlaceable()
        {
            Debug.Log("TODO: Render the tile as a valid placement.");
        }
        
        public void RenderSelectable()
        {
            Debug.Log("TODO: Render the tile as selectable.");
        }
        
        public void RenderSelected()
        {
            Debug.Log("TODO: Render the tile as currently selected.");
        }
    }
}
