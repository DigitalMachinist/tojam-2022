using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utilities;
using Managers;

namespace UI
{
	public class UiController : StateMonoBehaviour
	{
		[SerializeField] private bool debug;
		[SerializeField] private bool debug_triggerGameOver;
		
		[SerializeField] private UnityEvent pushPlay;
		[SerializeField] private UnityEvent playAgain;
		
		[Header("Title")]
		[SerializeField] private Button playButton;
		[SerializeField] private Button titleExitButton;
		[SerializeField] private CanvasGroup titleScreen;
		[SerializeField] private CanvasGroup buttonGroup;
		[SerializeField] private float logoDelay; 
		[SerializeField] private float buttonDelay; 
		[SerializeField] private Image logo;

		[Header( "Timmy Logo" )]
		[SerializeField] private CanvasGroup timmyScreen;
		[SerializeField] private float timmyLogoDelay;
		[SerializeField] private Image logoTimmy;
		[SerializeField] private Image logoSensible;

		[Header( "Timmy Logo" )]
		[SerializeField] private CanvasGroup apocScreen;
		[SerializeField] private float apocLogoDelay;
		[SerializeField] private Image logoArmageddon;

		[Header("GameOver")]
		[SerializeField] private Button playAgainButton;
		[SerializeField] private Button endExitButton;
		[SerializeField] private CanvasGroup gameOverScreen;
		[SerializeField] private TextMeshProUGUI winnerText;

		public bool AllowGameInput { get; private set; } = false;
		
		private void Awake()
		{
			// hook up the buttons.
			playButton.onClick.AddListener(() =>
			{
				pushPlay.Invoke();
				SetState(FadeOutTitleScreen);
			});
			
			playAgainButton.onClick.AddListener(() =>
			{
				playAgain.Invoke();
				SetState(FadeOutGameOverScreen);
			});
			
			endExitButton.onClick.AddListener(() =>
			{
				if (!Application.isEditor) Application.Quit();
			});
			
			titleExitButton.onClick.AddListener(() =>
			{
				if (!Application.isEditor) Application.Quit();
			});
		}

		private void Update()
		{
			if (!debug) return;
			if (debug_triggerGameOver)
			{
				ShowGameOverScreen("GAME OVER, YO.");
				debug_triggerGameOver = false; 
			}
		}

		private void Start()
		{
			gameOverScreen.gameObject.SetActive(false);
			titleScreen.gameObject.SetActive(false);
			SetState( TitleScreen );

			GameManager.Get().PlayerBlack.TurnAdvanced += ChangeLogo;
			GameManager.Get().PlayerWhite.TurnAdvanced += ChangeLogo;			
		}

		private void OnDestroy()
		{
			GameManager.Get().PlayerBlack.TurnAdvanced -= ChangeLogo;
			GameManager.Get().PlayerWhite.TurnAdvanced -= ChangeLogo;
		}

		void ChangeLogo()
        {
			if ( GameManager.Get().CurrentPhase == 2 )
			{
				Vector3 timmyTransform = logoTimmy.transform.localPosition;
				//Do Timmy Tween
				timmyScreen.alpha = 1;
				LeanTween.value( 980f, -120f, 1f )
					.setEaseInOutCubic()
					.setOnUpdate( ( value ) => {
					timmyTransform.y = value;
					logoTimmy.transform.localPosition = timmyTransform;
				} )
					.setOnComplete( ( value ) => {
						StartCoroutine( OnComplete( timmyScreen ) );
					} );
			}
			else if ( GameManager.Get().CurrentPhase == 3 )
			{

				Vector3 apocTransform = logoArmageddon.transform.localPosition;
				//Do Armageddon Tween
				apocScreen.alpha = 1;
				LeanTween.value( 760f, 0f, 1f )
					.setEaseInOutCubic()
					.setOnUpdate( ( value ) => {
					apocTransform.y = value;
					logoArmageddon.transform.localPosition = apocTransform;
				} )
					.setOnComplete( ( value ) => {
						StartCoroutine( OnComplete( apocScreen ) );
				} );
			}
		}

		IEnumerator OnComplete( CanvasGroup cg)
        {
			yield return new WaitForSeconds( 3f );
			cg.alpha = 0f;
			cg.gameObject.SetActive( false );
		}

		IEnumerator TitleScreen()
		{
			AllowGameInput = false; 
			titleScreen.gameObject.SetActive(true);
			titleScreen.alpha = 1f;
			buttonGroup.alpha = 0f;
			logo.color = Color.white.SetAlpha(0f);
			yield return new WaitForSeconds(logoDelay);
			LeanTween.value(logo.gameObject, f => logo.color = f, Color.white.SetAlpha(0f), Color.white.SetAlpha(1f),
				1f);
			yield return new WaitForSeconds(buttonDelay);
			LeanTween.value(buttonGroup.gameObject, f => buttonGroup.alpha = f, 0f, 1f, 0.25f);
		}
		
		IEnumerator FadeOutTitleScreen()
		{
			titleScreen.interactable = false;
			titleScreen.blocksRaycasts = false; 
			LeanTween.value(titleScreen.gameObject, f => titleScreen.alpha = f, 1f, 0f, 2f);
			yield return new WaitForSeconds(2f);
			titleScreen.gameObject.SetActive(false);
			SetState(Playing);
		}

		IEnumerator Playing()
		{
			AllowGameInput = true;
			yield return null;
		}

		IEnumerator GameOver()
		{
			AllowGameInput = false; 
			gameOverScreen.gameObject.SetActive(true);
			gameOverScreen.alpha = 0f; 
			LeanTween.value(gameOverScreen.gameObject, f => gameOverScreen.alpha = f, 0f, 1f, 1f);
			gameOverScreen.interactable = true;
			gameOverScreen.blocksRaycasts = true;
			yield return null;
		}
		
		IEnumerator FadeOutGameOverScreen()
		{
			gameOverScreen.interactable = false;
			gameOverScreen.blocksRaycasts = false; 
			LeanTween.value(gameOverScreen.gameObject, f => gameOverScreen.alpha = f, 1f, 0f, 2f);
			yield return new WaitForSeconds(2f);
			gameOverScreen.gameObject.SetActive(false);
			winnerText.text = "";
			SetState(Playing);
		}

		public void ShowGameOverScreen(string message)
		{
			winnerText.text = message;
			SetState(GameOver);
		}

		public enum StateTypes
		{
			Title,
			Playing,
			GameOver
		}
	} 
}

