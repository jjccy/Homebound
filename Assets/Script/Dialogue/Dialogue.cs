using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class Dialogue
{	
	public CinemachineVirtualCameraBase camera;

	[TextArea(1, 3)]
    public string instruction;

    [TextArea(3, 25)]
    public string[] sentences;
}
