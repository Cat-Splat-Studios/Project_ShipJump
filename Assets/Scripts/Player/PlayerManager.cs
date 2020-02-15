/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: A central hub for all of the player scripts
**/

using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISwapper
{
    [SerializeField]
    private GameObject[] rockets;

    private PlayerMovement movement;
    private PlayerShoot shoot;
    private PlayerCollision collision;
    private PlayerDamage damage;

    private int unlockIdx;

    // Start is called before the first frame update
    void Start()
    {
        // Find References
        movement = GetComponent<PlayerMovement>();
        shoot = GetComponent<PlayerShoot>();
        collision = GetComponent<PlayerCollision>();
        damage = GetComponent<PlayerDamage>();

        SwapIt();

        unlockIdx = SwapManager.PlayerUnlocks.IndexOf(SwapManager.PlayerIdx);
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
    
    public void SetBoost(bool value)
    {
        rockets[SwapManager.PlayerIdx].GetComponent<Thrusters>().BoostToggle(value);
    }

    public void SwapIt()
    {
        foreach(GameObject rocket in rockets)
        {
            rocket.SetActive(false);
        }

        rockets[SwapManager.PlayerIdx].SetActive(true);
    }

    public void ToggleRocket(bool forward)
    {
        if (forward)
        {
            if (unlockIdx + 1 >= SwapManager.PlayerUnlocks.Count)
            {
                unlockIdx = 0;
            }
            else
            {
                ++unlockIdx;
            }
        }
        else
        {
            if (unlockIdx - 1 < 0)
            {
                unlockIdx = SwapManager.PlayerUnlocks.Count - 1;
            }
            else
            {
                --unlockIdx;
            }
        }

        SwapManager.PlayerIdx = SwapManager.PlayerUnlocks[unlockIdx];

        SwapIt();
    }
}
