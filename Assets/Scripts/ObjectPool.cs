using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public GameObject m_PooledObject;
    private int m_PoolAmount = 30;
    private bool m_WillGrow = false;

    private List<GameObject> m_ObjectPool;

    public void SetPoolAmount(int aAmount)
    {
        m_PoolAmount = aAmount;
    }

    public void SetGrow(bool aGrow)
    {
        m_WillGrow = aGrow;
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_ObjectPool = new List<GameObject>();
        
        for (int i = 0; i < m_PoolAmount; i++)
        {
            GameObject obj = (GameObject)Instantiate(m_PooledObject);
            obj.SetActive(false);
            obj.name += " " + i;
            m_ObjectPool.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < m_ObjectPool.Count; i++)
        {
            if (!m_ObjectPool[i].activeInHierarchy)
            {
                return m_ObjectPool[i];
            }
        }

        if (m_WillGrow)
        {
            GameObject obj = (GameObject)Instantiate(m_PooledObject);
            m_ObjectPool.Add(obj);
            return obj;
        }

        return null;
    }

    public void ResetPoolInactive()
    {
        for (int i = 0; i < m_PoolAmount; i++)
        {
            m_ObjectPool[i].SetActive(false);
        }
    }
}
