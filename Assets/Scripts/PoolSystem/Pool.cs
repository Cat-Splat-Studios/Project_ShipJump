using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is a static class to handle the spawning and despawning of objects from the object pools
 * Athuor: Matthew Douglas
 **/

[System.Serializable]
public struct PoolData
{
    public string name;
    public GameObject prefab;
    public int size;
}


public static class Pool
{
    static PoolManager poolManager;

    public static void SetPoolManagerReference(PoolManager manager)
    {
        poolManager = manager;
    }

    public static GameObject SpawnObject(string name)
    {
        GameObject obj = poolManager.FindObject(name);
        obj.SetActive(true);
        return obj;
           
    }

    public static void RemoveObject(string name, GameObject obj)
    {
        poolManager.PutBackObject(name, obj);
    }
}
