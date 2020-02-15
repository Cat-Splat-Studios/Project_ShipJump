using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class SwapManager : MonoBehaviour
{
    public static int PlayerIdx;
    public static int BackgroundIdx;
    public static int MusicIdx;
    public static int ObstacleIdx;
    public static int ProjectileIdx;

    public static List<int> PlayerUnlocks { get; private set; }

    public static List<int> BackgroundUnlocks { get; private set; }

    public static List<int> MusicUnlocks { get; private set; }

    public static List<int> ObstacleUnlocks { get; private set; }

    public static List<int> ProjectileUnlocks { get; private set; }

    private void Awake()
    {
        GetSwapInfo();
    }

    public static void GetSwapInfo()
    {
        GetCurrentUnlocks();
        GetCurrentIndexes();
    }

    private static void GetCurrentIndexes()
    {
        // get all current assets player has in there preferences
        PlayerIdx = 0;
        BackgroundIdx = 0;
        MusicIdx = 0;
        ObstacleIdx = 0;
        ProjectileIdx = 0;
    }

    private static void GetCurrentUnlocks()
    {
        // store the array of unlocks for each

        // initialize
        PlayerUnlocks = new List<int>();
        BackgroundUnlocks = new List<int>();
        MusicUnlocks = new List<int>();
        ObstacleUnlocks = new List<int>();
        ProjectileUnlocks = new List<int>();

        // Set with database


        // add default skins 0
        PlayerUnlocks.Add(0);
        BackgroundUnlocks.Add(0);
        MusicUnlocks.Add(0);
        ObstacleUnlocks.Add(0);
        ProjectileUnlocks.Add(0);

        //testing
        PlayerUnlocks.Add(1);
        PlayerUnlocks.Add(2);
        PlayerUnlocks.Add(3);
        PlayerUnlocks.Add(4);
        PlayerUnlocks.Add(5);
        PlayerUnlocks.Add(6);
        PlayerUnlocks.Add(7);
    }
}
