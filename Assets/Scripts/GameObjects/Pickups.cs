/** 
* Authors: Matthew Douglas, Hisham Ata
* Purpose: To populate pickups with the correct information
**/

using UnityEngine;

// Different pickup types in game
public enum EPickupType
{
    FUEL,
    SHIELD,
    BOOST,
    GEAR,
    PROJECTILE
}

public class Pickups : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private EPickupType curPickup;

    // FOR FUEL PICKUPS ONLY - Amount of fuel player receives
    // Need this because fuel pickups when going backwards give more fuel
    // TODO - put this logic into a player script 
    [SerializeField]
    private float fuelAmount = 10.0f;

    [SerializeField]
    private string poolName;


    public EPickupType GetPickupType()
    {
        return curPickup;
    }

    public float GetFuel()
    {
        return fuelAmount;
    }

    public string GetPoolName()
    {
        return poolName;
    }
}
