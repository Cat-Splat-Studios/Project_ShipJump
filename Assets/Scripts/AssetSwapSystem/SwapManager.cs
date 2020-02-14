using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class SwapManager : MonoBehaviour
{
    public static int PlayerIdx { get; private set; }
    public static int BackgroundIdx { get; private set; }
    public static int MusicIdx { get; private set; }
    public static int ObstacleIdx { get; private set; }
    public static int ProjectileIdx { get; private set; }

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
        PlayerIdx = 4;
        BackgroundIdx = 0;
        MusicIdx = 0;
        ObstacleIdx = 1;
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
    }
}
