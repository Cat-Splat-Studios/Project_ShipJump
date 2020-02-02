using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public int coinsToSpawn = 0;
    public float yOffset = 0.5f;
    public GameObject coinPrefab;

    private ObjectSpawner objSpawner;

    // Start is called before the first frame update
    void Start()
    {
        objSpawner = FindObjectOfType<ObjectSpawner>();

        coinsToSpawn = Random.Range(1, 4);

        for (int i = 0; i < coinsToSpawn; ++i)
        {
            GameObject coingObj = Instantiate(coinPrefab, new Vector3(transform.position.x, transform.position.y + (yOffset * i), transform.position.z), Quaternion.identity) as GameObject;
            objSpawner.AddCoinToList(coingObj);
        }

        Destroy(this.gameObject);
    }

}
