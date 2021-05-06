using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
	Rigidbody rb;


	// Desired hovering height.
	public float hoverHeight = 1f;

	// The force applied per unit of distance below the desired height.
    public float hoverForce = 1f;

    // The amount that the lifting force is reduced per unit of upward speed.
    // This damping tends to stop the object from bouncing after passing over
    // something.
    public float hoverDamp = 0.5f;

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {	
        // Hover 
        RaycastHit hit; 
        Ray hoverRay = new Ray(transform.position, Vector3.down);

        // Debug.DrawRay(transform.position, Vector3.down * hoverHeight);

        if (Physics.Raycast(hoverRay, out hit, hoverHeight) && !hit.collider.isTrigger) {
			// The "error" in height is the difference between the desired height
            // and the height measured by the raycast distance.
            float hoverError = hoverHeight - hit.distance;

            // Only apply a lifting force if the object is too low (ie, let
            // gravity pull it downward if it is too high).
            if (hoverError > 0)
            {
                // Subtract the damping from the lifting force and apply it to
                // the rigidbody.
                float upwardSpeed = rb.velocity.y;
                float lift = hoverError * hoverForce - upwardSpeed * hoverDamp;
                rb.AddForce(lift * Vector3.up, ForceMode.Acceleration);
            }
        }
    }
}
