using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyboy : Animal
{

    protected override void stop() {
        gameObject.GetComponent<RandomFlyer>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = true;
    }

    protected override void moveAgain() {
        // gameObject.GetComponent<AutoMoveRotate>().rotation = new Vector3(0 , Random.Range(-.9f, .9f) , 0);
        // gameObject.GetComponent<AutoMoveRotate>().enabled = true;

        gameObject.GetComponent<RandomFlyer>().enabled = true;
        gameObject.GetComponent<RandomFlyer>().homeTarget = null;
        gameObject.GetComponent<RandomFlyer>().returnToBase = false;

        gameObject.GetComponent<Rigidbody>().useGravity = false;
    }

    protected override void toLocation(Transform targetLocation) {
        // Vector3 direction = (targetLocation.position - transform.position).normalized;
        // Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        // gameObject.GetComponent<Transform>().rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

    	gameObject.GetComponent<RandomFlyer>().homeTarget = targetLocation;
        gameObject.GetComponent<RandomFlyer>().returnToBase = true;
    }

    protected override void luredAction() {
        // override action if get lured


        float distance = Vector3.Distance(lure.transform.position, transform.position);

        if (distance > lureDistance) {
            toLocation(lure.transform);
        }
        else { // reaches lured location
            animator.SetBool(isLuredParam, isLured);

            statusSprite.GetComponent<SpriteRenderer>().sprite = hungrySprite;
        }
    }
}
