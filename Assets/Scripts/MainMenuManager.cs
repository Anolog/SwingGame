using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        //Set the orientation
#if UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.orientation = ScreenOrientation.LandscapeLeft;
#endif
    }

    // Update is called once per frame
    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}

    public void PlayGame_ButtonPress()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame_ButtonPress()
    {
        Application.Quit();
    }
}
