using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LurePickup : Collectable
{
    [SerializeField] GameObject display;

    [SerializeField] float coolDown;


    protected override void collectableBehavior() 
    {
        display.SetActive(false);
        gameObject.layer = LayerMask.NameToLayer("Default");


        player.GetComponent<LureHoldable>().TriggerDialogue();

        StartCoroutine("backToUseable");
    }

    IEnumerator backToUseable () {
        yield return new WaitForSeconds(coolDown);

        gameObject.layer = LayerMask.NameToLayer("Interactable");
        display.SetActive(true);
    }

}