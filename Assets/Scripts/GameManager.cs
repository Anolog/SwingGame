using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Vector3 m_StageDimensions;
    //private GameObject m_Player = null;

    //[SerializeField] private GameObject m_PlayerPrefab;

    public enum GAMESTATE
    {
        MAIN_MENU,
        OPTIONS_MENU,
        IN_GAME_MODE_ENDLESS,
        IN_GAME_MODE_LEVELS,
        PAUSED_GAME,
        GAME_OVER
    }

    private GAMESTATE m_CurrentGameState;

    public Vector3 GetStageDimensions()
    {
        return m_StageDimensions;
    }

    // Use this for initialization
    void Start ()
    {
        Instance = this;

        m_CurrentGameState = GAMESTATE.MAIN_MENU;

#if UNITY_EDITOR
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            m_CurrentGameState = GAMESTATE.IN_GAME_MODE_ENDLESS;
        }
#endif

	}

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update ()
    {
        m_StageDimensions = (new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0));
    }

    private void LateUpdate()
    {
        
    }

    public GAMESTATE GetCurrentGameState()
    {
        return m_CurrentGameState;
    }

    public void SetGameState(GAMESTATE aGameState)
    {
        m_CurrentGameState = aGameState;
    }


    /*
    public void CreatePlayer()
    {
        if (m_Player == null && m_PlayerPrefab != null)
        {
            m_Player = (GameObject)Instantiate(m_PlayerPrefab);
        }
    }
    */
}
