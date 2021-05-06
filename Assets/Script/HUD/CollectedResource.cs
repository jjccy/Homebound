using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedResource : MonoBehaviour
{
    public Text text;

	public void SetText (int num)
	{
		text.text = num.ToString();
	}
}
