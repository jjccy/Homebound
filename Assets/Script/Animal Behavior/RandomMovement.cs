using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
	public float turnSpeed, maxVelocityChange, maxSpeed;
	[SerializeField] Vector2 moveSpeedMinMax, moveTimeMinMax, idelTimeMinMax, changeTargetEveryFromTo;


	public Transform moveToTarget, baseTarget;
	[SerializeField] Transform currentTarget;
    public Vector2 radiusMinMax;

	Rigidbody body; 
	Animator animator;

    bool move = true;

	[System.NonSerialized] public float changeTarget = 0f, timeSinceTarget = 0f, moving = 0f, stayStill = 0f, speed;
	private Vector3 rotateTarget, position, direction, velocity;
	private Quaternion lookRotation;
	[System.NonSerialized] public float distanceFromBase, distanceFromTarget;
    [System.NonSerialized] public bool stop, toLocation;

	void Start()
    {
        // Inititalize
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        direction = Quaternion.Euler(transform.eulerAngles) * (Vector3.forward);

        currentTarget = baseTarget;
    }


    void FixedUpdate() {

        Vector3 velocityChange;

        if (!stop) {

            if (!toLocation) {
                // Calculate distances
                distanceFromTarget = Vector3.Magnitude(currentTarget.position - body.position);

                // Time for a new target position
                if (changeTarget < 0f)
                {
                    rotateTarget = ChangeDirection(body.transform.position);
                    rotateTarget = new Vector3(rotateTarget.x, 0f, rotateTarget.z);
                    changeTarget = Random.Range(changeTargetEveryFromTo.x, changeTargetEveryFromTo.y);
                    timeSinceTarget = 0f;
                    speed = Random.Range(moveSpeedMinMax.x, moveSpeedMinMax.y);
                }

                // Time for move or idel
                if (move) {
                    moving -= Time.fixedDeltaTime;
                    if (moving <= 0f) {
                        moving = Random.Range(moveTimeMinMax.x, moveTimeMinMax.y);
                        move = false;
                    }
                }
                else {
                    stayStill -= Time.fixedDeltaTime;
                    if (stayStill <= 0f) {
                        stayStill = Random.Range(idelTimeMinMax.x, idelTimeMinMax.y);
                        move = true;
                    }
                }

                 // Rotate towards target
                Quaternion lookingRotation = Quaternion.LookRotation(new Vector3(rotateTarget.x, 0f, rotateTarget.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookingRotation, Time.deltaTime * turnSpeed);

                // Update times
                changeTarget -= Time.fixedDeltaTime;
                timeSinceTarget += Time.fixedDeltaTime;
            }
            else {
                Vector3 facingDirection = (moveToTarget.position - transform.position).normalized;
                Quaternion lookingRotation = Quaternion.LookRotation(new Vector3(facingDirection.x, 0f, facingDirection.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookingRotation, Time.deltaTime * turnSpeed);
            }

            // Move rabbit
            if (move || toLocation) {
                direction = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

                if (toLocation) velocityChange = (maxSpeed * direction - body.velocity);
                else velocityChange = (speed * direction - body.velocity);

                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            }
            else {


                velocityChange = (Vector3.zero - body.velocity);

                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                

                
            }
            
        }
        else {
            velocityChange = (Vector3.zero - body.velocity);

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        }

        velocityChange.y = 0;

        body.AddForce(velocityChange, ForceMode.VelocityChange);
    	
    }

    // Select a new direction to go in randomly
    private Vector3 ChangeDirection(Vector3 currentPosition)
    {
        Vector3 newDir;
        if (distanceFromTarget > radiusMinMax.y)
        {
            newDir = currentTarget.position - currentPosition;
            move = true;
            moving = moveTimeMinMax.y;
        }
        else if (distanceFromTarget < radiusMinMax.x)
        {
            newDir = currentPosition - currentTarget.position;
        } 
        else
        {
            // 360-degree freedom of choice on the horizontal plane
            float angleXZ = Random.Range(0, 360);
            // Calculate direction
            newDir = Mathf.Sin(angleXZ) * Vector3.forward + Mathf.Cos(angleXZ) * Vector3.right + 0f * Vector3.up;
        }
        return newDir.normalized;
    }

    public bool isMoving() {
        return move || toLocation;
    }
}
