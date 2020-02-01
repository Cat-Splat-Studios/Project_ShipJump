using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    private bool hasSheild = false;

    public void GotHit(GameObject obstacle)
    {
        if (hasSheild)
        {
            // remove sheilds 
            hasSheild = false;
            Destroy(obstacle);
        }
        else
        {
            // Destroyed
            DestroyShip();
        }
    }

    public void AttatchSheild()
    {
        if (!hasSheild)
        {

            hasSheild = true;
        }
    }

    private void DestroyShip()
    {
        GetComponent<PlayerMovement>().StopMovement();

    }
}
