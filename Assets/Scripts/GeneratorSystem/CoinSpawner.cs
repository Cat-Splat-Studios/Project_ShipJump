/** THIS CODE WILL BE REMOVED AFTER GENERATOR SYSTEM IS COMPLETE**/
// Matthew Douglas

using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public int coinsToSpawn = 0;
    public float yOffset = 0.5f;
    public GameObject coinPrefab;

    private string poolName = "PickupGears";

    // Start is called before the first frame update
    void Start()
    {
        coinsToSpawn = Random.Range(1, 4);

        for (int i = 0; i < coinsToSpawn; ++i)
        {
            GameObject gear = Pool.SpawnObject(poolName);
            gear.transform.position = new Vector3(transform.position.x, transform.position.y + (yOffset * i), transform.position.z);
        }

        Destroy(this.gameObject);
    }

}
