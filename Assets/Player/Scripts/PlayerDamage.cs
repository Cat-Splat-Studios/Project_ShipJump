using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public GameObject shieldText;
    public GameObject[] destroyParticlePrefab;
    public GameObject[] obstacleParticlePrefab;
    public AudioClip[] destroySounds;
    public AudioClip[] destoryObstacleSounds;
    private bool hasSheild = false;

    private AudioManager audio;

    private void Start()
    {
        audio = FindObjectOfType<AudioManager>();
    }

    public void GotHit(GameObject obstacle)
    {
        if (hasSheild)
        {
            // remove sheilds 
            hasSheild = false;
            shieldText.SetActive(false);
            int randomParticle = Random.Range(0, obstacleParticlePrefab.Length);
            int randomSound = Random.Range(0, destoryObstacleSounds.Length);
            GameObject particleObj = Instantiate(obstacleParticlePrefab[randomParticle], new Vector3(obstacle.transform.position.x, obstacle.transform.position.y, -1.0f), Quaternion.identity) as GameObject;
            audio.PlaySound(destoryObstacleSounds[randomSound]);
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

        int randomParticle = Random.Range(0, destroyParticlePrefab.Length);
        int randomSound = Random.Range(0, destroySounds.Length);

        audio.PlaySound(destroySounds[randomSound]);

        GameObject particleObj = Instantiate(destroyParticlePrefab[randomParticle], new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity) as GameObject;
        Destroy(particleObj, 1.5f);

        this.gameObject.SetActive(false);

        FindObjectOfType<UIDelgate>().GameOver();

        // show game over

    }
}
