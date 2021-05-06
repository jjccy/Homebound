using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherBullet : MonoBehaviour
{
	public float selfDestroyTimer;
	bool hit = false;

	public LayerMask animalLayer;
	public LayerMask ignoreLayer;
	public float stunTime;
	public float criticalStun;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
    	if (!hit && 
            ((ignoreLayer & 1 << collision.collider.gameObject.layer) != 1 << collision.collider.gameObject.layer)) {

    		hit = true;

    		Rigidbody rb = GetComponent<Rigidbody>();
    		rb.velocity = Vector3.zero;

 			rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    		rb.isKinematic = true;

    		


    		if ((animalLayer & 1 << collision.collider.gameObject.layer) == 1 << collision.collider.gameObject.layer){

	    		GameObject hitAnimal = collision.collider.transform.root.gameObject;

                gameObject.GetComponent<Transform>().parent = collision.collider.transform;

	    		if (collision.collider.gameObject.tag == "Head") {
	    			hitAnimal.GetComponent<Animal>().GetStuned(criticalStun);

	    			Debug.Log("head get hit");

                    Destroy(gameObject, criticalStun);
	    		}
	    		else {
	    			hitAnimal.GetComponent<Animal>().GetStuned(stunTime);
                    Destroy(gameObject, stunTime);
	    		}

	    		
	    	}
            else {
                Destroy(gameObject, stunTime);
            }
    	}
    }
}
