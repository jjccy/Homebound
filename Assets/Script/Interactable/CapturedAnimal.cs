using UnityEngine;

public class CapturedAnimal : Interactable
{
	[TextArea(3, 25)]
	[SerializeField] string description;

	public NetBullet bullet;

    protected override void Interact ()
    {
    	Inventory inventory = player.GetComponent<Inventory>();
	    inventory.addItem(gameObject.tag, description);

	    if (bullet != null) {
	    	bullet.capturedAnimals.Remove(gameObject);
	    }

	    player.GetComponent<ThirdPersonController>().focus = null;
	    Destroy(gameObject);
    }
}
