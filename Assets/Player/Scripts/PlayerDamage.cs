using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    bool hasSheild = false;

    public void GotHit()
    {
        if (hasSheild)
        {
            // remove sheilds 
            hasSheild = false;
        }
        else
        {
            // Destroyed
        }
    }
}
