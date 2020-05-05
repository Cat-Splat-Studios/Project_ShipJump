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
#pragma warning disable 0649
    [SerializeField]
    private EPickupType curPickup;

    [SerializeField]
    private string poolName;

    public EPickupType GetPickupType()
    {
        return curPickup;
    }

    public string GetPoolName()
    {
        return poolName;
    }
}
