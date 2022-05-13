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
        public float RowOffset = 0.5f;
        public float RowSpacing = 1.0f;
        public float ColOffset = 0.5f;
        public float ColSpacing = 1.0f;

        public Tile[] Tiles;

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
                float rowPosition = RowOffset + row * RowSpacing;
                for (var col = 0; col < Cols; col++)
                {
                    float colPosition = ColOffset + col * ColSpacing;
                    Vector3 localPosition = new Vector3(colPosition, 0, rowPosition);
                    Vector3 worldPosition = transform.TransformPoint(localPosition);

                    // Choose whether to place a while or black tile based on position on the board.
                    Tile tilePrefab = (row % 2 == 0 && col % 2 == 0 || row % 2 == 1 && col % 2 == 1)
                        ? BlackTilePrefab
                        : WhiteTilePrefab;
                    
                    Tile tileInstance = Instantiate(tilePrefab, worldPosition, Quaternion.identity, transform);
                    tileInstance.name = $"Tile {Convert.ToChar(col + 65)}{row + 1}";
                    tileInstance.Board = this;
                    tileInstance.Row = row;
                    tileInstance.Col = col;

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

            if (nearestTileHit.transform == null)
            {
                return null;
            }

            return nearestTileHit.transform.GetComponent<Tile>();
        }
    }
}
