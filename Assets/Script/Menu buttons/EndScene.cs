using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{	
	[SerializeField] private string mainMenu;

    public void ExitGame() {
		SceneManager.LoadScene(mainMenu);
	}
}
