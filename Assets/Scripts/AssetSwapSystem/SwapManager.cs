/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: Holds the information of assets and handles the swaping of them
**/

using System.Collections.Generic;
using UnityEngine;

public class SwapManager : MonoSingleton<SwapManager>
{
    // local references to player state information
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

    public static List<int> allRockets = new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,};
    public static List<int> rocketPrices = new List<int> { 0, 800, 1000, 1200, 1500, 2000, 3000, 5000, 3000, 5000 };

    private void Start()
    {
        // find references
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

    // After buying an item
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

    public void SwapInit()
    {
        // set music and background to currently selected
        BackgroundSelect(BackgroundIdx);
        MusicSelect(MusicIdx);
    }

    // ** Item selection logic **/
    public void BackgroundSelect(int idx)
    {
        BackgroundIdx = idx;
        PlayerPrefs.SetInt("backgroundIdx", idx);
        background.SwapIt();
    }

    public void PlayerSelect(int idx)
    {
        PlayerIdx = idx;
        PlayerPrefs.SetInt("playerIdx", idx);
        player.SwapIt();
    }

    public void MusicSelect(int idx)
    {
        MusicIdx = idx;
        PlayerPrefs.SetInt("musicIdx", idx);
        audio.SwapIt();
    }

    public void ObstacleSelect(int idx)
    {
        ObstacleIdx = idx;
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
