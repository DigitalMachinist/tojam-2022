using Exceptions;
using Pieces;
using Players;
using UnityEngine;

namespace Board
{
    public class BoardTest : MonoBehaviour
    {
        public Board Board;
        public Piece PieceBlack1;
        public Piece PieceBlack2;
        public Piece PieceWhite1;
        public Piece PieceWhite2;
        public Player PlayerBlack;
        public Player PlayerWhite;
        public Piece SelectedPiece;

        void Start()
        {
            if (Board == null)
            {
                Board = GetComponentInChildren<Board>();
            }

            Board.Build();

            PlayerBlack.PlacePiece(PieceBlack1, Board.GetTile(2, 3));
            PlayerBlack.PlacePiece(PieceBlack2, Board.GetTile(3, 2));
            
            PlayerWhite.PlacePiece(PieceWhite1, Board.GetTile(6, 7));
            PlayerWhite.PlacePiece(PieceWhite2, Board.GetTile(7, 6));
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (SelectedPiece == null)
                {
                    Tile tile = Board.MouseSelectTile();
                    if (tile != null)
                    {
                        Debug.Log(tile.name);
                        try
                        {
                            SelectedPiece = tile.Piece;
                            Debug.Log(SelectedPiece);
                        }
                        catch (SelectionException e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
                else
                {
                    Tile tile = Board.MouseSelectTile();
                    if (tile != null)
                    {
                        Debug.Log(tile.name);
                        PlayerBlack.MovePiece(SelectedPiece, tile);
                        PlayerBlack.NextTurn();
                        SelectedPiece = null;
                    }
                }
            }
        }
    }
}
