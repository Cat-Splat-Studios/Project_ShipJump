/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: Holds the information of assets and handles the swaping of them
**/

using System.Collections.Generic;
using UnityEngine;

public class SwapManager : MonoSingleton<SwapManager>
{
    public static int Gears;
    public static int PlayerIdx;
    public static int BackgroundIdx;
    public static int MusicIdx;
    public static int ObstacleIdx;
    public static int ProjectileIdx;

    public static int EmergencyFuelCount;
    public static int DoubleShieldCount;

    public static List<int> PlayerUnlocks { get; set; } = new List<int>();

    public static List<int> BackgroundUnlocks { get; set; } = new List<int>();

    public static List<int> MusicUnlocks { get; set; } = new List<int>();

    public static List<int> ObstacleUnlocks { get; set; } = new List<int>();

    public static List<int> ProjectileUnlocks { get; set; } = new List<int>();

    [SerializeField]
    private BackgroundManager background;
    [SerializeField]
    private PlayerManager player;
    [SerializeField]
    private new AudioManager audio;

    public static List<int> allRockets = new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7};
    public static List<int> rocketPrices = new List<int> { 0, 500, 600, 800, 1000, 1500, 2000, 2500 };

    private void Start()
    {
        //Gears = 0;

        //PlayerIdx = 0;
        //BackgroundIdx = 0;
        //MusicIdx = 0;
        //BackgroundIdx = 0;

        //PlayerUnlocks = new List<int>();
        //BackgroundUnlocks = new List<int>();
        //MusicUnlocks = new List<int>();
        //ObstacleUnlocks = new List<int>();

        player = FindObjectOfType<PlayerManager>();
        background = FindObjectOfType<BackgroundManager>();

        player.InitUnlock();
    }

    public void AddDefaults()
    {
        PlayerUnlocks.Add(0);
        BackgroundUnlocks.Add(0);
        MusicUnlocks.Add(0);
        ObstacleUnlocks.Add(0);
    }

    public void PurchaseAsset(int idx, EAssetType type)
    {
        Debug.Log("happened");
        switch (type)
        {
            case EAssetType.ROCKET:
                PlayerUnlocks.Add(idx);
                break;
            case EAssetType.BACKGROUND:
                BackgroundUnlocks.Add(idx);
                break;
            case EAssetType.MUSIC:
                MusicUnlocks.Add(idx);
                break;
            case EAssetType.OBSTACLE:
                ObstacleUnlocks.Add(idx);
                break;
        }

        SaveManager.instance.SaveToCloud();
    }

    public void BackgroundSelect(int idx)
    {
        BackgroundIdx = idx;
        background.SwapIt();
    }

    public void PlayerSelect(int idx)
    {
        PlayerIdx = idx;
        player.SwapIt();
    }

    public void MusicSelect(int idx)
    {
        MusicIdx = idx;
        audio.SwapIt();
    }

    public void Preview(EAssetType type, int idx)
    {
        switch (type)
        {
            case EAssetType.BACKGROUND:
                background.Preview(idx);
                break;
            case EAssetType.MUSIC:
                audio.PreviewMusic(idx);
                break;
        }
    }

    public void PreviewReset()
    {

        background.SwapIt();
        audio.SwapIt();

    }
}
