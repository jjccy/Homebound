using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LureHoldable : MonoBehaviour
{	
	[SerializeField] float power;
	[SerializeField] GameObject bullet;
	[SerializeField] GameObject rolypolylure, flyboylure, tazerfacelure, blastrabbitlure;
	[SerializeField] Transform barrelPivot;
	[SerializeField] LureSlot lureslot;

	public Dialogue dialogue;
	bool noLurePickup = true;

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
			if (bullet != null) {
				Fire();

				Debug.Log("fire lure");
			}
		}
    }

	 void Fire ()
    {
		Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
		bulletrb.velocity = barrelPivot.forward * power;
		bulletrb.gameObject.GetComponent<Lure>().Place();

		bullet = null;
		lureslot.UseLure();
    }

    public void SetLure(string tag) {
    	switch (tag) {
				case "RabbitBlasterLure":
				bullet = blastrabbitlure;
				break;

				case "RolyPolyLure":
				bullet = rolypolylure;
				break;

				case "FlyboyLure":
				bullet = flyboylure;
				break;

				case "TazerFaceLure":
				bullet = tazerfacelure;
				break;

				default:
				Debug.Log("wrong tag");
				break;
			}
    }

    public void TriggerDialogue() {
    	if (noLurePickup) {
    		FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    		noLurePickup = false;
    	}
    	
    }
    
}
