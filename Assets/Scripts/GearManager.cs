using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearManager : MonoBehaviour
{

    public static int totalGears { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GetGears();
    }

    public void GetGears()
    {
        // get gears from back end
        totalGears = 1000;
    }

    public static void AddGears(int amount)
    {
        totalGears += amount;
        Debug.Log($"Add {amount} of gears");
    }

    public static void RemoveGears(int amount)
    {
        totalGears -= amount;
    }

    public static bool CheckGears(int amount)
    {
        return true;
       // return totalGears >= amount;
    }

    private void SaveGears()
    {
        // save total gears to back end
    }
}
