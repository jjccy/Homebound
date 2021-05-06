using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimal : OnQueDialogue
{	
	Animal animal;

    // Start is called before the first frame update
    void Start()
    {
        animal = GetComponent<Animal>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animal.isCaptured) {
        	TriggerDialogue();
        	Destroy(this);
        }
    }
}
