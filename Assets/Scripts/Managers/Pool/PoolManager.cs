using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject poolPrefab;
    public PoolData[] pools;

    private Dictionary<string, ObjectPool> poolList;

    // Start is called before the first frame update
    void Start()
    {
       InitilalizeAllPools();
       Pool.SetPoolManagerReference(this);
    }

    public void InitilalizeAllPools()
    {
        //Initialize the pool list
        poolList = new Dictionary<string, ObjectPool>();

        // Create all pool Gameobjects and initialize them with the correct objects and sizes;
        for (int i = 0; i < pools.Length; ++i)
        {
            GameObject obj = Instantiate(poolPrefab);

            obj.transform.position = gameObject.transform.position;
            obj.transform.SetParent(gameObject.transform);

            ObjectPool pool = obj.GetComponent<ObjectPool>();
            obj.name = " " + pools[i].name;
            pool.InitializePool(pools[i].prefab, pools[i].size);

            poolList.Add(pools[i].name, pool);
        }

    }

    public GameObject FindObject(string poolName)
    {
        // Finds the correct pool and grabs the object
        return poolList[poolName].GetObject();
    }

    public void PutBackObject(string poolName, GameObject obj)
    {
        // Returns the object to the correct pool
        poolList[poolName].ReturnObject(obj);
    }

    public void ResetObjects()
    {
        foreach (string poolKey in poolList.Keys)
        {
            poolList[poolKey].ResetObjects();
        }
    }

}
