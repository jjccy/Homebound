using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AutoMoveRotate : MonoBehaviour
{
	Rigidbody rb;
	public float speed;
	public Vector3 rotation;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

        void FixedUpdate()
    {	

    	// if (Input.GetKey(KeyCode.LeftShift)) {
    	// 	rb.MovePosition(rb.position + moveDir * speed * boosting * Time.fixedDeltaTime);
    	// }
    	// else {
    	// 	rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);
    	// }

    	// Apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.velocity;
//         Vector3 targetvelocity = GetComponent<Transform>().forward;
//         Debug.Log(targetvelocity);
//         targetvelocity.x = (targetvelocity.x + 1) * movement.x;
//         targetvelocity.z = (targetvelocity.z + 1) * movement.z;
//         targetvelocity.y = (targetvelocity.y + 1) * movement.y;
// Debug.Log(targetvelocity);
        
        Vector3 velocityChange = (GetComponent<Transform>().forward * speed  - velocity);

        velocityChange.x = Mathf.Clamp(velocityChange.x, -999, 999);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -999, 999);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        transform.rotation = Quaternion.Euler(transform.eulerAngles + rotation / 2f);

        // rb.AddTorque(rotation, ForceMode.VelocityChange);       

    }
}
