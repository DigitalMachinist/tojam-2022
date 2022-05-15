using System.Collections;
using UnityEngine;

namespace Utilities
{
	public class StateMonoBehaviour : MonoBehaviour
	{
		public delegate IEnumerator StateMethod();
		public StateMethod state { get; private set; }
		public StateMethod lastState { get; private set; }
		public string stateName { get; private set; }
		public string lastStateName { get; private set; }

		private Coroutine currentState = null;
 
		public void SetState(StateMethod stateMethod)
		{
			if (stateMethod != state)
			{
				if (currentState != null) StopCoroutine(currentState);
 
				lastStateName = stateName;
				stateName = stateMethod.Method.Name;
				
				lastState = state;
				state = stateMethod;
 
				currentState = StartCoroutine(DoSetState());
			}
		}
		
 
		void OnDisable()
		{
			if (currentState != null) StopCoroutine(currentState);
			state = null;
		}
 
		IEnumerator DoSetState()
		{
			yield return null;
			currentState = StartCoroutine(state());
		}
 
		public void InvokeState(StateMethod stateMethod, float time)
		{
			currentState = StartCoroutine(DoInvokeState(stateMethod, time));
		}
 
		IEnumerator DoInvokeState(StateMethod stateMethod, float time)
		{
			yield return new WaitForSeconds(time);
			SetState(stateMethod);
		}
	}
}