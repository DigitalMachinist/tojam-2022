using System;
using System.Linq;
using UnityEngine;

namespace Board
{
    public class Board : MonoBehaviour
    {
        public Tile WhiteTilePrefab;
        public Tile BlackTilePrefab;
        public int Rows = 8;
        public int Cols = 8;
        public float rowOffset = 0.5f;
        public float rowSpacing = 1.0f;
        public float colOffset = 0.5f;
        public float colSpacing = 1.0f;

        public Tile[] Tiles;

        void Start()
        {
            Build();
        }

        public void Destroy()
        {
            foreach (var tile in Tiles)
            {
                Destroy(tile.gameObject);
            }

            Tiles = null;
        }

        public void Build()
        {
            Tiles = new Tile[Rows * Cols];

            for (var row = 0; row < Rows; row++)
            {
                float rowPosition = rowOffset + row * rowSpacing;
                for (var col = 0; col < Cols; col++)
                {
                    float colPosition = colOffset + col * colSpacing;
                    Vector3 localPosition = new Vector3(colPosition, 0, rowPosition);
                    Vector3 worldPosition = transform.TransformPoint(localPosition);

                    // Choose whether to place a while or black tile based on position on the board.
                    Tile tilePrefab = (row % 2 == 0 && col % 2 == 0 || row % 2 == 1 && col % 2 == 1)
                        ? BlackTilePrefab
                        : WhiteTilePrefab;
                    
                    Tile tileInstance = Instantiate(tilePrefab, worldPosition, Quaternion.identity, transform);
                    tileInstance.name = $"Tile {Convert.ToChar(col + 65)}{row + 1}";

                    Tiles[Rows * row + col] = tileInstance;
                }
            }
        }

        public Tile GetTile(int row, int col)
        {
            return Tiles[Rows * row + col];
        }

        public Tile MouseSelectTile()
        {
            Vector3 origin = Camera.main.transform.position;
            Vector3 direction = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
            RaycastHit nearestTileHit = Physics
                .RaycastAll(origin, direction)
                .Where(hit => hit.transform.GetComponent<Tile>() != null)
                .OrderBy(hit => hit.distance)
                .FirstOrDefault();

            return nearestTileHit.transform.GetComponent<Tile>();
        }
    }
}
