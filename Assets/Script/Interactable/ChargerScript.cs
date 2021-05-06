using UnityEngine;

public class ChargerScript : Interactable
{	
	bool refuel = false;

	public ParticleSystem chargerBean;

    protected override void Interact ()
    {	
    	// ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
	    // playerController.currentEnergy += 20;

	    // playerController.currentEnergy = Mathf.Min(playerController.currentEnergy, playerController.maxEnergy);

	    refuel = true;
    }

    protected override void constantUpdate() 
    {
    	if (refuel && isFocus) {
    		
    		float distance = Vector3.Distance(player.GetComponent<Transform>().position, transform.position);

    		if (Input.GetKey(KeyCode.F) && distance <= radius) {  			

    			// refuel player
    			ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();
			    playerController.currentEnergy += Time.deltaTime * (playerController.maxEnergy / 10);

			    playerController.currentEnergy = Mathf.Min(playerController.currentEnergy, playerController.maxEnergy);



			    // charger bean facing player

			    chargerBean.Play();

    			Vector3 direction = (playerController.transform.position - chargerBean.transform.position).normalized;
		        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
		        chargerBean.transform.rotation = lookRotation;
    		}
    		else {
    			refuel = false;
    		}
    	}
    	else {
    		chargerBean.Stop();
    	}
    }

}
