using UnityEngine;

namespace Board
{
    public class BoardTest : MonoBehaviour
    {
        public Board Board;

        void Start()
        {
            if (Board == null)
            {
                Board = GetComponentInChildren<Board>();
            }

            Board.Build();
        }

        void Update()
        {
            Tile tile = Board.MouseSelectTile();
            if (tile != null)
            {
                Debug.Log(tile.name);
            }
            else
            {
                Debug.Log("No tile under the cursor.");
            }
        }
    }
}
