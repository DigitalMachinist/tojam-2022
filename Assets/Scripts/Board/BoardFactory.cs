using Pieces;
using Players;
using UnityEngine;

namespace Board
{
    public class BoardFactory : MonoBehaviour
    {
        public PieceFactory PieceFactory;

        void Start()
        {
            PieceFactory = FindObjectOfType<PieceFactory>();
        }
        
        public void ConfigureInitialBoard(Chessboard chessboard, Player black, Player white)
        {
            chessboard.Build();

            black.PlacePiece(PieceFactory.Rook(black), chessboard.GetTile(8, 1), true);
            black.PlacePiece(PieceFactory.Knight(black), chessboard.GetTile(8, 2), true);
            black.PlacePiece(PieceFactory.Bishop(black), chessboard.GetTile(8, 3), true);
            black.PlacePiece(PieceFactory.Queen(black), chessboard.GetTile(8, 4), true);
            black.PlacePiece(PieceFactory.King(black), chessboard.GetTile(8, 5), true);
            black.PlacePiece(PieceFactory.Bishop(black), chessboard.GetTile(8, 6), true);
            black.PlacePiece(PieceFactory.Knight(black), chessboard.GetTile(8, 7), true);
            black.PlacePiece(PieceFactory.Rook(black), chessboard.GetTile(8, 8), true);
            for (var col = 1; col <= chessboard.Cols; col++)
            {
                black.PlacePiece(PieceFactory.Pawn(black), chessboard.GetTile(7, col), true);
            }
            
            white.PlacePiece(PieceFactory.Rook(white), chessboard.GetTile(1, 1), true);
            white.PlacePiece(PieceFactory.Knight(white), chessboard.GetTile(1, 2), true);
            white.PlacePiece(PieceFactory.Bishop(white), chessboard.GetTile(1, 3), true);
            white.PlacePiece(PieceFactory.Queen(white), chessboard.GetTile(1, 4), true);
            white.PlacePiece(PieceFactory.King(white), chessboard.GetTile(1, 5), true);
            white.PlacePiece(PieceFactory.Bishop(white), chessboard.GetTile(1, 6), true);
            white.PlacePiece(PieceFactory.Knight(white), chessboard.GetTile(1, 7), true);
            white.PlacePiece(PieceFactory.Rook(white), chessboard.GetTile(1, 8), true);
            for (var col = 1; col <= chessboard.Cols; col++)
            {
                white.PlacePiece(PieceFactory.Pawn(white), chessboard.GetTile(2, col), true);
            }
        }
    }
}
