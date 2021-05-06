using UnityEngine;

public class Collectable : Interactable
{
	[TextArea(3, 25)]
	[SerializeField] string description;

    protected override void Interact ()
    {
    	Inventory inventory = player.GetComponent<Inventory>();
	    inventory.addItem(gameObject.tag, description);

	    player.GetComponent<ThirdPersonController>().focus = null;
	    // Destroy(gameObject);

	    collectableBehavior();
    }

    protected virtual void collectableBehavior() 
    {

    }

}
