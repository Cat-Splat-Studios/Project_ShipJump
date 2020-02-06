/** 
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

    private UIDelgate ui;

    // Start is called before the first frame update
    void Start()
    {
        ui = FindObjectOfType<UIDelgate>();
    }
    public void Shoot()
    {
        Instantiate(projectilePrefab, projectileSpawn.transform);
        canShoot = false;
        ui.ToggleShoot(false);

    }

    public void EnableShoot()
    {
        if (!canShoot)
        {
            canShoot = true;
            ui.ToggleShoot(true);
        }

    }
}
