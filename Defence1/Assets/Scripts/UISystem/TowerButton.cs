using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class TowerButton : MonoBehaviour{//, IPointerEnterHandler, IPointerExitHandler {

	public static bool mouseOnButton { get{return numOfButtonsWithMouseIn != 0;}}
	static int numOfButtonsWithMouseIn = 0;

	public void OnPointerEnter (PointerEventData eventData) {
		numOfButtonsWithMouseIn += 1;
	}

	public void OnPointerExit (PointerEventData eventData) {
		numOfButtonsWithMouseIn -= 1;
	}
}
