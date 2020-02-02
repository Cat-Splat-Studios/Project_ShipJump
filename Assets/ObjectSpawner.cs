using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public PlayerMovement player;

    public GameObject[] objectsToSpawn;

    public float minSpawn;
    public float maxSpawn;

    public bool isPlaying = true;

    private float timeTillSpawn;
    private float timeSpawn;
    // Start is called before the first frame update
    void Start()
    {
        SetSpawnIntervals();
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

    private void Spawn()
    {
        float randXPos = Random.Range(-player.xClamp, player.xClamp);
        int rand = Random.Range(0, objectsToSpawn.Length);

        GameObject prefab = objectsToSpawn[rand];

        GameObject obj = Instantiate(prefab, new Vector3(randXPos, transform.position.y, transform.position.x), Quaternion.identity) as GameObject;

        //obj.transform.rotation = new Vector3(obj.transform.rotation.x, 180.0f, obj.transform.rotation.z);
    }

    private void SetSpawnIntervals()
    {
        timeTillSpawn = Random.Range(minSpawn, maxSpawn);
        timeSpawn = 0.0f;
    }
}
