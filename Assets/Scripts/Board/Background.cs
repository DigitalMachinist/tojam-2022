using System;
using UnityEngine;
using Utilities;

namespace Board
{
	public class Background : MonoBehaviour
	{
		public bool debug;
		public TurnType debug_turn;
		[Range(0f, 1f)] public float debug_crossFader = 0f;

		[SerializeField] private Renderer normalWhite;
		[SerializeField] private Renderer normalBlack;
		[SerializeField] private Renderer armageddonWhite;
		[SerializeField] private Renderer armageddonBlack;
		
		private float crossFade = 0f;
		public float Crossfade
		{
			get => crossFade;
			set
			{
				if (Math.Abs(crossFade - value) < 0.01f) return;
				// only god can judge me. 
				crossFade = Mathf.Clamp01(value); 
				// 0 is normal, 1 is armageddon
				normalWhite.material.SetColor("_Color", Color.white.SetAlpha(1-value));
				normalBlack.material.SetColor("_Color", Color.white.SetAlpha(1-value));
				armageddonWhite.material.SetColor("_Color", Color.white.SetAlpha(value));
				armageddonBlack.material.SetColor("_Color", Color.white.SetAlpha(value));
			}
		}
		
		private TurnType currentTurn;
		public TurnType CurrentTurn
		{
			get => currentTurn;
			set
			{
				if (currentTurn == value) return;
				currentTurn = value; 
				switch (value)
				{
					// this can be tweenified later if we're feeling spicy. 
					case TurnType.White:
						normalBlack.gameObject.SetActive(false);
						armageddonBlack.gameObject.SetActive(false);
						normalWhite.gameObject.SetActive(true);
						armageddonWhite.gameObject.SetActive(true);
						break;
					case TurnType.Black:
						normalBlack.gameObject.SetActive(true);
						armageddonBlack.gameObject.SetActive(true);
						normalWhite.gameObject.SetActive(false);
						armageddonWhite.gameObject.SetActive(false);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(currentTurn), currentTurn, null);
				}
			}
		}

		private void Start()
		{
			// remove this if not needed.
			Crossfade = 0f;
			CurrentTurn = TurnType.White;
		}

		private void Update()
		{
			if (!debug) return;

			Crossfade = debug_crossFader;
			CurrentTurn = debug_turn;
		}

		public enum TurnType
		{
			White,
			Black
		}

		public enum WorldStateType
		{
			Normal,
			Armageddon
		}
	}
}

