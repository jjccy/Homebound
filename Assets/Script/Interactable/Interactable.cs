using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    protected bool isFocus = false;
    [System.NonSerialized] public GameObject player;

    bool hasInteracted = false;
    bool looking = false;

    public GameObject text;

    public Transform interactPoint;

    protected virtual void Interact () 
    {
    	 // this method is meant to override
    	Debug.Log("Interacting with " + transform.name);
    }

    protected virtual void constantUpdate() 
    {

    }

    void Start() {
        if (interactPoint == null) {
            interactPoint = transform;
        }

        gameObject.layer = LayerMask.NameToLayer("Interactable");
    }

    void Update () 
    {
    	if (looking) {
    		text.SetActive(true);
    	}
    	else {
    		text.SetActive(false);
    	}

    	if (isFocus && !hasInteracted) 
    	{
    		Interact();
    		hasInteracted = true;
    	}

    	looking = false;

        constantUpdate();
    }

    public void OnFocused (GameObject Player) 
    {
    	isFocus = true;
    	player = Player;
    	hasInteracted = false;
    }

    public void OnDefocused ()
    {
    	isFocus = false;
    	hasInteracted = false;
    }

    public bool distanceCheck(Transform playerTransform, Transform camTransform) 
    {	
        Vector3 direction = (interactPoint.position - camTransform.position).normalized;
        text.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));

    	float distance = Vector3.Distance(playerTransform.position, interactPoint.position);

    	bool result = (distance <= radius);
    	if (result) 
    	{
    		looking = true;
    	}

		return result;
    }

    void OnDrawGizmosSelected() 
    {
    	Gizmos.color = Color.yellow;
    	Gizmos.DrawWireSphere(interactPoint.position, radius);
    }

}
