using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroSceneTransition : MonoBehaviour
{
	VideoPlayer vp;

	[SerializeField] string nextScene;

    // Start is called before the first frame update
    void Start()
    {
    	Cursor.lockState = CursorLockMode.Locked;
    	Cursor.visible = false;
        vp = GetComponent<VideoPlayer>();
        vp.loopPointReached += EndReached;
    }

    void EndReached(UnityEngine.Video.VideoPlayer vp)
	{
	    vp.playbackSpeed = vp.playbackSpeed / 10.0F;
	    Cursor.lockState = CursorLockMode.Confined;
    	Cursor.visible = true;
    	SceneManager.LoadScene(nextScene);
	}
}
