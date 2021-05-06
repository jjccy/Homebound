using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolyPoly : Animal
{   
    RandomMovement movementController;

    protected override void startOverride() {
        movementController = gameObject.GetComponent<RandomMovement>();
    }
    
    protected override void stop() {
        movementController.stop = true;
        movementController.toLocation = false;
    }

    protected override void moveAgain() {
        movementController.stop = false;
        movementController.toLocation = false;
    }

    protected override void toLocation(Transform targetLocation) {
    	movementController.moveToTarget = targetLocation;
        movementController.toLocation = true;
    }

    protected override void luredAction() {
        // override action if get lured


        float distance = Vector3.Distance(lure.transform.position, transform.position);

        if (distance > lureDistance) {
            toLocation(lure.transform);
        }
        else { // reaches lured location
            animator.SetBool(isLuredParam, isLured);
			stop();
            statusSprite.GetComponent<SpriteRenderer>().sprite = hungrySprite;
        }
    }

    protected override void seePlayerAction() {
    	
        movementController.speed = gameObject.GetComponent<RandomMovement>().maxSpeed;
    }
}
