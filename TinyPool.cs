/// TinyPool is a very small class that provides a
/// fast and easy way to pool objects in Unity.
/// Reach me at: twitter.com/c4m170 if you have any questions.
/// for more information visit: https://github.com/camiloei

using UnityEngine;
using System.Collections.Generic;

public class PoolableObject : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(false);
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.transform.localScale = scale;
        gameObject.SetActive(true);
        return gameObject;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}

public class Pool
{
    private List<PoolableObject> m_pooledObjects;
    private int m_maxPoolSize;
    private GameObject m_prefab;
    private GameObject m_objectsParent;

    public Pool(GameObject prefab, int initialPoolSize, int maxPoolSize, GameObject objectsParent)
    {
        m_pooledObjects = new List<PoolableObject>();
        m_prefab = (prefab != null) ? prefab : new GameObject("Pooled Object Default");
        m_maxPoolSize = maxPoolSize;
        m_objectsParent = (objectsParent != null) ? objectsParent : new GameObject("Pooled Objects Parent");
        initialPoolSize = (initialPoolSize < maxPoolSize) ? initialPoolSize : maxPoolSize;
        PoolObjects(initialPoolSize);
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        for (var i = 0; i < m_pooledObjects.Count; i++)
        {
            if (!m_pooledObjects[i].gameObject.activeSelf)
            {
                return m_pooledObjects[i].Spawn(position, rotation, scale);
            }
        }

        if (m_pooledObjects.Count < m_maxPoolSize)
        {
            Debug.LogWarning("Initial pool size reached (" + m_pooledObjects.Count + "). Adding one object more.");
            PoolableObject newObject = PoolObject();
            newObject.Spawn(position, rotation, scale);
            return newObject.gameObject;
        }
        else
        {
            Debug.LogWarning("Max pool size reached (" + m_maxPoolSize + ") Objects");
            return null;
        }
    }

    public void Destroy(GameObject pooledObject)
    {
        PoolableObject poolableObject = pooledObject.GetComponent<PoolableObject>();
        if (pooledObject == null)
        {
            Debug.LogError("Trying to destroy a non poolable object");
            return;
        }
        poolableObject.Destroy();
    }

    public void Expand(int extendQuantity)
    {
        if (extendQuantity + m_pooledObjects.Count > m_maxPoolSize)
        {
            extendQuantity = m_maxPoolSize - extendQuantity;
        }
        PoolObjects(extendQuantity);
    }

    private void PoolObjects(int quantity)
    {
        for (var i = 0; i < quantity; i++)
        {
            PoolObject();
        }
    }

    private PoolableObject PoolObject()
    {
        GameObject newObject = GameObject.Instantiate(m_prefab);
        PoolableObject poolableObject = newObject.AddComponent<PoolableObject>();
        poolableObject.Init();
        m_pooledObjects.Add(poolableObject);
        if (m_objectsParent != null)
        {
            newObject.transform.SetParent(m_objectsParent.transform);
        }
        return poolableObject;
    }
}

public static class TinyPool
{
    private static Dictionary<string, Pool> m_pools;

    public static void Initialize()
    {
        m_pools = new Dictionary<string, Pool>();
    }

    public static void CreatePool(GameObject prefab, string poolID, int initialPoolSize, int maxPoolSize = 50, GameObject objectsParent = null)
    {
        Pool pool = new Pool(prefab, initialPoolSize, maxPoolSize, objectsParent);
        m_pools.Add(poolID, pool);
    }

    public static GameObject Spawn(string poolID, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if(!m_pools.ContainsKey(poolID))
        {
            Debug.LogError("Trying to spawn using an invalid ID");
            return null;
        }
        return m_pools[poolID].Spawn(position, rotation, scale);
    }

    public static void Destroy(string poolID, GameObject pooledObject)
    {
        if (!m_pools.ContainsKey(poolID))
        {
            Debug.LogError("Trying to destroy using an invalid ID");
            return;
        }
        m_pools[poolID].Destroy(pooledObject);
    }

    public static void ExpandPool(string poolID, int extendQuantity)
    {
        if (!m_pools.ContainsKey(poolID))
        {
            Debug.LogError("Trying to expand a pool using an invalid ID");
            return;
        }
        m_pools[poolID].Expand(extendQuantity);
    }
}
