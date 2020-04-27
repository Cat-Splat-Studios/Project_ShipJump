/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To reference different rocket stats to use for the player
**/

using UnityEngine;

// Rocket Size
public enum ERocketSize
{
    SMALL,
    LARGE
}

public class Rocket : MonoBehaviour
{
    public int unlockIdx;

    [SerializeField]
    private ERocketSize size;

    [SerializeField]
    private float fuelBurn;

    [SerializeField]
    private float topSpeed;

    public ERocketSize GetSize()
    {
        return size;
    }

    public float GetFuelBurn()
    {
        return fuelBurn;
    }

    public float GetTopSpeed()
    {
        return topSpeed;
    }

    public int GetShieldStack()
    {
        if (size == ERocketSize.LARGE)
            return 2;
        else
            return 1;
    }

    public float GetFuelIntake()
    {
        if (size == ERocketSize.LARGE)
            return 1.0f;
        else
            return 1.5f;
    }
}
