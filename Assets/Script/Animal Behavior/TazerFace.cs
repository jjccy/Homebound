using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TazerFace : Animal
{   
    [SerializeField] ParticleSystem tazerEffect;
    RandomMovement movementController;

    [SerializeField] GameObject LightingEffects;
    List<GameObject>Lightings = new List<GameObject>();

    [SerializeField] Vector2 lightingDensityAndRange;
    [SerializeField] float angerCoolDown;
    float angerCoolDownCount;

    [SerializeField] float clearLightingDelay = 2f;

    [SerializeField] float stunedTime;

    [SerializeField] AudioSource zapAudio, thunderAudio;

    protected override void constanstUpdate() {

        if (angerCoolDownCount >= 0) {
            angerCoolDownCount -= Time.deltaTime;

            if (angerCoolDownCount <= 0) {
                tazerEffect.Play();

                zapAudio.Play();
            }
        }
    }

    protected override void startOverride() {
        movementController = gameObject.GetComponent<RandomMovement>();
        tazerEffect.Play();
        zapAudio.Play();
    }
    
    protected override void stop() {
        movementController.stop = true;
        movementController.toLocation = false;
    }

    protected override void moveAgain() {
        movementController.stop = false;
        movementController.toLocation = false;
    }

    protected override void toLocation(Transform targetLocation) {
    	movementController.moveToTarget = targetLocation;
        movementController.toLocation = true;
    }

    protected override void luredAction() {
        // override action if get lured


        float distance = Vector3.Distance(lure.transform.position, transform.position);

        if (distance > lureDistance) {
            toLocation(lure.transform);
        }
        else { // reaches lured location
            animator.SetBool(isLuredParam, isLured);
			stop();
            statusSprite.GetComponent<SpriteRenderer>().sprite = hungrySprite;
        }
    }

    protected override void seePlayerAction() {
    	
        if (angerCoolDownCount <= 0) {
            toLocation(playerlocation);

            // spawn lighiting
            if (Vector3.Distance(playerlocation.position, transform.position) <= lightingDensityAndRange.y / 2f) {
                angerCoolDownCount = angerCoolDown;

                for (int i = 0; i < lightingDensityAndRange.x; ++i) {
                    Vector3 position = new Vector3(transform.position.x + Random.Range(-lightingDensityAndRange.y / 2f , lightingDensityAndRange.y / 2f),
                                                    transform.position.y + LightingEffects.transform.position.y,
                                                    transform.position.z + Random.Range(-lightingDensityAndRange.y / 2f , lightingDensityAndRange.y / 2f));

                    GameObject newLighting = Instantiate(LightingEffects, position, LightingEffects.transform.rotation);

                    Lightings.Add(newLighting);
                }

                StartCoroutine("ClearLighting");           
                tazerEffect.Stop();


                // aduio effect stop zap sound play thunder sound
                thunderAudio.Play();
                zapAudio.Stop();

                moveAgain();


                // effects on player
                ThirdPersonController playerController = playerlocation.GetComponent<ThirdPersonController>();

                playerController.LostControl(stunedTime, true);
                playerController.currentEnergy -= 50;
            }
            
        }
    }

    IEnumerator ClearLighting () {
        yield return new WaitForSeconds(clearLightingDelay);

        foreach(GameObject lighting in Lightings) {
            Destroy(lighting);
        }

        thunderAudio.Stop();
    }
}
