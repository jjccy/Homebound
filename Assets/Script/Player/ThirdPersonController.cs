using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
	Rigidbody rb;
    public Animator animator;

    int animateWalkingSpeed = Animator.StringToHash("walkingSpeed");
    int jumpTrigger = Animator.StringToHash("jump");
    int isSpriting = Animator.StringToHash("isSpriting");
    int isFlying = Animator.StringToHash("isFlying");
    int isDead = Animator.StringToHash("isDead");
    int isLostControl = Animator.StringToHash("isLostControl");

    public Camera cam;
    public Transform camTransform;

    public CinemachineVirtualCamera cinemachines;

    [SerializeField] Transform respawnLocation;

    public bool displayWeaponWhenWalking = false;

	public UIScript UI;

	public float speed = 16f;
	public float maxVelocityChange = 10f;
	public Vector3 moveDir;

	public float turnSmoothTime = 0.1f;

    [SerializeField] Transform groundCheckPoint;
    [SerializeField] float groundHeight, jumpPower, jumpToBostCooldown;
    [SerializeField] LayerMask notGroundLayer;
    float jumpToBostCooldownCount;


    public float sprinting = 32f;
    public float sprintingCost = 5f;

	public float liftingpSpeed = 500f;
	public float maxLiftingSpeed = 10f;
    public float liftingpCost = 10f;

    public ParticleSystem liftingParticle;

    public float maxOverHeatTime = 30f;
    public float overHeatTimer = 30f;	
    public float liftingCoolDown = 5f;
    public float sprintCoolDown = 1f;


    public LayerMask targetLayer;

    public GameObject focus;

    public float maxEnergy = 600f;
    public float currentEnergy;

    [SerializeField] float deadTimer = 5f;

    public int currentResource = 0; 

    public bool pathFinding = false;

    public Transform weaponAttactedPoint;	
	public List<GameObject> weapons = new List<GameObject>();
	int currentWeapon = -1;

    CharacterAiming characterAiming;
    Inventory inventory;

    bool lostControl, onGround, jump, dead, haventDeadYet = true, firstLowOxygen = true;
    float lostControlCountdown;

    Hover hover;

    [SerializeField] AudioSource walkAudio, sprintAudio, flyAudio;

    [SerializeField] OnQueDialogue firstDeathDialogue, firstLowOxygenDialogue;


    // Start is called before the first frame update
    void Start()
    {
    	rb = GetComponent<Rigidbody>();

        camTransform = cam.GetComponent<Transform>();

        currentEnergy = maxEnergy;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        characterAiming = GetComponent<CharacterAiming>();
        inventory = GetComponent<Inventory>();

        overHeatTimer = maxOverHeatTime;

        liftingParticle.Stop();

        hover = GetComponent<Hover>();
    }

    // Update is called once per frame
    void Update()
    {   

        // if only player has controle
        if (!lostControl) {

            // interact with object by pressing F 
            handleInteraction();

            // minimap on off

            if (Input.GetKeyDown(KeyCode.M)) {
            	UI.AffectMinimap();
            }


            if (Input.GetKeyDown(KeyCode.Space)) {
                // try to jump
                jump = true;
            }


            // if weapon slot is not empty
            if (weapons.Count > 0) {

                // weapon reload
                if (Input.GetKeyDown(KeyCode.R) && weapons.Count > 0) {
                    weapons[currentWeapon].GetComponent<Weapons>().Reload();
                }

                // check if display weapon
                if (Input.GetKeyDown(KeyCode.T)){
                    displayWeaponWhenWalking = !displayWeaponWhenWalking;

                    weapons[currentWeapon].GetComponent<Weapons>().RenderDisplay(displayWeaponWhenWalking);
                }

                // check mouse scrolling
                if (Input.GetAxisRaw("Mouse ScrollWheel") != 0) {
                    weapons[currentWeapon].GetComponent<Weapons>().RenderDisplay(false);

                    if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
                    {
                        //wheel goes up
                        --currentWeapon;

                        if (currentWeapon < 0) {
                            currentWeapon = weapons.Count - 1;
                        }
                    }
                    else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
                    {
                     //wheel goes down

                        ++currentWeapon;

                        if (currentWeapon >= weapons.Count) {
                            currentWeapon = 0;
                        }
                    }

                    UI.ChangeWeapon();

                    if (displayWeaponWhenWalking) {
                        weapons[currentWeapon].GetComponent<Weapons>().RenderDisplay(true);
                    }  

                }   
            }


            // reduce engery overtime, every second coust 1 energy
            currentEnergy -= Time.deltaTime;
            currentEnergy = Mathf.Max(currentEnergy, 0); 

            // trigger dialogue first time oxygen less than 25%
            if (firstLowOxygen && currentEnergy < (maxEnergy * 0.25f)) {
                firstLowOxygenDialogue.TriggerDialogue();
                firstLowOxygen = false;
            }
        }
        

        // run out of oxygen
        if (currentEnergy <= 0 && !dead) {
            LostControl(2, true);

            Die();
        }
    }


    void handleInteraction() {
        // interact with object by pressing F 

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, targetLayer)) {

            // if (hit.transform != null 
            //     && hit.transform.gameObject.layer == LayerMask.NameToLayer("Interactable")) {

            if (hit.transform != null) {

                GameObject interactable = hit.transform.gameObject;
                bool closeToInteract = interactable.GetComponent<Interactable>().distanceCheck(transform, camTransform);

                if (Input.GetKeyDown(KeyCode.F) && closeToInteract) {

                    SetFocus(interactable);
                }                
            }
            
        }

        Debug.DrawLine(camTransform.position, hit.point,Color.red);
        
        if (Input.anyKey && !Input.GetKey(KeyCode.F)){
            //remove focus for any other key input(for now, maybe better way next time)
            RemoveFocus();
        }

        if (focus != null) {
            FaceTarget();
        }

        // interaction part end
    }


    void SetFocus(GameObject newFocus)
    {
        if (newFocus != focus) 
        {
            if (focus != null)
            {
                focus.GetComponent<Interactable>().OnDefocused();
            }

            focus = newFocus;
        }
        
        newFocus.GetComponent<Interactable>().OnFocused(gameObject);
    }

    void RemoveFocus() {

        if (focus != null)
        {
            focus.GetComponent<Interactable>().OnDefocused();
        }
        
        focus = null;
    }

    void FaceTarget() {
        Vector3 direction = (focus.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void FixedUpdate()
    {	      
        animator.SetBool(isSpriting, false);

        if (!lostControl && !inventory.isOpen() && !dead) {
            // moveing

            Vector3 velocity = rb.velocity;
            Vector3 targetVelocity;
            Vector3 velocityChange;

            if (characterAiming.isAiming) {
                targetVelocity = moveDir * (speed / 2f);
            }
            else if (Input.GetKey(KeyCode.LeftShift) && moveDir.magnitude > 0.1f && currentEnergy > 0 && overHeatTimer > 0) {
                targetVelocity = moveDir * sprinting;
                currentEnergy -= Time.fixedDeltaTime * (sprintingCost - 1f);

                overHeatTimer -= Time.fixedDeltaTime * sprintCoolDown; 
                animator.SetBool(isSpriting, true);
                hover.enabled = true;

                walkAudio.Stop();
                if (!sprintAudio.isPlaying) sprintAudio.Play();
            }
            else {
                targetVelocity = moveDir * speed;
                
                if (!walkAudio.isPlaying && moveDir.magnitude > 0.1f) walkAudio.Play();
                else if (moveDir.magnitude < 0.1f) walkAudio.Stop();
            }

            if (!Input.GetKey(KeyCode.LeftShift) || moveDir.magnitude < 0.1f || currentEnergy <= 0 || overHeatTimer <= 0) {
                hover.enabled = false;
                sprintAudio.Stop();
            }

            // set animation 
            animator.SetFloat(animateWalkingSpeed, targetVelocity.magnitude);

            velocityChange = targetVelocity - velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            rb.AddForce(velocityChange, ForceMode.VelocityChange);

            // check player on ground
            onGround = Physics.Raycast(groundCheckPoint.position, Vector3.down, groundHeight, ~notGroundLayer);

            if (!onGround || jump) walkAudio.Stop();         

            if (jump && onGround && !Input.GetKey(KeyCode.LeftShift)) {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                jumpToBostCooldownCount = jumpToBostCooldown;
                jump = false;
                animator.SetTrigger(jumpTrigger);
            }        
            // apply lifing force when pressing space
            else if (Input.GetKey(KeyCode.Space) && 
                (!onGround || Input.GetKey(KeyCode.LeftShift)) && 
                jumpToBostCooldownCount < 0 && currentEnergy > 0 && overHeatTimer > 0)
            {   
                // Debug.Log("spaced hited");
                rb.AddForce(Vector3.up * (liftingpSpeed * (1 - (rb.velocity.y / maxLiftingSpeed))), ForceMode.Acceleration);
                currentEnergy -= Time.fixedDeltaTime * (liftingpCost - 1f);

                overHeatTimer -= Time.fixedDeltaTime * liftingCoolDown;   

                liftingParticle.Play();     

                walkAudio.Stop();  
                if (!flyAudio.isPlaying) flyAudio.Play(); 
            }
            else {
                liftingParticle.Stop();
                flyAudio.Stop(); 

                if (!(Input.GetKey(KeyCode.LeftShift) && moveDir.magnitude > 0.1f)
                    && rb.velocity.y > 0 && jumpToBostCooldownCount < 0) {
                    rb.AddForce(new Vector3(0,-rb.velocity.y * 0.1f,0), ForceMode.VelocityChange);
                }
                
            }




            if (jumpToBostCooldownCount >= 0) jumpToBostCooldownCount -= Time.fixedDeltaTime;
        }
        else if (lostControl){ // lost control
            liftingParticle.Stop();
            lostControlCountdown -= Time.fixedDeltaTime;

            walkAudio.Stop();
            sprintAudio.Stop();
            flyAudio.Stop();

            if (lostControlCountdown <= 0) {
                BackControl();
            }
        }

        animator.SetBool(isFlying, liftingParticle.isPlaying);
        


        if (overHeatTimer < maxOverHeatTime && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftShift)) {
            overHeatTimer += Time.fixedDeltaTime;
        }

    }


    public void LostControl (float f, bool animialAttack) {
        lostControl = true;
        lostControlCountdown = f;

        animator.SetFloat(animateWalkingSpeed, 0f);
        animator.SetBool(isFlying, false);
        animator.SetBool(isSpriting, false);

        if (animialAttack) animator.SetBool(isLostControl, true);

        hover.enabled = false;
    }

    public void BackControl() {
        lostControl = false;
        animator.SetBool(isLostControl, false);
    }

    public bool isControl() {
        return !lostControl && !dead;
    }

    // stop all movement
    public void Stop() {
        rb.velocity = Vector3.zero;
    }

    // when pick up new weapon
    public void AddWeapon (GameObject weapon) {
        GameObject newWeapon = Instantiate(weapon, weaponAttactedPoint.position, weaponAttactedPoint.rotation, weaponAttactedPoint);
        weapons.Add(newWeapon);     
        newWeapon.GetComponent<Weapons>().InintialWeapon();
        ++currentWeapon;


        UI.AddWeapon(newWeapon.tag);
    }

    // return current weapon
    public int getCurrentWeapon() {
        return currentWeapon;
    }

    // after player's oxygen is blow 0
    void Die() {
        rb.velocity = Vector3.zero;

        dead = true;
        animator.SetBool(isDead, true);

        StartCoroutine(Respawn());
    }

    IEnumerator Respawn() {
        yield return new WaitForSeconds(deadTimer);

        dead = false;
        animator.SetBool(isDead, false);

        lostControl = false;

        gameObject.GetComponent<Inventory>().clearInventory();
        currentEnergy = maxEnergy;
        overHeatTimer = maxOverHeatTime;

        transform.position = respawnLocation.position;

        if (haventDeadYet) {
            firstDeathDialogue.TriggerDialogue();
            haventDeadYet = false;
        }
    }


    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckPoint.position, new Vector3(groundCheckPoint.position.x, 
            groundCheckPoint.position.y - groundHeight, 
            groundCheckPoint.position.z));
    }
}
