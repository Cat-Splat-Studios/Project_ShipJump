/** THIS CODE WILL BE REMOVED AFTER GENERATOR SYSTEM IS COMPLETE**/
// Matthew Douglas

using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public int coinsToSpawn = 0;
    public float yOffset = 0.2f;
    public float xOffset = 1.0f;
    public GameObject coinPrefab;

    

    private string poolName = "PickupGears";

    // Start is called before the first frame update
    void Start()
    {
        coinsToSpawn = Random.Range(0, 4);

        int rand = Random.Range(0, 5);

        switch (rand)
        {
            case 0:
            case 1:
                for (int i = 0; i < coinsToSpawn; ++i)
                {
                    GameObject gear = Pool.SpawnObject(poolName);
                    gear.transform.position = new Vector3(transform.position.x, transform.position.y + (yOffset * i), transform.position.z);
                }
                break;
            case 2:
                for (int i = 0; i < coinsToSpawn; ++i)
                {
                    GameObject gear = Pool.SpawnObject(poolName);
                    gear.transform.position = new Vector3(transform.position.x + (xOffset * i), transform.position.y + (yOffset * i), transform.position.z);
                }
                break;
            case 3:
                for (int i = 0; i < coinsToSpawn; ++i)
                {
                    GameObject gear = Pool.SpawnObject(poolName);
                    gear.transform.position = new Vector3(transform.position.x - (xOffset * i), transform.position.y + (yOffset * i), transform.position.z);
                }
                break;
            case 4:
               
                    GameObject gear1 = Pool.SpawnObject(poolName);
                    GameObject gear2 = Pool.SpawnObject(poolName);
                    GameObject gear3 = Pool.SpawnObject(poolName);
                    gear1.transform.position = new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z);
                    gear2.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                    gear3.transform.position = new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z);


                break;
        }

       

        Destroy(this.gameObject);
    }

}
