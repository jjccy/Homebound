using UnityEngine;

public class SpawnPathFinder : Interactable
{
	public GameObject pathfinder;
	public Transform targetLocation;

	bool spawnPathFinder = false;

	float spawnGap = 1.5f;
	float spawnTimer;

	public float stopSpawnDistance = 7f;

	void Start() {
		spawnTimer = spawnGap;
	}

    protected override void Interact ()
    {
    	ThirdPersonController playerController = player.GetComponent<ThirdPersonController>();

    	if (!playerController.pathFinding) {
    		playerController.pathFinding = true;
    		spawnPathFinder = true;
    	}
    }

    protected override void constantUpdate ()
    {

    	if (spawnPathFinder) {
    		spawnTimer -= Time.deltaTime;


    		if (spawnTimer <= 0) {
    			//Transform spawnPosition = player.transform.Find("pathFinder_spawn_point").transform;
    			GameObject newPathFinder = Instantiate(pathfinder, player.transform.position, player.transform.rotation);

    			newPathFinder.GetComponent<PathFinder>().MoveToPoint(targetLocation.position);

    			//Debug.Log("spawn");

    			spawnTimer = spawnGap;
    		}



    		// player distance check
    		float distance = Vector3.Distance(player.transform.position, targetLocation.position);

	    	if (distance <= stopSpawnDistance) 
	    	{
	    		spawnPathFinder = false;
	    	}
    	}

    	
    	
    }
}
