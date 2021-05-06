using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
	public int magazineSize;
	protected int currentMag;
	public float reloadTime;
	public float reloadCountDown;
	public int totalAmmo;
	public float power;
	public GameObject bullet;
	public Transform barrelPivot;
	protected bool reloading = false;

    protected Renderer GFX;

    void Start() {
        GFX = transform.Find("GFX").GetComponent<Renderer>();
    }


    // Update is called once per frame
    void Update()
    {

        constantUpdate(); 
    	

        if (reloading) {
        	reloadCountDown -= Time.deltaTime;

        	if (reloadCountDown <= 0) {
        		reloading = false;

          //       int deductAmmo = magazineSize - currentMag;

        		// currentMag += Mathf.Min(deductAmmo, totalAmmo);	
          //       totalAmmo -= deductAmmo;
        		// totalAmmo = Mathf.Max(0, totalAmmo);
                currentMag += magazineSize;
        	}
        }
    }

    public void Reload() {
        if (!reloading && currentMag < magazineSize && totalAmmo > 0) {

            reloading = true;
            reloadCountDown = reloadTime;
        }
    }

    public void Fire() 
    {
    	if (!reloading) {
    		WeaponBehavior();

    		if (currentMag <= 0) {
	    		Reload();
	    	}
    	}
    	
    }

    public bool isReloading() 
    {
        return reloading;
    }

    public int getCurrentMag() {
        return currentMag;
    }


    protected virtual void constantUpdate() 
    {

    }

    protected virtual void WeaponBehavior () {
    	// this method is meant to override
    	Debug.Log("Fire " + bullet.transform.name);
    } 

    public void InintialWeapon() {
        currentMag = magazineSize;
        RenderDisplay(false);
    } 

    public void RenderDisplay(bool display) {
        GFX = transform.Find("GFX").GetComponent<Renderer>();
        GFX.enabled = display;
    }
 }
