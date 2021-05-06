using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapturedAnimal))]

public class Animal : MonoBehaviour
{
	[System.NonSerialized] public bool isStuned = false;
	[System.NonSerialized] public bool isCaptured = false;
	[System.NonSerialized] public bool isLured = false;
	public Collider interactCollider;

	[System.NonSerialized] public float stunedTimer;

	protected Animator animator;

	protected int isStunedParam = Animator.StringToHash("isStuned");
	protected int isCapturedParam = Animator.StringToHash("isCaptured");
	protected int isLuredParam = Animator.StringToHash("isLured");

    public GameObject lure;
    public float lureDistance = 5f;
    [System.NonSerialized] public float lureTimer = -999;

    public bool seePlayer = false, lostVision = false;
    public Transform playerlocation;

    Transform camLocation;
    [SerializeField] protected GameObject statusSprite;
    [SerializeField] Vector3 statusSpriteLocation;
    SpriteRenderer statusSpriteRender;

    [SerializeField] protected Sprite angerSprite, capturedSprite, hungrySprite, stunedSprite, surprisedSprite;

    [SerializeField] Transform groundCheckPoint;
    [SerializeField] LayerMask notGroundLayer;
    [SerializeField] float animalHeight;
    protected bool onGround = true;

    // Start is called before the first frame update
    void Start()
    {
        startOverride();

        gameObject.layer = LayerMask.NameToLayer("Animal");

        animator = GetComponent<Animator>();

        animator.SetBool(isStunedParam, isStuned);
        animator.SetBool(isCapturedParam, isCaptured);
        animator.SetBool(isLuredParam, isLured);

        camLocation = Camera.main.GetComponent<Transform>();
        statusSpriteRender = statusSprite.GetComponent<SpriteRenderer>();
        statusSpriteRender.sprite = null;

        if (groundCheckPoint == null) groundCheckPoint = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (statusSprite.transform.position - camLocation.position).normalized;
        statusSprite.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        statusSprite.transform.position = transform.position + statusSpriteLocation;

        constanstUpdate();

        if (!isCaptured) {


            if (isStuned) {
                stunedTimer -= Time.deltaTime;

                if (stunedTimer <= 0) {
                    isStuned = false;

                    moveAgain();
                    animator.SetBool(isStunedParam, isStuned);

                    statusSpriteRender.sprite = null;
                }
            }
            else if (isLured) {
                luredAction();

                // if reaches lured location
                if (animator.GetBool(isLuredParam))
                {
                    lureTimer -= Time.deltaTime;

                    if (lureTimer < 0) {
                        ExitLure(lure);
                    }
                }
            }
            else if(seePlayer) {
                statusSpriteRender.sprite = angerSprite;
                seePlayerAction();
            }
            else if (lostVision) {
                lostVision = false;
                moveAgain();
            }
            else {
                statusSpriteRender.sprite = null;
            }
        }

    }

    void FixedUpdate () {
        onGround = Physics.Raycast(groundCheckPoint.position, Vector3.down, animalHeight, ~notGroundLayer);

        constanstFixedUpdate();
    }

	protected virtual void constanstUpdate() {
    }

    protected virtual void constanstFixedUpdate() {
    }

    protected virtual void startOverride() {

    }

    protected virtual void stop() {
        gameObject.GetComponent<AutoMoveRotate>().enabled = false;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    protected virtual void moveAgain() {
        gameObject.GetComponent<AutoMoveRotate>().rotation = new Vector3(0 , Random.Range(-.9f, .9f) , 0);
        gameObject.GetComponent<AutoMoveRotate>().enabled = true;
    }

    protected virtual void toLocation(Transform targetLocation) {
        Vector3 direction = (targetLocation.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        gameObject.GetComponent<Transform>().rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    protected virtual void luredAction() {
        // override action if get lured


        float distance = Vector3.Distance(lure.transform.position, transform.position);

        if (distance > lureDistance) {
            toLocation(lure.transform);
        }
        else { // reaches lured location
            stop();
            animator.SetBool(isLuredParam, isLured);

            statusSpriteRender.sprite = hungrySprite;
        }
    }

    protected virtual void seePlayerAction() {
        
    }

    public void GetCaptured() {

        if (!isCaptured) {
            isCaptured = true;

            stop();

            gameObject.GetComponent<Interactable>().enabled = true;
            gameObject.layer = LayerMask.NameToLayer("Interactable");

            animator.SetBool(isCapturedParam, isCaptured);

            interactCollider.enabled = true;

            statusSpriteRender.sprite = capturedSprite;

            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    public void GetStuned(float f) {
        if (!isCaptured) {
            isStuned = true;
            ExitLure(lure);

            stop();


            stunedTimer = f;

            animator.SetBool(isStunedParam, isStuned);

            statusSpriteRender.sprite = stunedSprite;
        }
    }

    public void GetLured(GameObject lurer, float luredTime) {

        if (!isCaptured && !isLured) {
            isLured = true;
            lure = lurer;
            lureTimer = luredTime;

            statusSpriteRender.sprite = surprisedSprite;
        }
    }

    public void ExitLure(GameObject inputLure) {
        if (lure != null && lure == inputLure) {
            isLured = false;
            lure = null;

            if (!isCaptured && !isStuned) {

                moveAgain();

                animator.SetBool(isLuredParam, isLured);
                statusSpriteRender.sprite = null;
            }
        }

    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheckPoint.position, new Vector3(groundCheckPoint.position.x, 
            groundCheckPoint.position.y - animalHeight, 
            groundCheckPoint.position.z));
    }
}
