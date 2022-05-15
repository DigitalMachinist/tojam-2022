using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace  UI
{
	public class ButtonHoverHelper : MonoBehaviour, IPointerEnterHandler
	{
		public UnityEvent onHoverEvent;
		
		public void OnPointerEnter(PointerEventData eventData)
		{
			onHoverEvent.Invoke();
		}
	}
}
