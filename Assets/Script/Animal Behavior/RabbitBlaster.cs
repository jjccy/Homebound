using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitBlaster : Animal
{
	[SerializeField] float blasterPower, blasterRadius, blastTimer;
	[SerializeField] ParticleSystem blasterParticle;
	bool blasting;
	float blastingCountDown;

    RandomMovement movementController;

    [SerializeField] float jumpPower;

    [SerializeField] AudioSource boostSound;

    protected override void startOverride() {
        movementController = gameObject.GetComponent<RandomMovement>();
    }

	protected override void constanstUpdate() {

        if (blasting) {
        	blastingCountDown -= Time.deltaTime;

        	if (blastingCountDown <= 0) {
        		blasting = false;
        		moveAgain();

                boostSound.Stop();
        	}
        }
    }

    protected override void constanstFixedUpdate() {
        if (!isCaptured && movementController.isMoving() && onGround) {
            gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            onGround = false;
        }
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
    	if (!blasting) {
    		toLocation(playerlocation);

	        if (Vector3.Distance(playerlocation.position, transform.position) <= blasterRadius) {
	        	stop();
	        	Vector3 direction = (playerlocation.position - transform.position).normalized;
	        	direction = new Vector3(direction.x, 0f, direction.z);
	            transform.rotation = Quaternion.LookRotation(direction);

	            // apply force
	            playerlocation.GetComponent<ThirdPersonController>().LostControl(blastTimer, true);
	        	playerlocation.GetComponent<Rigidbody>().AddForce(direction * blasterPower * 1.5f, ForceMode.Impulse);
	        	gameObject.GetComponent<Rigidbody>().AddForce(-direction * blasterPower, ForceMode.Impulse);

	        	blasterParticle.Play();

	        	blasting = true;
	        	blastingCountDown = blastTimer;

                // play audio
                boostSound.Play();
	        }
    	}
        
    }
}
