using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public static PlatformManager Instance;
    public ObjectPool m_Platforms;

    private Vector3 m_StageDimensions;

    public GameObject m_InitialSpawnLocation;

    private float m_PlatformSize;

    private GameObject m_MostRecentSpawnPlatform;
    [SerializeField]
    private List<GameObject> m_LastThreePlatforms;
    private uint m_AmountOfPlatformsSpawned = 0;

    private float m_DeathY;

	// Use this for initialization
	void Start ()
    {
        Instance = this;

        GameObject temp = m_Platforms.GetPooledObject();
        m_PlatformSize = temp.transform.localScale.x * temp.GetComponent<SpriteRenderer>().size.x;
        temp.SetActive(false);

        SpawnInitialPlatforms();

        m_DeathY = GameObject.Find("DeathSquare").transform.position.y;

    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public void SpawnInitialPlatforms()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject temp = m_Platforms.GetPooledObject();

            Vector3 newPos = m_InitialSpawnLocation.transform.position + new Vector3(m_PlatformSize * i , 0, 0);

            temp.transform.position = newPos;

            temp.SetActive(true);

            m_MostRecentSpawnPlatform = temp;
        }
    }

    public void SpawnPlatform()
    {
        GameObject temp = m_Platforms.GetPooledObject();

        if (temp != null)
        {
            //float playerX = GameObject.Find("Player").transform.position.x;
            //float playerActualY = GameObject.Find("Player").transform.position.y;

            float playerX = m_MostRecentSpawnPlatform.transform.position.x;
            float playerY = m_MostRecentSpawnPlatform.transform.position.y;

            float x = Random.Range(playerX + m_PlatformSize, playerX + (m_PlatformSize * 2.5f));
            float y = Random.Range(playerY - 3f, playerY + 2f);

            if (playerY >= y)
            {
                if (Random.Range(0, 10) > 4)
                {
                    y += Random.Range(1, 4);
                }
            }

            if (Random.Range(0, 20) > 13)
            {
                x += Random.Range(1, 3);
            }

            //If the distance between the player and the new platform is > ~4 reset it
            if (y - playerY > playerY + 3)
            {
                y = Random.Range(playerY - 3f, playerY + 2f);
            }

            //Check how close platform is close to the ground
            else if (y - m_DeathY <= 3f)
            {
                y = Random.Range(playerY - 3f, playerY + 2f);
            }

            //Check if in specific range of platform
            RaycastHit2D hit = Physics2D.CircleCast(new Vector2(x, y), 1.0f, Vector2.zero);

            while (hit.collider != null)
            {
                if (hit.collider.tag == "Platform")
                {
                    x = Random.Range(playerX + m_PlatformSize * 3, playerX + (m_PlatformSize) * 5f);
                    y = Random.Range(playerY, playerY + 5f);

                    hit = Physics2D.CircleCast(new Vector2(x, y), 1.5f, Vector2.zero);
                }

                else
                {
                    //If the distance between the player and the new platform is > ~4 reset it
                    if (y - playerY > playerY + 3)
                    {
                        y = Random.Range(playerY - 3f, playerY + 2f);
                    }

                    else if (y - m_DeathY <= 3f)
                    {
                        y = Random.Range(playerY - 3f, playerY + 2f);
                    }

                    else
                    {
                        break;
                    }
                }
            }

            if (m_AmountOfPlatformsSpawned > 3)
            {
                if ((m_LastThreePlatforms[0].transform.position.y >
                     m_LastThreePlatforms[1].transform.position.y) &&
                    (m_LastThreePlatforms[1].transform.position.y >
                     m_LastThreePlatforms[2].transform.position.y))
                {
                    y = m_LastThreePlatforms[0].transform.position.y;
                    Debug.Log("Platform y position has been changed due to the last 3 platforms decreasing");
                }
            }

            temp.transform.position = new Vector2(x, y);

            temp.SetActive(true);

            //Set the most recent platform
            m_MostRecentSpawnPlatform = temp;

            m_AmountOfPlatformsSpawned++;
            //When anything gets added, just move the last two down and insert one at the beginning

            if (m_AmountOfPlatformsSpawned > 3)
            {
                m_LastThreePlatforms[2] = m_LastThreePlatforms[1];
                m_LastThreePlatforms[1] = m_LastThreePlatforms[0];
                m_LastThreePlatforms[0] = m_MostRecentSpawnPlatform;
            }

            else
            {
                m_LastThreePlatforms.Add(m_MostRecentSpawnPlatform);
            }

        }
    }

    public void ResetAllPlatformsInPool()
    {
        m_Platforms.ResetPoolInactive();

        m_AmountOfPlatformsSpawned = 0;

        m_LastThreePlatforms.Clear();

        m_LastThreePlatforms.Capacity = 3;
    }
}
