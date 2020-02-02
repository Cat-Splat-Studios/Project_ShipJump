using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public GameObject shieldText;
    public GameObject[] destroyParticlePrefab;
    public GameObject[] obstacleParticlePrefab;
    private bool hasSheild = false;

    public void GotHit(GameObject obstacle)
    {
        if (hasSheild)
        {
            // remove sheilds 
            hasSheild = false;
            shieldText.SetActive(false);
            int random = Random.Range(0, obstacleParticlePrefab.Length);
            GameObject particleObj = Instantiate(obstacleParticlePrefab[random], new Vector3(obstacle.transform.position.x, obstacle.transform.position.y, -1.0f), Quaternion.identity) as GameObject;
            Destroy(particleObj, 1.5f);
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
        FindObjectOfType<ObjectSpawner>().isPlaying = false;

        int random = Random.Range(0, destroyParticlePrefab.Length);

        GameObject particleObj = Instantiate(destroyParticlePrefab[random], new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity) as GameObject;
        Destroy(particleObj, 1.5f);

        this.gameObject.SetActive(false);

        // show game over

    }
}
