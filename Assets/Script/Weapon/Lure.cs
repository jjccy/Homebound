using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{

	public LayerMask animalLayer;
	public SphereCollider lureCollider;

	public float lureRange;

	public float duration;

	bool placed = false;

    public List<Animal> luredAnimal = new List<Animal>();
    public float luredTime;

    [SerializeField] Vector2 animalLuredMaxMin;

    int luredAmount;

    // Start is called before the first frame update
    void Awake()
    {
        lureCollider.radius = lureRange;
        lureCollider.enabled = false;


        luredAmount = Mathf.RoundToInt(Random.Range(animalLuredMaxMin.x, animalLuredMaxMin.y));
    }

    // Update is called once per frame
    void Update()
    {
        if (placed) {
        	duration -= Time.deltaTime;

            if(duration <= 1) {
                 foreach (Animal animal in luredAnimal) 
                 {  
                    if (animal != null) animal.ExitLure(gameObject);
                 }
            }
        }


    }

    public void Place() {
    	placed = true;
        Destroy(gameObject, duration);

        Debug.Log(transform.name);
        lureCollider.enabled = true;
        Debug.Log(lureCollider.enabled);
    }


    void OnTriggerStay(Collider collider) {
        if (placed && luredAnimal.Count <= luredAmount
            && (animalLayer & 1 << collider.transform.root.gameObject.layer) == 1 << collider.transform.root.gameObject.layer
            && collider.transform.root.gameObject.tag == gameObject.tag) {
            if (duration > 1) {
                Animal newAnimal = collider.transform.root.gameObject.GetComponent<Animal>();

                if (!luredAnimal.Contains(newAnimal)) {
                    luredAnimal.Add(newAnimal);
                    newAnimal.GetLured(gameObject, luredTime);
                }
            }           
            
        }
    }


}
