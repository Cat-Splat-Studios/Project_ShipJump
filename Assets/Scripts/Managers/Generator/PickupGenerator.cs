using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGenerator : Generator
{
    string lastPickup = "";
    int lastFuelCount = 0;
    int lastPickupCount = 0;

    protected override void Spawn()
    {
        float randXPos = Random.Range(-xPosClamp, xPosClamp);

        Vector3 spawnPos = new Vector3(randXPos, transform.position.y, transform.position.z);

        int idx;

        // collision check on other objects (somtimes DOES NOT WORK)
        Collider[] hitcolliders = Physics.OverlapSphere(spawnPos, 1.0f, LayerMask.GetMask("Spawners"));

        if (hitcolliders.Length > 0)
        {
            didSkip = true;
            return;
        }

        if (poolNames.Length > 1)
        {
            if (lastFuelCount > 4)
            {
                idx = 0;
                lastFuelCount = 0;
            }               
            else
                idx = Random.Range(0, poolNames.Length);

                string type = poolNames[idx];

            if (type != "PickupFuel")
                lastFuelCount++;
            else
                lastFuelCount = 0;

            if(type == lastPickup)
            {
                if (lastPickupCount >= 2)
                {
                    didSkip = true;
                    return;
                }
                else
                {
                    lastPickupCount++;
                }                    
            }
            else
            {        
                lastPickup = type;
                lastPickupCount = 0;
                lastPickupCount++;
            }

            GameObject obj = Pool.SpawnObject(type);
            obj.transform.position = spawnPos;
        }
        else if (poolNames.Length == 1)
        {
            idx = 0;
            GameObject obj = Pool.SpawnObject(poolNames[idx]);
            obj.transform.position = spawnPos;
        }
        

        didSkip = false;
    }
}
