﻿/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the player damage logic
**/

using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    // Destroy Effects
    [Header("Destroy Effects")]
    [SerializeField]
    private GameObject[] destroyParticlePrefab;
    [SerializeField]
    private AudioClip[] destroySounds;

    // References
    [SerializeField]
    private GameObject sheild;

    private new AudioManager audio;
    private UIDelgate ui;
    private PlayerMovement playerMovement;

    // Helper Variables
    private bool hasSheild = false;

    private void Start()
    {
        // Find References
        audio = FindObjectOfType<AudioManager>();
        ui = FindObjectOfType<UIDelgate>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void GotHit(GameObject obj)
    {
        if (hasSheild)
        {
            // Remove sheilds 
            hasSheild = false;
            sheild.SetActive(false);

            // Destroy Obstacle
            Obstacle obstacle = obj.GetComponent<Obstacle>();
            if(obstacle)
            {
                obstacle.DestroyObstacle();              
            }    
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
        playerMovement.StopMovement();

        // Will remove this when GENERATOR system is completed
        FindObjectOfType<ObjectSpawner>().isPlaying = false;

        // Find random sound and particle to play
        int randomParticle = Random.Range(0, destroyParticlePrefab.Length);
        int randomSound = Random.Range(0, destroySounds.Length);

        audio.PlaySound(destroySounds[randomSound]);

        GameObject particleObj = Instantiate(destroyParticlePrefab[randomParticle],
                                new Vector3(transform.position.x, transform.position.y, -1.0f), Quaternion.identity) as GameObject;
        Destroy(particleObj, 1.5f);

        // Game over and turn off player (this is so we can reset on replay)
        ui.GameOver();
        this.gameObject.SetActive(false);
    }
 
}