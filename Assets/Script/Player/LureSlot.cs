using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LureSlot : MonoBehaviour, IDropHandler
{
	[SerializeField] Inventory Inventory;

	public GameObject currentLure;
	[TagSelector] public string[] lureTag;

    public void OnDrop(PointerEventData eventData) {

    	if (eventData.pointerDrag != null && System.Array.Exists(lureTag, element => element == eventData.pointerDrag.tag)) {


    		if (currentLure != null) {
    			Inventory.addItem(currentLure.tag, currentLure.GetComponent<InventorySlots>().tooltip.text);
    			Destroy(currentLure);
    		}

    		currentLure = eventData.pointerDrag;


    		// set parent and position
    		currentLure.transform.SetParent(gameObject.transform);

    		Vector2 center = new Vector2(0.5f, 0.5f);		
    		currentLure.GetComponent<RectTransform>().anchorMin = center;
		    currentLure.GetComponent<RectTransform>().anchorMax = center;
		    currentLure.GetComponent<RectTransform>().pivot = center;
    		currentLure.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;


    		// set lure
    		Inventory.gameObject.GetComponent<LureHoldable>().SetLure(currentLure.tag);
    	}
    	
    }

    public void UseLure() {
        Inventory.itemList.Remove(currentLure);
    	Destroy(currentLure);
    	currentLure = null;
    }
}
