/** 
* Author: Matthew Douglas
* Purpose: Pool logic that stores objects for reuse
**/

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool;
    private GameObject prefab;

    public void InitializePool(GameObject obj, int size)
    {
        // Intialize List and store reference to this pool's object
        pool = new List<GameObject>();
        prefab = obj;

        // Create Objects to store in Pool
        for (int i = 0; i < size; ++i)
        {
            GameObject thisObj = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
            thisObj.transform.SetParent(gameObject.transform);
            thisObj.SetActive(false);
            pool.Add(thisObj);
        }
    }

    public GameObject GetObject()
    {
        // Check if there is any objects in the pool
        if (pool.Count > 0)
        {
            // If there are objects in the pool, grab the first object and remove from pool
            GameObject obj = pool[0];
            pool.RemoveAt(0);
            //obj.transform.parent = null;
            return obj;
        }
        else
        {
            // If there are no objects in pool, create a new object to use
            // NOTE: This should not be used much, make sure to have a good reasonable initial size of pool to limit creating objects
            GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity) as GameObject;
            obj.transform.SetParent(gameObject.transform);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        // Retun object back to the pool and reset its properties
        pool.Add(obj);
        obj.transform.SetParent(gameObject.transform);
        obj.transform.position = transform.position;
        obj.SetActive(false);
    }

    public void ResetObjects()
    {
        // returns all the objects
        foreach (Transform child in transform)
        {
            ReturnObject(child.gameObject);
        }
    }
}
