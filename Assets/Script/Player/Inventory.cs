using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{	
	[SerializeField] GameObject InventoryMenu, parent, ItemsParent, InventorySlot, requireList;
	public float maxItems;

	[SerializeField] Sprite RolyPoly, BlastRabbit, FlyBoy, TazerFace, 
									RolyPolyLure, BlastRabbitLure, FlyBoyLure, TazerFaceLure,
									Oxygen, Default;

	bool inventoryOpen = false;

	public List<GameObject> itemList  = new List<GameObject>();

	float destroyChildTimer = 1f;

	bool firstBlastRabbit = true, firstFlyboy = true, firstTazerFace = true, requirementIsOpen = false;

	[SerializeField] OnQueDialogue firstBlastRabbitDialogue, firstFlyboyDialogue, firstTazerFaceDialogue;

	void Start() {
		CloseInventory();
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) {
        	if (!inventoryOpen) OpenInventory();
        	else CloseInventory();
        }

        if (inventoryOpen) {
        	Cursor.lockState = CursorLockMode.Confined;
        	Cursor.visible = true;
        }
    }

    public void CloseInventory() {
		// InventoryMenu.SetActive(false);

		parent.GetComponent<RectTransform>().offsetMin = new Vector2(parent.GetComponent<RectTransform>().offsetMin.x, 9999);

		Cursor.lockState = CursorLockMode.Locked;
    	Cursor.visible = false;

    	inventoryOpen = false;

    	requireList.SetActive(requirementIsOpen);
	}

	public void OpenInventory() {
		// InventoryMenu.SetActive(true);

 		parent.GetComponent<RectTransform>().offsetMin = new Vector2(parent.GetComponent<RectTransform>().offsetMin.x, 0);

      	Cursor.lockState = CursorLockMode.Confined;
    	Cursor.visible = true;

    	inventoryOpen = true;

    	requirementIsOpen = requireList.active;
    	requireList.SetActive(false);
 	}

	public void addItem(string tag, string description) {

		if (itemList.Count < maxItems) {
			GameObject newItem = Instantiate(InventorySlot);
			newItem.GetComponent<InventorySlots>().addDescription(description);

			Image icon = newItem.transform.Find("ItemButton/Icon").GetComponent<Image>();

			switch (tag) {
				case "RabbitBlaster":
					icon.sprite = BlastRabbit;
					newItem.tag = "RabbitBlaster";

					if (firstBlastRabbit) {
						firstBlastRabbitDialogue.TriggerDialogue();
						firstBlastRabbit = false;
					}

					break;

				case "RolyPoly":
					icon.sprite = RolyPoly;
					newItem.tag = "RolyPoly";
					break;

				case "Flyboy":
					icon.sprite = FlyBoy;
					newItem.tag = "Flyboy";

					if (firstFlyboy) {
						firstFlyboyDialogue.TriggerDialogue();
						firstFlyboy = false;
					}

					break;

				case "TazerFace":
					icon.sprite = TazerFace;
					newItem.tag = "TazerFace";

					if (firstTazerFace) {
						firstTazerFaceDialogue.TriggerDialogue();
						firstTazerFace = false;
					}

					break;

				case "RabbitBlasterLure":
					icon.sprite = BlastRabbitLure;
					newItem.tag = "RabbitBlasterLure";
					break;

				case "RolyPolyLure":
					icon.sprite = RolyPolyLure;
					newItem.tag = "RolyPolyLure";
					break;

				case "FlyboyLure":
					icon.sprite = FlyBoyLure;
					newItem.tag = "FlyboyLure";
					break;

				case "TazerFaceLure":
					icon.sprite = TazerFaceLure;
					newItem.tag = "TazerFaceLure";
					break;

				default:
					icon.sprite = Default;
					break;
			}

			newItem.transform.SetParent(ItemsParent.transform);
			itemList.Add(newItem);
		}
		
	}

	public bool isOpen() {
		return inventoryOpen;
	}

	public void clearInventory() {
		foreach (GameObject item in itemList) {
			Destroy(item);
		}

		itemList.Clear();
	}

	public List<GameObject> getItemList() {
		return itemList;
	}

	public void RemoveFromInventory(string tag) {
		foreach (GameObject item in itemList) 
	    {  
	    	if (item.tag == tag) {
	    		Destroy(item);
	    		itemList.Remove(item);
	    		break;
	    	}

	    }
	}
}
