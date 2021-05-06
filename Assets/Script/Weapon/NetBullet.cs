using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBullet : MonoBehaviour
{
	public GameObject checker;
	public float selfDestroyTimer;
	bool hit = false;

	public List<GameObject> capturedAnimals = new List<GameObject>();

	public LayerMask animalLayer;
    public LayerMask ignoreLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) {
        	selfDestroyTimer -= Time.deltaTime;

        	if (selfDestroyTimer <= 0 && capturedAnimals.Count <= 0){
        		Destroy(gameObject);
        	}
        }
    }

    void OnCollisionEnter(Collision collision)
    {
    	if (!hit && 
            ((ignoreLayer & 1 << collision.collider.gameObject.layer) != 1 << collision.collider.gameObject.layer)) {
            
    		hit = true;

    		Rigidbody rb = GetComponent<Rigidbody>();
    		rb.velocity = Vector3.zero;

    		checker.SetActive(true);

            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
    		rb.isKinematic = true;

    		//Destroy(gameObject, selfDestroyTimer);
    	}
    }

    void OnTriggerEnter(Collider collider)
    {
    	//Debug.Log(collider.gameObject.layer);
    	if (hit && selfDestroyTimer >= 0 &&
    		((animalLayer & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer)){

    		bool duplicate = false;
    		GameObject newAnimal = collider.transform.root.gameObject;

    		foreach (GameObject animal in capturedAnimals) {
    			if (animal == newAnimal) {
    				duplicate = true;
    				break;
    			}
    		}
    		

    		if (!duplicate && !newAnimal.GetComponent<Animal>().isCaptured) {
    			capturedAnimals.Add(newAnimal);
    			newAnimal.GetComponent<Animal>().GetCaptured();
                newAnimal.GetComponent<CapturedAnimal>().enabled = true;
    			newAnimal.GetComponent<CapturedAnimal>().bullet = this;
    		}

    		
    	}
        
        
    }
}
