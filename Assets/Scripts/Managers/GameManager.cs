using System;
using System.Collections;
using System.Collections.Generic;
using Board;
using Pieces;
using Players;
using States;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
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
        public Transform PlayerHandBlackCanvasTarget;
        public Transform PlayerHandWhiteCanvasTarget;
        public GameObject PlayerHandBlackCanvas;
        public GameObject PlayerHandWhiteCanvas;
        public Transform DeckTarget;
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
        public Background Background;
        public UiController UiController;
        public TextMeshProUGUI PlayerTurnText;
        public TextMeshProUGUI TurnNumberText;
        public TextMeshProUGUI InstructionText;
        public AudioSource Audio_PieceSelect;
        public AudioSource Audio_PieceDestroy;
        public AudioSource Audio_DinoRoar;
        public AudioSource Audio_BoardCrumble;
        public AudioSource Audio_TimmyYess;
        public AudioSource Audio_TimmyLaugh;
        public AudioSource AudioPieceDestroyPrefab;
        public int TurnPhase2 = 3;
        public int TurnPhase3 = 10;
        public int ApocalypseTurnDelay = 3;
        public float TileBreakStep = 0.1f;
        public float TileBreakForceMin = 1f;
        public float TileBreakForceMax = 5f;
        public Color Pink = Color.magenta;
        public Color Green = Color.green;
        
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

        void Init()
        {
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
        }

        void PreStart()
        {
            foreach (var tile in Board.Tiles)
            {
                tile.IsSensible = true;
            }
            
            PlayerBlack.Reset();
            PlayerWhite.Reset();
            
            ApocalypseTiles.Clear();
            ApocalypseProgress = 0;
            TurnNumber = 0;
            
            BoardFactory.ConfigureInitialBoard(Board, PlayerBlack, PlayerWhite);
            PlayerHandBlack.InitHand();
            PlayerHandBlack.RefreshHandCards();
            PlayerHandWhite.InitHand();
            PlayerHandWhite.RefreshHandCards();
            BeginPhase(1);
            
            // To start on White's turn, we begin from Black and advance to the opposite player.
            PlayerTurn = PlayerColour.Black;
            BeginOtherPlayerTurn();
        }

        public void StartGame()
        {
            PreStart();
            StartCoroutine(CoStartGame());
        }

        private IEnumerator CoStartGame()
        {
            Audio_PieceSelect.Play();
            yield return new WaitForSeconds(0.2f);
            Audio_PieceSelect.mute = true;
            StateMachine.ChangeState(StateType.BeginTurn);
            Audio_PieceSelect.mute = false;
        }

        public void ResetGame()
        {
            Init();
            StartGame();
        }

        void Start()
        {
            StateMachine = new StateMachine(this);
            Board = GetComponentInChildren<Chessboard>();
            RestartGameButton.onClick.AddListener(ResetGame);
            Init();
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
            Audio_TimmyYess.Play();
            IsPlayingApocalypseEvent = true;
            foreach (var tile in ApocalypseTiles)
            {
                // Destroy tiles and kill pieces but don't place them into any player's taken pieces list.
                tile.Destroy();
                yield return new WaitForSeconds(TileBreakStep);
            }

            yield return new WaitForSeconds(2);
            
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
                tile.CrumblingState = (Tile.CrumblingStateTypes) Mathf.Max(0, 3 - ApocalypseTurnsLeft);
                
                yield return new WaitForSeconds(TileBreakStep);
            }

            CheckGameOverCondition();
            
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

        public bool CheckGameOverCondition()
        {
            var hasBlackLost = PlayerBlack.HasLost;
            var hasWhiteLost = PlayerWhite.HasLost;
            if (hasBlackLost && hasWhiteLost)
            {
                Winner = null;
                StateMachine.ChangeState(StateType.GameOver);
                UiController.ShowGameOverScreen("The apocalypse swallows all!");
                return true;
            }
            else if (hasBlackLost)
            {
                Winner = PlayerWhite;
                StateMachine.ChangeState(StateType.GameOver);
                UiController.ShowGameOverScreen("Pink wins!");
                return true;
            }
            else if (hasWhiteLost)
            {
                Winner = PlayerBlack;
                StateMachine.ChangeState(StateType.GameOver);
                UiController.ShowGameOverScreen("Green wins!");
                return true;
            }

            return false;
        }

        public void BeginOtherPlayerTurn()
        {
            PlayerTurn = GetOtherColour();
            CurrentPlayer.AdvanceTurn();
            TurnNumber = Mathf.Max(PlayerBlack.TurnNumber, PlayerWhite.TurnNumber);
        }

        public void UpdateTurnInfo()
        {
            TurnNumberText.text = TurnNumber.ToString();
            if (CurrentPhase == 1)
            {
                PlayerTurnText.text = PlayerTurn == PlayerColour.Black
                    ? "Black's Turn"
                    : "White's Turn";
            }
            else
            {
                if (PlayerTurn == PlayerColour.Black)
                {
                    PlayerTurnText.text = "Green's Turn";
                    PlayerTurnText.color = Green;
                }
                else
                {
                    PlayerTurnText.text = "Pink's Turn";
                    PlayerTurnText.color = Pink;
                }
            }
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
            if ( CurrentPhase == 2 )
            {
                Audio_TimmyYess.Play();
                IntroduceDeckbuilderTween();
                Background.gameObject.SetActive(true);
                foreach (var tile in Board.Tiles)
                {
                    tile.IsSensible = false;
                }
            }
            if (CurrentPhase == 3)
            {
                BeginApocalypseEventCountdown();
                ComputeTilesToDestroy();
                Deck.SetApocalypse();
                Audio_TimmyLaugh.Play();
            }
            UiController.SetPhase(CurrentPhase);
            Background.SetPhase(CurrentPhase);
            PhaseChanged?.Invoke( number );
        }

        private void IntroduceDeckbuilderTween()
        {
            PlayerHandBlackCanvas.gameObject.SetActive(true);
            PlayerHandWhiteCanvas.gameObject.SetActive(true);
            Deck.gameObject.SetActive(true);
            LeanTween
                .move(PlayerHandBlackCanvas.gameObject, PlayerHandBlackCanvasTarget, 0.5f)
                .setEaseInOutCubic();
            LeanTween
                .move(PlayerHandWhiteCanvas.gameObject, PlayerHandWhiteCanvasTarget, 0.5f)
                .setEaseInOutCubic();
            LeanTween
                .move(Deck.gameObject, DeckTarget, 0.5f)
                .setEaseInOutCubic();
        }

        public void BeginApocalypseEventCountdown()
        {
            ApocalypseTurnsLeft = ApocalypseTurnDelay;
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
