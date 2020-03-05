/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To spawn in collectibles in there approriate pattern
**/

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
        float PosX = transform.position.x;
        int randMax = 6;

        // Check where random position in on x axis
        // eliminate patterns that would cause collectibles to go off screen
        if (PosX > 1.5f || PosX < -1.5f)
            randMax = randMax - 2;

        if ((coinsToSpawn == 3 && PosX < -0.75f) || (coinsToSpawn == 2 && PosX < -1.75f))
            randMax--;

        if ((coinsToSpawn == 3 && PosX > 0.75f) || (coinsToSpawn == 2 && PosX > 1.75f))
            randMax--;

        int rand = Random.Range(0, randMax);

        switch (rand)
        {
            case 0:
            case 1:
                for (int i = 0; i < coinsToSpawn; ++i)
                {
                    CheckSpawn(new Vector3(transform.position.x, transform.position.y + (yOffset * i), transform.position.z));
                }
                break;
            case 2:
                for (int i = 0; i < coinsToSpawn; ++i)
                {
                    CheckSpawn(new Vector3(transform.position.x + (xOffset * i), transform.position.y + (yOffset * i), transform.position.z));
                }
                break;
            case 3:
                for (int i = 0; i < coinsToSpawn; ++i)
                {
                    CheckSpawn(new Vector3(transform.position.x - (xOffset * i), transform.position.y + (yOffset * i), transform.position.z));
                }
                break;
            case 4:
                CheckSpawn(new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z));
                break;
            case 5:
                CheckSpawn(new Vector3(transform.position.x, transform.position.y, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x - 1.25f, transform.position.y, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x, transform.position.y - 1.25f, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x + 1.25f, transform.position.y, transform.position.z));
                break;
        }    

        Destroy(this.gameObject);
    }

    // makes sure you can spawn and object there

    private void CheckSpawn(Vector3 pos)
    {
        Collider[] hitcolliders = Physics.OverlapSphere(pos, 1.0f, LayerMask.GetMask("Spawners"));

        if(hitcolliders.Length > 0)
        {
            return;
        }

        GameObject gear = Pool.SpawnObject(poolName);
        gear.transform.position = pos;
    }

}
