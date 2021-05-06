using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherGun : Weapons
{
	public Renderer projectileRender;

	protected override void constantUpdate() 
    {
    	if (GFX.enabled) {
    		if (!reloading) {
    			projectileRender.enabled = true;
    		}
    		else {
    			projectileRender.enabled = false;
    		}
    		
    	}
    	else {
    		projectileRender.enabled = false;
    	}
    }

    protected override void WeaponBehavior ()
    {
		Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
		bulletrb.velocity = barrelPivot.forward * power;

		--currentMag;
    	
    }
}
