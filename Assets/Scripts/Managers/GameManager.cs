using System.Collections;
using System.Collections.Generic;
using Board;
using Pieces;
using Players;
using States;
using UnityEngine;
using UnityEngine.UIElements;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public BoardFactory BoardFactory;
        public Player PlayerBlack;
        public Player PlayerWhite;
        public GameObject Deck;
        public Hand PlayerHandBlack;
        public Hand PlayerHandWhite;
        public GameObject ConfirmCardDialog;
        public Button ConfirmCardButton;
        public Button CancelCardButton;
        public GameObject PlaceCardDisplay;
        public int TurnPhase2 = 3;
        public float TileBreakStep = 0.1f;
        public float TileBreakForceMin = 0.1f;
        public float TileBreakForceMax = 0.5f;
        
        public PlayerColour PlayerTurn;
        public int TurnNumber;
        public int BattleRoyaleProgress = 0;
        public Piece SelectedPiece;
        public Card SelectedCard;
        public Player Winner;
        public Chessboard Board;
        public List<Tile> TilesToDestroy;

        public StateMachine StateMachine;

        public int PlayerTurnNumber => Mathf.Max(PlayerBlack.TurnNumber, PlayerWhite.TurnNumber);

        public Player CurrentPlayer => GetPlayer(PlayerTurn);
        
        public Hand CurrentPlayerHand => GetPlayerHand(PlayerTurn);
        
        public PlayerColour GetOtherColour()
        {
            return PlayerTurn == PlayerColour.Black ? PlayerColour.White : PlayerColour.Black;
        }
        
        public Player GetPlayer(PlayerColour colour)
        {
            return colour == PlayerColour.Black ? PlayerBlack : PlayerWhite;
        }
        
        public Player GetOtherPlayer(PlayerColour colour)
        {
            return colour == PlayerColour.Black ? PlayerWhite : PlayerBlack;
        }
        
        public Hand GetPlayerHand(PlayerColour colour)
        {
            return colour == PlayerColour.Black ? PlayerHandBlack : PlayerHandWhite;
        }
        
        void Start()
        {
            TilesToDestroy.Clear();
            BattleRoyaleProgress = 0;
            TurnNumber = 0;
            
            Board = GetComponentInChildren<Chessboard>();
            BoardFactory.ConfigureInitialBoard(Board, PlayerBlack, PlayerWhite);

            PlayerTurn = PlayerColour.White;
            PlayerWhite.AdvanceTurn();

            // TODO: Revisit this. This manager is likely to be the arbiter or turn advancement so it might not need to listen.
            PlayerBlack.TurnAdvanced += OnPlayerTurnAdvanced;
            PlayerWhite.TurnAdvanced += OnPlayerTurnAdvanced;
            
            StateMachine = new StateMachine();
            StateMachine.ChangeState(StateType.BeginTurn);
        }

        void Update()
        {
            StateMachine?.Update();
        }

        private void OnPlayerTurnAdvanced()
        {
            if (TurnNumber == PlayerTurnNumber)
            {
                return;
            }

            TurnNumber = PlayerTurnNumber;
            
            if (TurnNumber == 2)
            {
                // TODO: Big event to break out of pure chess
                ComputeTilesToDestroy();
            }
            else if (TurnNumber == 5)
            {
                AdvanceBattleRoyaleProgress();
            }
            else if (TurnNumber == 6)
            {
                ComputeTilesToDestroy();
            }
            else if (TurnNumber == 8)
            {
                AdvanceBattleRoyaleProgress();
            }
            else if (TurnNumber == 9)
            {
                ComputeTilesToDestroy();
            }
            // TODO: More turn handling for changing stuff that happens
        }

        private void AdvanceBattleRoyaleProgress()
        {
            BattleRoyaleProgress++;
            StartCoroutine(CoAdvanceBattleRoyaleProgress());
        }

        private IEnumerator CoAdvanceBattleRoyaleProgress()
        {
            foreach (var tile in TilesToDestroy)
            {
                // Destroy tiles and kill pieces but don't place them into any player's taken pieces list.
                tile.IsDestroyed = true;
                if (tile.Piece != null)
                {
                    tile.Piece.Take();
                }
                
                var force = Random.Range(TileBreakForceMin, TileBreakForceMax) * Vector3.up;
                var rigidbody = tile.GetComponent<Rigidbody>();
                rigidbody.AddForce(force, ForceMode.Impulse);
                rigidbody.useGravity = true;
                
                yield return new WaitForSeconds(TileBreakStep);
            }

            yield return new WaitForSeconds(10);
            
            foreach (var tile in TilesToDestroy)
            {
                tile.gameObject.SetActive(false);
            }
        }

        private void ComputeTilesToDestroy()
        {
            TilesToDestroy.Clear();
            var colLowerBound = BattleRoyaleProgress + 1;
            var colUpperBound = Board.Cols - BattleRoyaleProgress;
            var rowLowerBound = BattleRoyaleProgress + 1;
            var rowUpperBound = Board.Rows - BattleRoyaleProgress;
            for (var col = colLowerBound; col <= colUpperBound; col++)
            {
                TilesToDestroy.Add(Board.GetTile(BattleRoyaleProgress, col));
            }
            for (var row = rowLowerBound + 1; row <= rowUpperBound; row++)
            {
                TilesToDestroy.Add(Board.GetTile(row, (Board.Rows - BattleRoyaleProgress)));
            }
            for (var col = (colUpperBound - 1); col >= colLowerBound; col--)
            {
                TilesToDestroy.Add(Board.GetTile(BattleRoyaleProgress, col));
            }
            for (var row = (rowUpperBound - 1); row >= (rowLowerBound + 1); row--)
            {
                TilesToDestroy.Add(Board.GetTile(row, (Board.Rows - BattleRoyaleProgress)));
            }
        }

        public void BeginOtherPlayerTurn()
        {
            PlayerTurn = GetOtherColour();
            CurrentPlayer.AdvanceTurn();
        }

        // This is the crappiest possible singleton because this has to exist in the scene already for it to work. lol
        private static GameManager instance;
        public static GameManager Get()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }
}
