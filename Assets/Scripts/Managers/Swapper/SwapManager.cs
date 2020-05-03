/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: Holds the information of assets and handles the swaping of them
**/

using System.Collections.Generic;
using UnityEngine;

// This is some morphed type class with static and object functionality. Maybe a better way to clean this up or split it up.

public class SwapManager : MonoSingleton<SwapManager>
{
    // local references to player state information
    public static int PlayerIdx;
    public static int BackgroundIdx;
    public static int MusicIdx;
    public static int ObstacleIdx;
    public static int ProjectileIdx;

    public static int EmergencyFuelCount;
    public static int DoubleShieldCount;

    public static List<int> PlayerUnlocks;
    public static List<int> BackgroundUnlocks;
    public static List<int> MusicUnlocks;
    public static List<int> ObstacleUnlocks;

    [SerializeField]
    private BackgroundManager background;
    [SerializeField]
    private PlayerManager player;
    [SerializeField]
    private new AudioManager audio;

    public static List<int> allRockets = new List<int>{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
    public static List<int> rocketPrices = new List<int> { 0, 0, 1500, 1500, 3000, 3000, 3500, 3500, 5000, 5000, 8000, 8000, 10000, 10000 };


    private void Start()
    {
        // find references
        player = FindObjectOfType<PlayerManager>();
        background = FindObjectOfType<BackgroundManager>();
    }

    public static void AddDefaults()
    {
        ListDefaultCheck(PlayerUnlocks, true);
        ListDefaultCheck(BackgroundUnlocks);
        ListDefaultCheck(MusicUnlocks);
        ListDefaultCheck(ObstacleUnlocks);
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

        CloudSaving.instance.SaveGame();
       // SaveManager.instance.SaveToCloud();
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

    private static void ListDefaultCheck(List<int> listToCheck, bool isPlayer = false)
    {
        if (listToCheck == null)
            listToCheck = new List<int>();

        if(!listToCheck.Contains(0))
            listToCheck.Add(0);

        if (isPlayer && !listToCheck.Contains(2))
            listToCheck.Add(2);
    }
}
