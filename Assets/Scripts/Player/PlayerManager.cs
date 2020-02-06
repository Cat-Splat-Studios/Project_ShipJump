/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: A central hub for all of the player scripts
**/

using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerShoot shoot;
    private PlayerCollision collision;
    private PlayerDamage damage;


    // Start is called before the first frame update
    void Start()
    {
        // Find References
        movement = GetComponent<PlayerMovement>();
        shoot = GetComponent<PlayerShoot>();
        collision = GetComponent<PlayerCollision>();
        damage = GetComponent<PlayerDamage>();
    }

    // Movment
    public PlayerMovement PlayerMovement()
    {
        return movement;
    }

    public PlayerShoot PlayerShoot()
    {
        return shoot;
    }

    public PlayerCollision PlayerCollision()
    {
        return collision;
    }

    public PlayerDamage PlayerDamage()
    {
        return damage;
    }
}
