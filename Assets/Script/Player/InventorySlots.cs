using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class InventorySlots : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler 
{	
	private Canvas inventoryCanvas;
	private RectTransform rectTransform;
	private CanvasGroup canvasGroup;

	private Vector2 orginalPosition;

	public TMP_Text tooltip;
	[SerializeField] GameObject tooltipBox;


	void Start() {
		if (transform.parent.parent.parent.name == "InventoryMenu") {
			inventoryCanvas = transform.parent.parent.parent.GetComponent<Canvas>();
		}
		else {
			Debug.Log("get canvas fail");
		}

		canvasGroup = GetComponent<CanvasGroup>();
		rectTransform = GetComponent<RectTransform>();
		tooltipBox.SetActive(false);
		
	}

	public void OnBeginDrag(PointerEventData eventData) {
    	// Debug.Log("begin drag");
		canvasGroup.blocksRaycasts = false;
		canvasGroup.alpha = .6f;
		orginalPosition = rectTransform.anchoredPosition;
	}

	public void OnDrag(PointerEventData eventData) {
    	// Debug.Log("on drag");
    	rectTransform.anchoredPosition += eventData.delta / inventoryCanvas.scaleFactor;

	}

	public void OnEndDrag(PointerEventData eventData) {
    	// Debug.Log("end drag");	
		canvasGroup.alpha = 1f;

		if (transform.parent.name != "LureSlot") {
			rectTransform.anchoredPosition = orginalPosition;
			canvasGroup.blocksRaycasts = true;
		}
	}

	public void addDescription (string description) {
		tooltip.text = description;
	}

	public void OnPointerEnter(PointerEventData eventData) {
		tooltipBox.SetActive(true);
	}

	public void OnPointerExit(PointerEventData eventData) {
		tooltipBox.SetActive(false);
	}
}
