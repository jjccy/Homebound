using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable
{
	public GameObject display, weapon;
	public Dialogue dialogue;

	protected override void Interact () 
    {
    	player.GetComponent<ThirdPersonController>().AddWeapon(weapon);
    	Destroy(display);

    	gameObject.layer = LayerMask.NameToLayer("Default");

    	FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
