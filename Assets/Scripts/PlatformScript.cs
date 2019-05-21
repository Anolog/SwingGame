using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformScript : MonoBehaviour
{
    private float m_Size;

    private bool m_SpawnedPlatform = false;

    private void Start()
    {
        m_Size = this.GetComponent<SpriteRenderer>().size.x;
    }

    private void OnEnable()
    {
        m_SpawnedPlatform = false;
    }

    // Update is called once per frame
    void LateUpdate ()
    {
        if (GameManager.Instance.GetStageDimensions().x > this.transform.position.x && m_SpawnedPlatform == false)
        {
            m_SpawnedPlatform = true;
            PlatformManager.Instance.SpawnPlatform();
            PlayerController.Instance.AddToScore();
        }

        if (GameManager.Instance.GetStageDimensions().x > this.transform.position.x + 15)
        //if (Vector3.Distance(GameManager.Instance.GetStageDimensions(), this.transform.position) > 10)
        {
            this.gameObject.SetActive(false);
        }
	}
}
