using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;

    public Button PlayAgainButton;
    public Button ExitButton;

	// Use this for initialization
	void Start ()
    {
        Instance = this;

        PlayAgainButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void ShowUI()
    {
        PlayAgainButton.gameObject.SetActive(true);
        ExitButton.gameObject.SetActive(true);
    }

    public void PlayAgainEndless()
    {
        PlatformManager.Instance.ResetAllPlatformsInPool();
        PlatformManager.Instance.SpawnInitialPlatforms();
        PlayerController.Instance.RespawnPlayer();
        GameManager.Instance.SetGameState(GameManager.GAMESTATE.IN_GAME_MODE_ENDLESS);

        PlayAgainButton.gameObject.SetActive(false);
        ExitButton.gameObject.SetActive(false);
    }


    public void ExitToMenu()
    {
        GameManager.Instance.SetGameState(GameManager.GAMESTATE.MAIN_MENU);
        SceneManager.LoadScene("MainMenuScene");
    }
}
