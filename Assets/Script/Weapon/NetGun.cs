using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetGun : Weapons
{
    protected override void WeaponBehavior ()
    {
		Rigidbody bulletrb = Instantiate(bullet, barrelPivot.position, barrelPivot.rotation).GetComponent<Rigidbody>();
		bulletrb.velocity = barrelPivot.forward * power;

		--currentMag;
    	
    }
}
