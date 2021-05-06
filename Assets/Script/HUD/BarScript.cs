using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
	public Slider slider;
	public Gradient gradient;
	public Image fill;

	public void SetMax (float max)
	{
		slider.maxValue = max;
		slider.value = max;

		fill.color = gradient.Evaluate(1f);
	}

    public void SetCurrent(float current) 
    {
    	slider.value = current;

    	fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
