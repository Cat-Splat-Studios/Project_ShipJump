/** THIS CODE WILL BE REMOVED AFTER GENERATOR SYSTEM IS COMPLETE**/
// Matthew Douglas

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectSpanws
{
    public GameObject prefab;
    public int instances;
}

public class ObjectSpawner : MonoBehaviour
{

    public PlayerMovement player;

    public ObjectSpanws[] itemSpawns;

    public GameObject coinSpawnerPrefab;
    public GameObject rocketPrefab;
    public GameObject asteroidPrefab;

    public float minSpawn;
    public float maxSpawn;

    public bool isPlaying = false;
    public bool isFalling = false;

    private float timeTillSpawn;
    private float timeSpawn;

    private float timeTillCoin = 0.0f;

    private float coinTime = 0.0f;

    private List<GameObject> prefasAvaiable;

    private List<GameObject> objectsInWork;
    private List<GameObject> coinsInWork;

    // Start is called before the first frame update
    void Start()
    {
        SetSpawnIntervals();
        objectsInWork = new List<GameObject>();
        prefasAvaiable = new List<GameObject>();
        coinsInWork = new List<GameObject>();

        foreach (var item in itemSpawns)
        {
            for (int i = 0; i < item.instances; ++i)
            {
                prefasAvaiable.Add(item.prefab);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player)
        {
            transform.position = new Vector3(0.0f, player.gameObject.transform.position.y + 15.0f, 0.0f);

            if (isPlaying)
            {


                timeSpawn += Time.deltaTime;

                if (timeSpawn >= timeTillSpawn)
                {
                    Spawn();

                    SetSpawnIntervals();
                }


                if(!isFalling)
                {
                    coinTime += Time.deltaTime;

                    if (coinTime >= timeTillCoin)
                    {
                        SpawnCoinSpawner();

                        SetSpawnCoinIntervals();
                    }
                }
  
            }
        }     
    }

    public void ResetSpawn()
    {
        foreach (var item in objectsInWork)
        {
            if(item)
            {
                Destroy(item);
            }
        }

        foreach (var item in coinsInWork)
        {
            if (item)
            {
                Destroy(item);
            }
        }

        coinsInWork.Clear();
        objectsInWork.Clear();
    }   

    private void Spawn()
    {
        float randXPos = Random.Range(-player.xClamp, player.xClamp);
        if (!isFalling)
        {
            
            int rand = Random.Range(0, prefasAvaiable.Count);

            GameObject prefab = prefasAvaiable[rand];

            GameObject obj = Instantiate(prefab, new Vector3(randXPos, transform.position.y, transform.position.x), Quaternion.identity) as GameObject;

            objectsInWork.Add(obj);
        }
        else
        {
            GameObject prefab;

            int rand = Random.Range(0, 3);

            if (rand == 0)
            {
                prefab = rocketPrefab;
            }
            else
            {
                prefab = asteroidPrefab;
            }

            GameObject obj = Instantiate(prefab, new Vector3(randXPos, transform.position.y - 50.0f, transform.position.x), Quaternion.identity) as GameObject;

            if(prefab == rocketPrefab)
                obj.GetComponent<Pickups>().ChangeFuel(50.0f);

            objectsInWork.Add(obj);
        }
        

        //obj.transform.rotation = new Vector3(obj.transform.rotation.x, 180.0f, obj.transform.rotation.z);
    }

    private void SpawnCoinSpawner()
    {
        
        if (!isFalling)
        {

            float randXPos = Random.Range(-player.xClamp, player.xClamp);

            GameObject obj = Instantiate(coinSpawnerPrefab, new Vector3(randXPos, transform.position.y, transform.position.x), Quaternion.identity) as GameObject;
        }
      
    }

    private void SetSpawnIntervals()
    {
        if(!isFalling)
        {
            timeTillSpawn = Random.Range(minSpawn, maxSpawn);
        }
        else
        {
            timeTillSpawn = Random.Range(0.5f, 0.8f);
        }
        
        timeSpawn = 0.0f;
    }

    private void SetSpawnCoinIntervals()
    {
        if(!isFalling)
        {
            timeTillCoin = Random.Range(0.8f, 1.2f);
            coinTime = 0.0f;
        }
    }

    public void ClearObjects ()
    {
        int buffer = objectsInWork.Count > 8 ? 8 : 0;

        for(int i = 0; i < objectsInWork.Count - buffer; ++i)
        {
            GameObject item = objectsInWork[i];
            objectsInWork.RemoveAt(i);

            Destroy(item);
        }
    }

    public void AddCoinToList(GameObject coins)
    {
        coinsInWork.Add(coins);
    }

}
