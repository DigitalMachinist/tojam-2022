using System;
using System.Linq;
using Pieces;
using UnityEngine;

namespace Board
{
    public class Chessboard : MonoBehaviour
    {
        public Tile WhiteTilePrefab;
        public Tile BlackTilePrefab;
        public int Rows = 8;
        public int Cols = 8;
        public float RowOffset = 0.5f;
        public float RowSpacing = 1.0f;
        public float ColOffset = 0.5f;
        public float ColSpacing = 1.0f;
        public bool AutoBuild = false;
        public Tile[] Tiles;

        void Start()
        {
            if (AutoBuild)
            {
                Build();
            }
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
                    tileInstance.Row = row +1;
                    tileInstance.Col = col +1;

                    Tiles[Rows * row + col] = tileInstance;
                }
            }
        }

        public Tile MouseSelectTile()
        {
            Vector3 origin = Camera.main.transform.position;
            Vector3 direction = Camera.main.ScreenPointToRay(Input.mousePosition).direction;
            int layerMask = LayerMask.GetMask("Tiles");
            RaycastHit nearestTileHit = Physics
                .RaycastAll(origin, direction, layerMask)
                .Where(hit => hit.transform.GetComponent<Tile>() != null)
                .OrderBy(hit => hit.distance)
                .FirstOrDefault();

            if (nearestTileHit.transform == null)
            {
                return null;
            }

            return nearestTileHit.transform.GetComponent<Tile>();
        }

        public Tile GetTile(Tile tile, Direction direction = Direction.None, int distance = 0)
        {
            return GetTile(tile.Row, tile.Col, direction, distance);
        }
        public Tile GetTile(int row, int col, Direction direction = Direction.None, int distance = 0)
        {
            switch (direction)
            {
                case Direction.N:
                    row += distance;
                    break;
                
                case Direction.NE:
                    row += distance;
                    col += distance;
                    break;
                
                case Direction.E:
                    col += distance;
                    break;
                
                case Direction.SE:
                    row -= distance;
                    col += distance;
                    break;
                
                case Direction.S:
                    row -= distance;
                    break;
                
                case Direction.SW:
                    row -= distance;
                    col -= distance;
                    break;
                
                case Direction.W:
                    col -= distance;
                    break;
                
                case Direction.NW:
                    row += distance;
                    col -= distance;
                    break;
                
                case Direction.None:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            // Since this is actually a 1D array, we need to check this based on rows/columns because its possible
            // that bad row/col coordinates could still give us a valid 1D array index.
            if (row < 1 || row > Rows || col < 1 || col > Cols)
            {
                throw new IndexOutOfRangeException("Off the edge of the board.");
            }
            
            return Tiles[Rows * (row - 1) + (col - 1)];
        }

        public Direction GetDirection(Tile start, Tile end)
        {
            if (end.Row == start.Row)
            {
                if (end.Col < start.Col)
                {
                    return Direction.W;
                }
                else if (end.Col > start.Col)
                {
                    return Direction.E;
                }
                else
                {
                    return Direction.None;
                }
            }
            
            if (end.Col == start.Col)
            {
                if (end.Row < start.Row)
                {
                    return Direction.S;
                }
                else if (end.Row > start.Row)
                {
                    return Direction.N;
                }
                else
                {
                    return Direction.None;
                }
            }

            var rowDiff = end.Row - start.Row;
            var colDiff = end.Col - start.Col;
            if (Mathf.Abs(rowDiff) == Mathf.Abs(colDiff))
            {
                if (rowDiff > 0)
                {
                    if (colDiff > 0)
                    {
                        return Direction.NE;
                    }
                    else
                    {
                        return Direction.NW;
                    }
                }
                else
                {
                    if (colDiff > 0)
                    {
                        return Direction.SE;
                    }
                    else
                    {
                        return Direction.SW;
                    }
                }
            }

            return Direction.None;
        }

        public int GetDistance(Tile start, Tile end)
        {
            if (end.Row == start.Row)
            {
                return Mathf.Abs(end.Col - start.Col);
            }
            
            if (end.Col == start.Col)
            {
                return Mathf.Abs(end.Row - start.Row);
            }
            
            var rowDiff = end.Row - start.Row;
            var colDiff = end.Col - start.Col;
            if (Mathf.Abs(rowDiff) == Mathf.Abs(colDiff))
            {
                return Mathf.Abs(rowDiff);
            }

            return 0;
        }
    }
}
