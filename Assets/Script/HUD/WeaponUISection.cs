using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUISection : MonoBehaviour
{
	public List<Graphic> weapons = new List<Graphic>();

    [Range(0,1)]
    [SerializeField] float weaponAlpha;

    [SerializeField] GameObject ammoDisplay;
    [SerializeField] GameObject ammoIconPrefab;

    List<Image> ammoList  = new List<Image>();

    [SerializeField] Sprite captureGunAmmo, TetherGunAmmo, TrapAmmo;
    [Range(0,1)]
    [SerializeField] float ammoAlpha;

    void Start() {
        foreach (Graphic g in weapons) {
            g.gameObject.SetActive(false);
        }

        ammoDisplay.SetActive(false);
    }

    public void ChangeWeapon(string tag, int maxAmmo) {

        // change weapon icon
        foreach (Graphic g in weapons) {
            Color c = g.color;
            if (g.tag == tag) {
                c.a = 1; 
            }
            else {       
                c.a = weaponAlpha;    
            }
            g.color = c;
        }  	

        // clear current ammo ui
        foreach (Image ammo in ammoList) {
            Destroy(ammo.gameObject);
        }

        ammoList.Clear();


        // add new weapon ammo ui
        for (int i = 0; i < maxAmmo; ++i) {
            Image ammo = Instantiate(ammoIconPrefab.GetComponent<Image>());

            switch (tag) {
                case "NetGun":
                ammo.sprite = captureGunAmmo;
                break;

                case "TetherGun":
                ammo.sprite = TetherGunAmmo;
                break;

                case "Trap":
                ammo.sprite = TrapAmmo;
                break;

                default:
                break;
            }

            ammo.transform.SetParent(ammoDisplay.transform);
            ammoList.Add(ammo);
        }

    }

    public void AmmoDisplay(int current, int total) {

        for (int i = 0; i < total; ++i) {
            Color c = ammoList[i].color;
            if (i < current) {
                c.a = 1;
            }
            else {
                c.a = ammoAlpha;
            }
            ammoList[i].color = c;
        }
    }

    public void NewWeapon(string tag) {
        foreach (Graphic g in weapons) {
            if (g.tag == tag) {
                g.gameObject.SetActive(true);
                break;
            }  
        }

        ammoDisplay.SetActive(true);
    }
}
