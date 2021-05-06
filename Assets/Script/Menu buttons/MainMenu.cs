using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{	
	[SerializeField] private string playScene, developerScene; //[SerializeField] allows you to see private variables in the editor without making them public

	void Start() { 
		// enable cursor
		
		Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

	}


	public void PlayGame() {
		SceneManager.LoadScene(playScene);
	}

	public void DeveloperOnly() {
		SceneManager.LoadScene(developerScene);
	}

	public void Quit() {
		Debug.Log("Quite");
		Application.Quit();
	}

}
