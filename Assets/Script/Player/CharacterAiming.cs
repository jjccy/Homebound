using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
	public Transform CameraLookAt;
	public Cinemachine.AxisState xAxis;
	public Cinemachine.AxisState yAxis;

	ThirdPersonController controller;
	Animator animator;

    public bool isAiming = false;
	int isAimingParam = Animator.StringToHash("isAiming");

    float turnSmoothVelocity;

    Inventory inventory;

    [SerializeField] Transform playerLookingMinimapIcon;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        if (!inventory.isOpen()) {
            if (controller.isControl()) {
                //aim check
                isAiming = Input.GetMouseButton(1);       
                animator.SetBool(isAimingParam, isAiming);

                // rotation + movement
                float horizontal = Input.GetAxisRaw("Horizontal");
                float vertical = Input.GetAxisRaw("Vertical");
                Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

                controller.moveDir = Vector3.zero;

                if (direction.magnitude >= 0.1f) 
                {
                    
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + controller.camTransform.eulerAngles.y;

                    // if not aiming
                    if (!isAiming) {
                        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, controller.turnSmoothTime);
                        transform.rotation = Quaternion.Euler(0f, angle, 0f);

                        if (!controller.displayWeaponWhenWalking && controller.weapons.Count > 0) {
                            controller.weapons[controller.getCurrentWeapon()].GetComponent<Weapons>().RenderDisplay(isAiming);
                        }        
                    }
                    
                    controller.moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                }

                // if aiming
                if (isAiming) {
                    float targetAngle = controller.cam.GetComponent<Transform>().eulerAngles.y;

                    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
                    // CameraLookAt.eulerAngles = new Vector3(0 , 0, 0);


                    // if player has weapon
                    if (controller.weapons.Count > 0) {
                        controller.weaponAttactedPoint.rotation = controller.cam.GetComponent<Transform>().rotation;

                        controller.weapons[controller.getCurrentWeapon()].GetComponent<Weapons>().RenderDisplay(true);




                        if (controller.weapons[controller.getCurrentWeapon()].tag == "Trap" ||
                            controller.weapons[controller.getCurrentWeapon()].tag == "Lure") {


                            Ray ray = controller.cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                            RaycastHit hit;

                            if (Physics.Raycast(ray, out hit, 15, LayerMask.GetMask("Ground"))) {

                                if (hit.transform != null) {

                                    controller.weapons[controller.getCurrentWeapon()].GetComponent<PlaceableWeapon>().PlaceReference(hit);
                                }

                            }
                        }


                        // Weapon firing
                        if (Input.GetMouseButtonDown(0)) {
                            if (controller.weapons[controller.getCurrentWeapon()] != null) {
                                controller.weapons[controller.getCurrentWeapon()].GetComponent<Weapons>().Fire();
                            }
                        }
                    }

                    
                }

                //  end control
            } 
            // end check if player is in control


            // camera aiming

       
            xAxis.Update(Time.deltaTime);
            yAxis.Update(Time.deltaTime);

            CameraLookAt.eulerAngles = new Vector3(yAxis.Value , xAxis.Value, 0);


            // update minimap player look at icon
            playerLookingMinimapIcon.eulerAngles = new Vector3(0, xAxis.Value, 0);
        }
        
    }


}
