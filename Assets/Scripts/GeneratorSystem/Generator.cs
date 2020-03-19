/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To Generate specific objects in the game
**/

using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private string[] poolNames;
    [SerializeField]
    private GameObject gearSpawnPrefab;

    [Header("Spawn Intervals")]
    [SerializeField]
    private bool isRandom;
    [SerializeField]
    private float minInterval;
    [SerializeField]
    private float maxInterval;

    private float xPosClamp;

    // time
    private float timeTillSpawn;
    private float timeSpawn;

    // if object spawn was skipped last time
    private bool didSkip = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        SetSpawnIntervals();
    }

    // Update is called once per frame
    void Update()
    {
        timeSpawn += Time.deltaTime;

        if (timeSpawn >= timeTillSpawn)
        {
            Spawn();

            SetSpawnIntervals();
        }
    }

    public void SetClamp(float clamp)
    {
        xPosClamp = clamp;
    }

    private void SetSpawnIntervals()
    {

        if (!didSkip)
            timeTillSpawn = Random.Range(minInterval, maxInterval);
        else
            timeTillSpawn = 0.5f;
        timeSpawn = 0.0f;
    }

    private void Spawn()
    {
        float randXPos = Random.Range(-xPosClamp, xPosClamp);

        Vector3 spawnPos = new Vector3(randXPos, transform.position.y, transform.position.z);

        int idx;

        // collision check on other objects (somtimes DOES NOT WORK)
        Collider[] hitcolliders = Physics.OverlapSphere(spawnPos, 1.0f, LayerMask.GetMask("Spawners"));

        if(hitcolliders.Length > 0)
        {
            didSkip = true;
            return;
        }

        if (poolNames.Length > 1)
        {
            idx = Random.Range(0, poolNames.Length);
            GameObject obj = Pool.SpawnObject(poolNames[idx]);
            obj.transform.position = spawnPos;
        }
        else if (poolNames.Length == 1)
        {
            idx = 0;
            GameObject obj = Pool.SpawnObject(poolNames[idx]);
            obj.transform.position = spawnPos;
        }
        else
        {
            Instantiate(gearSpawnPrefab, spawnPos, Quaternion.identity);
        }

        didSkip = false;
  
    }
}
