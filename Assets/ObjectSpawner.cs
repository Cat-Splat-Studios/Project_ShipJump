using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public PlayerMovement player;

    public GameObject[] objectsToSpawn;

    public float minSpawn;
    public float maxSpawn;

    public bool isPlaying = false;
    public bool isFalling = false;

    private float timeTillSpawn;
    private float timeSpawn;

    private List<GameObject> objects;

    // Start is called before the first frame update
    void Start()
    {
        SetSpawnIntervals();
        objects = new List<GameObject>();
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
            }
        }     
    }

    public void ResetSpawn()
    {
        foreach (var item in objects)
        {
            if(item)
            {
                Destroy(item);
            }
        }

        objects.Clear();
    }   

    private void Spawn()
    {
        float randXPos = Random.Range(-player.xClamp, player.xClamp);
        if (!isFalling)
        {
            
            int rand = Random.Range(0, objectsToSpawn.Length);

            GameObject prefab = objectsToSpawn[rand];

            GameObject obj = Instantiate(prefab, new Vector3(randXPos, transform.position.y, transform.position.x), Quaternion.identity) as GameObject;

            objects.Add(obj);
        }
        else
        {
            GameObject prefab = objectsToSpawn[6];

            GameObject obj = Instantiate(prefab, new Vector3(randXPos, -transform.position.y, transform.position.x), Quaternion.identity) as GameObject;

            objects.Add(obj);
        }
        

        //obj.transform.rotation = new Vector3(obj.transform.rotation.x, 180.0f, obj.transform.rotation.z);
    }

    private void SetSpawnIntervals()
    {
        if(!isFalling)
        {
            timeTillSpawn = Random.Range(minSpawn, maxSpawn);
        }
        else
        {
            timeTillSpawn = Random.Range(1.0f, 2.0f);
        }
        
        timeSpawn = 0.0f;
    }
}
