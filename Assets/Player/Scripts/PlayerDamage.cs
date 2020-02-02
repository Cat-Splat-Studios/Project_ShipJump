using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public GameObject[] destroyParticlePrefab;
    public GameObject[] obstacleParticlePrefab;
    public AudioClip[] destroySounds;
    public AudioClip[] destoryObstacleSounds;
    public GameObject sheild;
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
            int randomParticle = Random.Range(0, obstacleParticlePrefab.Length);
            int randomSound = Random.Range(0, destoryObstacleSounds.Length);
            GameObject particleObj = Instantiate(obstacleParticlePrefab[randomParticle], new Vector3(obstacle.transform.position.x, obstacle.transform.position.y, -1.0f), Quaternion.identity) as GameObject;
            audio.PlaySound(destoryObstacleSounds[randomSound]);
            Destroy(particleObj, 1.5f);
            Destroy(obstacle);

            sheild.SetActive(false);
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
            sheild.SetActive(true);
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
        FindObjectOfType<UIDelgate>().GameOver();
        this.gameObject.SetActive(false);


        

        // show game over

    }

 
}
