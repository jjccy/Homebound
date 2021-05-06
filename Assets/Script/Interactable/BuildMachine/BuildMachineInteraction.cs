using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMachineInteraction : Interactable
{	
	
	[SerializeField] BuildMachineControl buildMachine;

	public ThirdPersonController controller;

    public Dialogue dialogue;
    bool interacted;
    
    

    protected override void Interact () 
    {
    	controller = player.GetComponent<ThirdPersonController>();
    	controller.Stop();
    	buildMachine.StartBuilding();

        TriggerDialogue();
    }

    public void TriggerDialogue() {
        if (!interacted) {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            interacted = true;
        }
        
    }
}
