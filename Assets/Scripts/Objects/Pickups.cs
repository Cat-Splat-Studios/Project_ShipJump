/** 
* Authors: Matthew Douglas, Hisham Ata
* Purpose: To populate pickups with the correct information
**/

using UnityEngine;

public enum EPickupType
{
    FUEL,
    SHIELD,
    BOOST,
    GEAR,
    PROJECTILE
}

public class Pickups : MonoBehaviour
{
    [SerializeField]
    private EPickupType curPickup;

    [SerializeField]
    private float fuelAmount = 10.0f;

    [SerializeField]
    private float boostSpeed;
  
    public void ChangeFuel(float amount)
    {
        fuelAmount = amount;
    }

    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetBoost()
    {
        return boostSpeed;
    }

    public EPickupType GetPickupType()
    {
        return curPickup;
    }
}
