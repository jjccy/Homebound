using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : OnQueDialogue
{

    void OnTriggerStay(Collider collider) {
    	if (collider.transform.root.tag == "Player") {
    		TriggerDialogue();
    		Destroy(gameObject);
    	}
    	
    }
}
