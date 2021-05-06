using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animal))]

public class FieldOfView : MonoBehaviour
{
	public Transform fiewPoint;
	public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	[SerializeField] LayerMask targetMask;
	[SerializeField] LayerMask obstacleMask;

	Animal animal;

	void Start() 
	{
		StartCoroutine("FindTargetsWithDelay", .2f);
		animal = gameObject.GetComponent<Animal>();
	}

	IEnumerator FindTargetsWithDelay (float delay) 
	{
		while (true) {
			yield return new WaitForSeconds(delay);
			FindVisibleTarge();
		}
	}

	void FindVisibleTarge() {
		bool result = false;
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		if (targetsInViewRadius.Length > 0) {
			Transform target = targetsInViewRadius[0].transform.root;
			Vector3 dirToTarget = (target.position - fiewPoint.position).normalized;

			if (Vector3.Angle(fiewPoint.forward, dirToTarget) < viewAngle/2) {
				float disToTarget = Vector3.Distance (fiewPoint.position, target.position);

				if (!Physics.Raycast(fiewPoint.position, dirToTarget, disToTarget, obstacleMask)) {
					result = true;
				}
			}
		}

		if (!result && animal.seePlayer) {
			animal.lostVision = true;
		}
		
		animal.seePlayer = result;

	}

	public Vector3 DirFromAngle (float angleInDegrees, bool angleGobal) {
		if (!angleGobal) {
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0 , Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}