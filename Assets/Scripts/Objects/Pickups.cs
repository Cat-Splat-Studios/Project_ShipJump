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

public class Pickups : MonoBehaviour, IPoolObject
{
    [SerializeField]
    private EPickupType curPickup;

    [SerializeField]
    private float fuelAmount = 10.0f;

    [SerializeField]
    private float boostSpeed;

    [SerializeField]
    private string poolName;


    [SerializeField]
    private float pullSpeed;

    private bool isPull = false;

    GameObject playerTarget;

    private void Update()
    {
        if(isPull)
        {
            // Calculate direction vector
            Vector3 dir = playerTarget.transform.position - transform.position;
            // Normalize resultant vector to unit Vector
            dir = dir.normalized;
            // Move in the direction of the direction vector every frame 
            transform.position += dir * Time.deltaTime * pullSpeed;
        }
    }

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

    public string GetPoolName()
    {
        return poolName;
    }

    public void Pull(GameObject player)
    {
        if (curPickup == EPickupType.GEAR)
        {
            isPull = true;
            playerTarget = player;
        }
    }

    public void StopPull()
    {
        isPull = false;
    }

}
