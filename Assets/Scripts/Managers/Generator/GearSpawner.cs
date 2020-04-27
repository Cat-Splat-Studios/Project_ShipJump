/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To spawn in collectibles in their approriate patterns
**/

using UnityEngine;

public class GearSpawner : MonoBehaviour
{
    [SerializeField]
    private int gearsToSpawn = 0;

    // Offsets to spawn gears from on each axis when making patterns
    [SerializeField]
    private float yOffset = 0.2f;
    [SerializeField]
    private float xOffset = 1.0f;

    private string poolName = "PickupGears";

    void Start()
    {
        // Randomize number of gears to spawn and get the position of spawning area
        gearsToSpawn = Random.Range(0, 4);
        float PosX = transform.position.x;

        // Number of possible patterns
        int randMax = 6;

        // CRITERIA MATCHING
        // Check where random position in on x axis
        // Eliminate patterns that would cause collectibles to go off screen 
        if (PosX > 1.5f || PosX < -1.5f)
            randMax = randMax - 2;

        if ((gearsToSpawn == 3 && PosX < -0.75f) || (gearsToSpawn == 2 && PosX < -1.75f))
            randMax--;

        if ((gearsToSpawn == 3 && PosX > 0.75f) || (gearsToSpawn == 2 && PosX > 1.75f))
            randMax--;

        int rand = Random.Range(0, randMax);

        switch (rand)
        {
            case 0:
            case 1:
                // Straight Line Pattern
                for (int i = 0; i < gearsToSpawn; ++i)
                {
                    CheckSpawn(new Vector3(transform.position.x, transform.position.y + (yOffset * i), transform.position.z));
                }
                break;
            case 2:
                // Diagonal Right Pattern
                for (int i = 0; i < gearsToSpawn; ++i)
                {
                    CheckSpawn(new Vector3(transform.position.x + (xOffset * i), transform.position.y + (yOffset * i), transform.position.z));
                }
                break;
            case 3:
                // Diagonal Left Pattern
                for (int i = 0; i < gearsToSpawn; ++i)
                {
                    CheckSpawn(new Vector3(transform.position.x - (xOffset * i), transform.position.y + (yOffset * i), transform.position.z));
                }
                break;
            case 4:
                // Triangle Pattern
                CheckSpawn(new Vector3(transform.position.x + 1.0f, transform.position.y, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x - 1.0f, transform.position.y, transform.position.z));
                break;
            case 5:
                // Star Pattern
                CheckSpawn(new Vector3(transform.position.x, transform.position.y, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x - 1.25f, transform.position.y, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x, transform.position.y - 1.25f, transform.position.z));
                CheckSpawn(new Vector3(transform.position.x + 1.25f, transform.position.y, transform.position.z));
                break;
        }    

        Destroy(this.gameObject);
    }

    // Helper function that checks to make sure area does not conflict with any other objects
    // Works 95% of the time
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
