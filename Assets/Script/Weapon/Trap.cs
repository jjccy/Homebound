using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
	bool placed = false;
	public float checkDistance;
	public Transform pairTrap;
	public Transform selfLocation;

	public LayerMask animalLayer;

    public float selfDestoryTimer;

    // Update is called once per frame
    void Update()
    {
        if (placed) {

        	RaycastHit hit;

            Vector3 direction = (pairTrap.position - selfLocation.position).normalized;

        	if (Physics.Raycast(selfLocation.position, direction, out hit, checkDistance / 1.9f , animalLayer)
        		&& hit.collider.transform.root.tag != "Flyboy") {

                hit.collider.transform.root.GetComponent<Animal>().GetCaptured();
        	}
        }
    }

    public void Place(Transform otherOne) {
    	placed = true;
    	checkDistance = Vector3.Distance(selfLocation.position, otherOne.position);
    	Destroy(gameObject, selfDestoryTimer);

    	pairTrap = otherOne;
    }
}
