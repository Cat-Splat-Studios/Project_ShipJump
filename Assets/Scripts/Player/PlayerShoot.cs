/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the player shoot logic
**/

using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public UIDelgate ui;

    [Header("Projectile Info")]
    public GameObject projectilePrefab;

    [Header("Audio")]
    public new AudioManager audio;
    public AudioClip[] fireSounds;

    private bool canShoot;

    public void Shoot()
    {
        // Shoot projectile if able to
        if(canShoot)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            foreach(AudioClip sound in fireSounds)
            {
                audio.PlaySound(sound);
            }
            
            TurnOff();
        }
    }

    public void EnableShoot()
    {
        // Prompts shoot button and enables shoot
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
