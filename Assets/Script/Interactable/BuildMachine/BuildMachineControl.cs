using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BuildMachineControl : MonoBehaviour
{	
	public CinemachineVirtualCamera cineMachines;
	Cinemachine3rdPersonFollow componentBase;
	[SerializeField] Transform cameraAiming;

    public float speed = 3.5f;
	private float X;
	private float Y;
	public float zoomSpeed = .3f;

	[SerializeField] BuildMachineInteraction buildMachineInteraction;
	public bool onFocus;
	public GameObject GUI, playerGUI;
	[SerializeField] GameObject buildButton;

	Inventory inventory;

	[SerializeField] LayerMask buildMask, preBuildLayer;

	[SerializeField] TMP_Text RolyPolyButton, BlastRabbitButton, TazerFaceButton, FlyboyButton;
	[SerializeField] TMP_Text RequireRolyPoly, RequireBlastRabbit, RequireTazerFace, RequireFlyboy;
    [SerializeField] GameObject requriements;


	[SerializeField] GameObject RolyPolyModel, BlastRabbitModel, TazerFaceModel, FlyboyModel;
	int rolypoly, blastrabbit, tazerface, flyboy;

	[SerializeField] Material aiming, warning;
	Material placedown;

	GameObject currentAnimal;
	Renderer currentAnimalRender;
	Transform currentAnimalTransform;

	[SerializeField] int targetrolypoly = 10, targetblastrabbit = 10, targettazerface = 5, targetflyboy = 10;
	[SerializeField] string EndScene;

    [SerializeField] CinemachineVirtualCamera endingCameraAngle;
    [SerializeField] Rigidbody buildingPlatform;
    [SerializeField] Vector3 flyingRotate;
    [SerializeField] float flyingForce;
    [SerializeField] float endingAnimaitonLength;
    bool builded;

    // Start is called before the first frame update
    void Start()
    {
       componentBase = cineMachines.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
       GUI.SetActive(false);
       buildButton.SetActive(false);

       rolypoly = 0;
       blastrabbit = 0;
       tazerface = 0;
       flyboy = 0;
    }

    // Update is called once per frame
    void Update()
    {	
        if (builded) {
            // Move flyer
            Vector3 rotation = new Vector3(flyingRotate.x * Time.deltaTime,
                                                flyingRotate.y * Time.deltaTime,
                                                flyingRotate.z * Time.deltaTime);
            buildingPlatform.transform.Rotate(rotation);

            Vector3 direction = Quaternion.Euler(buildingPlatform.transform.eulerAngles) * Vector3.up;
            buildingPlatform.velocity += flyingForce * direction * Time.deltaTime;


            return;
        }
    	// on interface control
        else if (onFocus) {

        	// keep cursor visible
        	Cursor.lockState = CursorLockMode.Confined;
        	Cursor.visible = true;

        	// keep player lost control
        	buildMachineInteraction.controller.LostControl(999, false);

        	// input check for camera
        	if(Input.GetMouseButton(1)) {
	             cameraAiming.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * speed, Input.GetAxis("Mouse X") * speed, 0));
	             X = cameraAiming.rotation.eulerAngles.x;
	             Y = cameraAiming.rotation.eulerAngles.y;
	             cameraAiming.rotation = Quaternion.Euler(X, Y, 0);
	        }
	        else if(Input.GetMouseButton(2)) {
	            cameraAiming.localPosition = new Vector3(cameraAiming.localPosition.x, 
									            	Mathf.Clamp(cameraAiming.localPosition.y - Input.GetAxis("Mouse Y") * speed, 2, 35), 
									             	cameraAiming.localPosition.z);
	         } 
	        else if(Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                //wheel goes up
				componentBase.CameraDistance -= zoomSpeed;
				componentBase.CameraDistance = Mathf.Max(10, componentBase.CameraDistance);
	
            }
            else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
             	//wheel goes down
				componentBase.CameraDistance += zoomSpeed;
				componentBase.CameraDistance = Mathf.Min(45, componentBase.CameraDistance);

            }
            else if (Input.GetKeyDown("escape"))
	        {
	        	ExitBuilding();
	        }



            // move animal models if is building
            if (currentAnimal != null) {
            	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            	RaycastHit hit;

            	// get mouse aiming position
            	if (Physics.Raycast(ray, out hit, 100, ~preBuildLayer)) {
            		currentAnimal.transform.position = hit.point;

            		GameObject hitCollider = hit.collider.gameObject;

            		// check if it hits where we want it to build
            		if (((buildMask & 1 << hitCollider.transform.parent.gameObject.layer) == 1 << hitCollider.transform.parent.gameObject.layer) || 
            			((buildMask & 1 << hitCollider.layer) == 1 << hitCollider.layer)) {
            			currentAnimalRender.material = aiming;

            			// if player place animal
            			if (Input.GetMouseButtonDown(0)) {
                            currentAnimal.transform.SetParent(hitCollider.transform);
            				PlaceAnimal();
            			}
            		}
            		else {
            			currentAnimalRender.material = warning;
            		}
            	}

            	// rotate animal model with WASD
            	float x = Input.GetAxisRaw("buildx");
            	float y = Input.GetAxisRaw("buildy");
            	float z = Input.GetAxisRaw("buildz");

            	Vector3 direction = new Vector3(x, y, z).normalized;

            	if (direction.magnitude >= 0.1f) 
            	{
        			currentAnimalTransform.localRotation *= Quaternion.Euler(direction.x, direction.y, direction.z);
        		}
            }

        }
    }

    public void StartBuilding() {
        requriements.SetActive(true);
    	buildMachineInteraction.controller.LostControl(999, false);
    	onFocus = true;

    	cineMachines.Priority += 2;

    	Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        GUI.SetActive(true);

        // get inventory data
        inventory = buildMachineInteraction.player.GetComponent<Inventory>();
        List<GameObject> itemList = inventory.getItemList();

		rolypoly = 0;
		blastrabbit = 0;
		tazerface = 0;
		flyboy = 0;

		// count animal
        foreach(GameObject item in itemList) {
        	switch (item.tag) {

				case "RabbitBlaster":
				++blastrabbit;
				break;

				case "RolyPoly":
				++rolypoly;
				break;

				case "Flyboy":
				++flyboy;
				break;

				case "TazerFace":
				++tazerface;
				break;

				default:
				break;

        	}
        }

        // set text for buttons
        RolyPolyButton.text = "RolyPoly: " + rolypoly;
        BlastRabbitButton.text = "BlastRabbit: " + blastrabbit;
        TazerFaceButton.text = "TazerFace: " + tazerface;
        FlyboyButton.text = "Flyboy: " + flyboy;

        // set text for requirements
        RequireRolyPoly.text = "-   " + Mathf.Max(targetrolypoly, 0);
        RequireBlastRabbit.text = "-   " + Mathf.Max(targetblastrabbit, 0);
        RequireTazerFace.text = "-   " + Mathf.Max(targettazerface, 0);
        RequireFlyboy.text = "-   " + Mathf.Max(targetflyboy, 0);
    }

    public void ExitBuilding() {
    	onFocus = false;
    	cineMachines.Priority -= 2;

    	Cursor.lockState = CursorLockMode.Locked;
    	Cursor.visible = false;

    	GUI.SetActive(false);

    	buildMachineInteraction.controller.BackControl();

    	if (currentAnimal != null) {
    		Destroy(currentAnimal);
    	}
    }

    public void BuildAnimal(string tag) {

    	if (currentAnimal != null) {
    		Destroy(currentAnimal);
    	}

    	// check if there is animal
    	if (rolypoly > 0 || blastrabbit > 0 || tazerface > 0 || flyboy > 0) {

    		// check which animal
    		switch (tag) {

				case "RabbitBlaster":
					if (blastrabbit > 0) currentAnimal = Instantiate(BlastRabbitModel);
					else return;
					break;

				case "RolyPoly":
					if (rolypoly > 0) currentAnimal = Instantiate(RolyPolyModel);
					else return;
					break;

				case "Flyboy":
					if (flyboy > 0) currentAnimal = Instantiate(FlyboyModel);
					else return;
					break;

				case "TazerFace":
					if (tazerface > 0) currentAnimal = Instantiate(TazerFaceModel);
					else return;
					break;

				default:
					break;

	    	}

	    	
	    	
	    	// get animal's render and material
	    	currentAnimalRender = currentAnimal.transform.Find("GFX").GetComponent<Renderer>();
	    	placedown = currentAnimalRender.sharedMaterial;
	    	currentAnimalTransform = currentAnimal.transform.Find("GFX");
    	}
    	

    	
    }

    // when player place animals 
    public void PlaceAnimal() {


    	// mins current have animal and update UI;
		switch (currentAnimal.tag) {

			case "RabbitBlaster":
				--blastrabbit;
				--targetblastrabbit;
				BlastRabbitButton.text = "BlastRabbit: " + blastrabbit;
				RequireBlastRabbit.text = "BlastRabbit: " + Mathf.Max(targetblastrabbit, 0);
				break;

			case "RolyPoly":
				--rolypoly;
				--targetrolypoly;
				RolyPolyButton.text = "RolyPoly: " + rolypoly;
				RequireRolyPoly.text = "RolyPoly: " + Mathf.Max(targetrolypoly, 0);
				break;

			case "Flyboy":
				--flyboy;
				--targetflyboy;
				FlyboyButton.text = "Flyboy: " + flyboy;
				RequireFlyboy.text = "Flyboy: " + Mathf.Max(targetflyboy, 0);
				break;

			case "TazerFace":
				--tazerface;
				--targettazerface;
				TazerFaceButton.text = "TazerFace: " + tazerface;
				RequireTazerFace.text = "TazerFace: " + Mathf.Max(targettazerface, 0);
				break;

			default:
				break;

    	}


    	if (targetblastrabbit <= 0 && targetrolypoly <= 0 && targetflyboy <= 0 && targettazerface <= 0) {
    		buildButton.SetActive(true);
    	}

    	inventory.RemoveFromInventory(currentAnimal.tag);

	
    	ChangeLayerRecursively(currentAnimal, "Build");


    	currentAnimalRender.material = placedown;
        currentAnimal = null;


    }

    public void ChangeLayerRecursively(GameObject parent, string targetlayer) {
    	parent.layer = LayerMask.NameToLayer(targetlayer);

    	foreach(Transform child in parent.transform) {
    		ChangeLayerRecursively(child.gameObject, targetlayer);
    	}
    }

    public void BuildSpaceship() {
        endingCameraAngle.Priority = 99;
        builded = true;
        buildingPlatform.isKinematic = false;
        ExitBuilding();
        playerGUI.SetActive(false);
    	
        StartCoroutine("toEndingScene");     
    }

    IEnumerator toEndingScene () {
        yield return new WaitForSeconds(endingAnimaitonLength);

        // end scene
        SceneManager.LoadScene(EndScene);
    }
}
