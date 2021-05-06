using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{	
	[SerializeField] GameObject spawnAniaml;
	[SerializeField] Vector2 spawnRadiuHeight;
	[SerializeField] int population, spawnGap;

	[SerializeField] Transform playerTransform;

	List<GameObject> animalList  = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < population; ++i) {
        	animalList.Add(SpawnAnimal());     	
        }

        StartCoroutine("AnimalInAreaCheck", spawnGap);
    }

    IEnumerator AnimalInAreaCheck (float delay) 
	{
		while (true) {
			yield return new WaitForSeconds(delay);

			foreach (GameObject animal in animalList) 
		    {  
		    	if (animal == null) {
		    		animalList.Remove(animal);
		    		break;
		    	}

		    }

		    if (animalList.Count < population 
		    	&& Vector3.Distance(playerTransform.position, transform.position) > 650) {

		    	Debug.Log(Vector3.Distance(playerTransform.position, transform.position));
		    	Debug.Log(spawnRadiuHeight.x * 1.5);

		    	animalList.Add(SpawnAnimal());  
		    }
		}
	}

    GameObject SpawnAnimal() {
    	Vector3 postion = new Vector3(transform.position.x + Random.Range(-spawnRadiuHeight.x , spawnRadiuHeight.x),
    								transform.position.y + Random.Range(0, spawnRadiuHeight.y + 1),
    								transform.position.z + Random.Range(-spawnRadiuHeight.x, spawnRadiuHeight.x));

        GameObject newAnimal = Instantiate(spawnAniaml, postion, transform.rotation);

        if (newAnimal.tag == "Flyboy") {
        	newAnimal.GetComponent<RandomFlyer>().radiusMinMax = new Vector2(0, spawnRadiuHeight.x);
        	newAnimal.GetComponent<RandomFlyer>().flyingTarget = transform;
        }
        else {
        	newAnimal.GetComponent<RandomMovement>().radiusMinMax = new Vector2(0, spawnRadiuHeight.x);
        	newAnimal.GetComponent<RandomMovement>().baseTarget = transform;
        }

        

        newAnimal.GetComponent<Animal>().playerlocation = playerTransform;

        return newAnimal.gameObject;
    }


    void OnDrawGizmosSelected() 
    {
    	Gizmos.color = Color.yellow;
    	Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadiuHeight.x * 2, (spawnRadiuHeight.y + 1) * 2, spawnRadiuHeight.x * 2));
    }
}
