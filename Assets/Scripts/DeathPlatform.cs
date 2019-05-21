using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlatform : MonoBehaviour
{
    public Camera m_Camera;
    public GameObject m_Platform1;
    public GameObject m_Platform2;

    public float m_DeathYPos;

    private bool m_Plat1OnScreen = true;

    private float m_DeathSize;

    // Use this for initialization
    void Start()
    {
        m_DeathSize = m_Platform1.GetComponent<SpriteRenderer>().size.x;
        ResetPlatformPos();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.GetStageDimensions().x >= (m_Platform1.transform.position.x + (m_Platform1.transform.position.x * 0.15f)))
        {
            m_Platform2.transform.position = new Vector3((m_Platform1.transform.position.x + m_DeathSize), m_DeathYPos, 0);
        }

        if (GameManager.Instance.GetStageDimensions().x >= (m_Platform2.transform.position.x + (m_Platform2.transform.position.x * 0.15f)))
        {
            m_Platform1.transform.position = new Vector3((m_Platform2.transform.position.x + m_DeathSize), m_DeathYPos, 0);
        }
    }

    public void ResetPlatformPos()
    {
        m_Platform1.transform.position = new Vector2(-GameManager.Instance.GetStageDimensions().x, m_DeathYPos);
        m_Platform2.transform.position = new Vector2(m_Platform1.transform.position.x + m_DeathSize, m_DeathYPos);
    }
}
