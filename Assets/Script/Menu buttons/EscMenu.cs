using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
	[SerializeField] private string mainMenu;
	[SerializeField] GameObject EscPopoutMenu;


	// development purpose only, allow to keeping playing with out calling escape menu while pressing esc
	[SerializeField] bool callEscapeMenu = true;

	void Update(){
		if (Input.GetKeyDown("escape") && callEscapeMenu)
        {
        	EscPopoutMenu.SetActive(true);

          	Cursor.lockState = CursorLockMode.Confined;
        	Cursor.visible = true;

			Time.timeScale = 0.0f;
        }
	}

	public void ReturnToGame(){
		EscPopoutMenu.SetActive(false);

		Cursor.lockState = CursorLockMode.Locked;
    	Cursor.visible = false;

		Time.timeScale = 1.0f;
	}

	public void ExitGame() {
		Time.timeScale = 1.0f;
		SceneManager.LoadScene(mainMenu);
	}
}
