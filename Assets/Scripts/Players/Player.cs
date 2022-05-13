using System.Collections.Generic;
using Pieces;

namespace Players
{
    public class Player
    {
        public PlayerColour Colour;
        public List<Piece> TakenPieces;

        public void TakePiece(Piece piece)
        {
            piece.Take();
            TakenPieces.Add(piece);
            
            // TODO: Move the piece to some kind of graveyard area?
        }
    }
}
