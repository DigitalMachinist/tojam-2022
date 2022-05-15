using System;
using System.Collections;
using System.Collections.Generic;
using Board;
using Pieces;
using Players;
using States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public event Action<int> PhaseChanged;

        public BoardFactory BoardFactory;
        public Player PlayerBlack;
        public Player PlayerWhite;
        public Hand PlayerHandBlack;
        public Hand PlayerHandWhite;
        public GameObject PlayerHandBlackCanvas;
        public GameObject PlayerHandWhiteCanvas;
        public Deck Deck;
        public GameObject PlaceCardDisplay;
        public Card PlaceCardDisplayCard;
        public GameObject ConfirmCardDialog;
        public Card ConfirmCardDialogCard;
        public Button ConfirmCardButton;
        public Button CancelCardButton;
        public Button CancelPlaceButton;
        public Button CancelMoveButton;
        public Button DrawCardButton;
        public Button RestartGameButton;
        public TextMeshProUGUI PlayerTurnText;
        public TextMeshProUGUI TurnNumberText;
        public TextMeshProUGUI InstructionText;
        public int TurnPhase2 = 3;
        public int TurnPhase3 = 10;
        public int ApocalypseTurnStep = 3;
        public float TileBreakStep = 0.1f;
        public float TileBreakForceMin = 1f;
        public float TileBreakForceMax = 5f;
        
        public PlayerColour PlayerTurn;
        public int TurnNumber;
        public bool IsPlayingApocalypseEvent;
        public int ApocalypseTurnsLeft = 0;
        public int ApocalypseProgress = 0;
        public Piece SelectedPiece;
        public Card SelectedCard;
        public int CurrentPhase = 1;
        public Player Winner;
        public Chessboard Board;
        public List<Tile> ApocalypseTiles;

        public StateMachine StateMachine;


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
            StateMachine = new StateMachine();
            Board = GetComponentInChildren<Chessboard>();
            RestartGameButton.onClick.AddListener(Init);
            Init();
        }

        void Init()
        {
            PlayerBlack.Reset();
            PlayerWhite.Reset();
            
            ApocalypseTiles.Clear();
            ApocalypseProgress = 0;
            TurnNumber = 0;
            
            BoardFactory.ConfigureInitialBoard(Board, PlayerBlack, PlayerWhite);

            PlayerHandBlackCanvas.gameObject.SetActive(false);
            PlayerHandWhiteCanvas.gameObject.SetActive(false);
            Deck.gameObject.SetActive(false);
            PlaceCardDisplay.gameObject.SetActive(false);
            ConfirmCardDialog.gameObject.SetActive(false);
            ConfirmCardButton.gameObject.SetActive(false);
            CancelCardButton.gameObject.SetActive(false);
            CancelPlaceButton.gameObject.SetActive(false);
            CancelMoveButton.gameObject.SetActive(false);
            DrawCardButton.gameObject.SetActive(false);
            
            PlayerHandBlack.InitHand();
            PlayerHandBlack.RefreshHandCards();
            PlayerHandWhite.InitHand();
            PlayerHandWhite.RefreshHandCards();
            
            BeginPhase(1);
            
            // To start on White's turn, we begin from Black and advance to the opposite player.
            PlayerTurn = PlayerColour.Black;
            BeginOtherPlayerTurn();
            
            BeginApocalypseEventCountdown();
            ComputeTilesToDestroy();

            StateMachine.ChangeState(StateType.BeginTurn);

            // For testing battle royale
            // foreach (var tile in ApocalypseTiles)
            // {
            //     tile.TileState = Tile.TileStateTypes.Crumbling;
            //     tile.CrumblingState = Tile.CrumblingStateTypes.Level1;
            // }
            // DoApocalypseEvent();
        }

        void Update()
        {
            StateMachine?.Update();
        }

        public void DoApocalypseEvent()
        {
            
            StartCoroutine(CoApocalypseEvent());
            ApocalypseProgress++;
        }

        private IEnumerator CoApocalypseEvent()
        {
            IsPlayingApocalypseEvent = true;
            foreach (var tile in ApocalypseTiles)
            {
                // Destroy tiles and kill pieces but don't place them into any player's taken pieces list.
                tile.IsDestroyed = true;
                if (tile.Piece != null)
                {
                    tile.Piece.Take();
                }
                
                var force = UnityEngine.Random.Range(TileBreakForceMin, TileBreakForceMax) * Vector3.up;
                var rigidbody = tile.GetComponent<Rigidbody>();
                rigidbody.AddForce(force, ForceMode.Impulse);
                rigidbody.useGravity = true;
                
                yield return new WaitForSeconds(TileBreakStep);
            }

            yield return new WaitForSeconds(2);
            
            foreach (var tile in ApocalypseTiles)
            {
                var rigidbody = tile.GetComponent<Rigidbody>();
                rigidbody.useGravity = false;
                rigidbody.velocity = Vector3.zero;
                // tile.gameObject.SetActive(false);
            }
            
            BeginApocalypseEventCountdown();
            ComputeTilesToDestroy();
            
            // Clear the entire board's tile state.
            foreach (var tile in Board.Tiles)
            {
                tile.TileState = tile.IsDestroyed
                    ? Tile.TileStateTypes.Destroyed
                    : Tile.TileStateTypes.Normal;
            }
            
            // Mark tiles that are crumbling as such.
            foreach (var tile in ApocalypseTiles)
            {
                tile.TileState = Tile.TileStateTypes.Crumbling;
                tile.CrumblingState = (Tile.CrumblingStateTypes) ApocalypseTurnsLeft;
                
                yield return new WaitForSeconds(TileBreakStep);
            }
            
            IsPlayingApocalypseEvent = false;
        }

        public void ComputeTilesToDestroy()
        {
            ApocalypseTiles.Clear();
            var colLowerBound = ApocalypseProgress + 1;
            var colUpperBound = Board.Cols - ApocalypseProgress;
            var rowLowerBound = ApocalypseProgress + 1;
            var rowUpperBound = Board.Rows - ApocalypseProgress;
            for (var row = rowLowerBound; row <= rowUpperBound; row++)
            {
                ApocalypseTiles.Add(Board.GetTile(row, colUpperBound));
            }
            for (var col = colUpperBound - 1; col >= colLowerBound; col--)
            {
                ApocalypseTiles.Add(Board.GetTile(rowUpperBound, col));
            }
            for (var row = rowUpperBound - 1; row >= rowLowerBound; row--)
            {
                ApocalypseTiles.Add(Board.GetTile(row, colLowerBound));
            }
            for (var col = colLowerBound + 1; col < colUpperBound; col++)
            {
                ApocalypseTiles.Add(Board.GetTile(rowLowerBound, col));
            }
        }

        public void BeginOtherPlayerTurn()
        {
            PlayerTurn = GetOtherColour();
            CurrentPlayer.AdvanceTurn();
            TurnNumber = Mathf.Max(PlayerBlack.TurnNumber, PlayerWhite.TurnNumber);
            UpdateTurnInfo();
        }

        void UpdateTurnInfo()
        {
            TurnNumberText.text = TurnNumber.ToString();
            PlayerTurnText.text = PlayerTurn == PlayerColour.Black
                ? "Green's Turn"
                : "Pink's Turn";
        }

        public void ClearSelectedCard()
        {
            SelectedCard = null;
        }

        public void ClearSelectedPiece()
        {
            SelectedPiece = null;
        }

        public void BeginPhase(int number)
        {
            CurrentPhase = number;
            if (CurrentPhase == 3)
            {
                BeginApocalypseEventCountdown();
            }
            PhaseChanged?.Invoke( number );
        }

        public void BeginApocalypseEventCountdown()
        {
            ApocalypseTurnsLeft = ApocalypseTurnStep;
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
