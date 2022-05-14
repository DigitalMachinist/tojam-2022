using Players;
using UnityEngine;

namespace Pieces
{
    public class PieceFactory : MonoBehaviour
    {
        public Pawn PrefabPawn;
        public Knight PrefabKnight;
        public Rook PrefabRook;
        public Bishop PrefabBishop;
        public Queen PrefabQueen;
        public King PrefabKing;

        public Pawn Pawn(Player player)
        {
            return Instantiate(PrefabPawn, Vector3.zero, Quaternion.identity, player.transform);
        }

        public Knight Knight(Player player)
        {
            return Instantiate(PrefabKnight, Vector3.zero, Quaternion.identity, player.transform);
        }

        public Rook Rook(Player player)
        {
            return Instantiate(PrefabRook, Vector3.zero, Quaternion.identity, player.transform);
        }

        public Bishop Bishop(Player player)
        {
            return Instantiate(PrefabBishop, Vector3.zero, Quaternion.identity, player.transform);
        }

        public Queen Queen(Player player)
        {
            return Instantiate(PrefabQueen, Vector3.zero, Quaternion.identity, player.transform);
        }

        public King King(Player player)
        {
            return Instantiate(PrefabKing, Vector3.zero, Quaternion.identity, player.transform);
        }
    }
}
