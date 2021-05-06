using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PathFinder : MonoBehaviour
{
	NavMeshAgent agent;
	public GameObject gfx;
	public float selfDestoryTimer = 7f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        selfDestoryTimer -= Time.deltaTime;

        if (selfDestoryTimer <= 7) {
        	gfx.SetActive(true);
        }

        if (selfDestoryTimer <= 0) {
        	Destroy(gameObject);
        }
    }

    public void MoveToPoint(Vector3 point) 
    {	
    	gfx.SetActive(false);
    	agent = GetComponent<NavMeshAgent>();
    	agent.SetDestination(point);
    }
}
