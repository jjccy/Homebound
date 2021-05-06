using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadBar : MonoBehaviour
{
	public Transform reloadBar;
	public Text reloadTimeDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BarUpdate(float fill, float time) {
    	reloadBar.GetComponent<Image>().fillAmount = fill;
    	reloadTimeDisplay.text = time.ToString("n1");
    }
}
