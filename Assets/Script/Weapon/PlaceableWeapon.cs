using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableWeapon : Weapons
{

	public Transform aimReference;
	protected bool isAiming = false;

	public Material placedDown;
	public Material aiming;
    public Material warning;

    protected bool fire = false;

    protected Renderer aimReferenceRender;

    void Start()
    {
        currentMag = magazineSize;
        GFX = transform.Find("GFX").GetComponent<Renderer>();
        RenderDisplay(false);

        placedDown = bullet.transform.Find("GFX").GetComponent<Renderer>().sharedMaterial;
    }

    protected override void constantUpdate() 
    {
        if (currentMag > 0) {
            reloading = false;
        }

    	if (aimReference != null) {
    		if (isAiming) {
    		  aimReference.gameObject.SetActive(true);
	    	}
	    	else {
	    		aimReference.gameObject.SetActive(false);
	    	}
    	} 	

        keepUpdating();

        isAiming = false;
    }

    public void PlaceReference (RaycastHit hit) {
    	if (currentMag > 0) {
    		if (aimReference == null) {
    			aimReference = Instantiate(bullet).GetComponent<Transform>();
                aimReferenceRender = aimReference.transform.Find("GFX").GetComponent<Renderer>();

    			aimReferenceRender.material = aiming;

                fire = true;
    		}

    		aimReference.position = hit.point;
    		aimReference.up = hit.normal;

            isAiming = true;

    		customePlaceRefernce();	
    	}
    }

    protected virtual void customePlaceRefernce() {

    }

    protected virtual void keepUpdating() {

    }
}
