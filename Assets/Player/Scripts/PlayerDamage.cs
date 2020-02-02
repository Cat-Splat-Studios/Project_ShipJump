using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public GameObject shieldText;
    private bool hasSheild = false;

    public void GotHit(GameObject obstacle)
    {
        if (hasSheild)
        {
            // remove sheilds 
            hasSheild = false;
            shieldText.SetActive(false);
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
            shieldText.SetActive(true);
            hasSheild = true;
        }
    }

    private void DestroyShip()
    {
        GetComponent<PlayerMovement>().StopMovement();
        Destroy(this.gameObject);

    }
}
