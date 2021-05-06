using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapHoldable : PlaceableWeapon
{
	Transform firstTrap;
	[SerializeField] GameObject line;
	LineRenderer drawLine;

	public float MaxDistance;

    protected override void WeaponBehavior ()
    {
    	if (isAiming && fire) {
    		


			if (firstTrap == null) {
				firstTrap = aimReference;

				drawLine = Instantiate(line, firstTrap.GetComponent<Trap>().selfLocation.position, Quaternion.identity, firstTrap).GetComponent<LineRenderer>();
				drawLine.gameObject.SetActive(false);

				aimReferenceRender.material = aiming;
			}

			else {
				aimReferenceRender.material = placedDown;	
				firstTrap.transform.Find("GFX").GetComponent<Renderer>().material = placedDown;

				firstTrap.GetComponent<Trap>().Place(aimReference.GetComponent<Trap>().selfLocation);
				aimReference.GetComponent<Trap>().Place(firstTrap.GetComponent<Trap>().selfLocation);

				firstTrap = null;

				drawLine.endColor = Color.yellow;
				drawLine = null;
			}

			aimReference = null;
			aimReferenceRender = null;

			fire = false;
			--currentMag;	
    	}
		
    }

    protected override void customePlaceRefernce() {

		if (firstTrap != null) {
			drawLine.gameObject.SetActive(true);
			drawLine.SetPosition(1, aimReference.GetComponent<Trap>().selfLocation.position - 
									firstTrap.GetComponent<Trap>().selfLocation.position);



			float distanceCheck = Vector3.Distance(firstTrap.position, aimReference.position);

			if (distanceCheck >= MaxDistance) {
				aimReferenceRender.material = warning;
				drawLine.endColor = Color.red;

				fire = false;
			}
			else {
				RaycastHit hit;

				Vector3 direction = (firstTrap.GetComponent<Trap>().selfLocation.position
				 - aimReference.GetComponent<Trap>().selfLocation.position).normalized;

				

        		if (Physics.Raycast(aimReference.GetComponent<Trap>().selfLocation.position, direction, out hit, distanceCheck) &&
					hit.collider != firstTrap.GetComponent<Collider>()) {


    				aimReferenceRender.material = warning;
    				drawLine.endColor = Color.red;
					fire = false;

        		}

        		else {
        			aimReferenceRender.material = aiming;
        			drawLine.endColor = Color.green;

					fire = true;
        		}
				
			}
		}
    }

    protected override void keepUpdating () {
    	if (Input.GetAxisRaw("Mouse ScrollWheel") != 0 && firstTrap != null) {
    		Destroy(firstTrap.gameObject);
    		firstTrap = null;
    		Destroy(aimReference.gameObject);
    		aimReference = null;
    		drawLine = null;
    		++currentMag;
    	}

    	if (!isAiming && firstTrap != null) {
    		drawLine.gameObject.SetActive(false);
    	}
    }
}
