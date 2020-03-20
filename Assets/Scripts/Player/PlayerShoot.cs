﻿/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the player shoot logic
**/

using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    // Shooting
    public bool canShoot;
    public GameObject projectilePrefab;
    public GameObject projectileSpawn;

    public new AudioManager audio;
    public AudioClip fireSound;

    private UIDelgate ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIDelgate>();
    }
    public void Shoot()
    {
        if(canShoot)
        {
            Instantiate(projectilePrefab, projectileSpawn.transform.position, Quaternion.identity);
            audio.PlaySound(fireSound);
            TurnOff();
        }
    }

    public void EnableShoot()
    {
        if (!canShoot)
        {
            canShoot = true;
        }
        ui.ToggleShoot(true);
    }

    public void TurnOff()
    {
        canShoot = false;
        ui.ToggleShoot(false);
    }
}
