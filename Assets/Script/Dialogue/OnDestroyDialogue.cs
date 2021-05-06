using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyDialogue : OnQueDialogue
{	
	public GameObject checkObject;

    // Update is called once per frame
    void Update()
    {
       if (checkObject == null) {
       		TriggerDialogue();
    		Destroy(gameObject);
       }
    }
}
