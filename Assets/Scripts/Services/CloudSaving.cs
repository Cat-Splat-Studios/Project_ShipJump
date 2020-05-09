/** 
* Author: Hisham Ata, Matthew Douglas
* Purpose: To Handle the cloud saving and loading process utilizing the Voxel Native Plugin
**/

using System;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;

public class CloudSaving : MonoSingleton<CloudSaving>
{
    string GearsKey = "curGears";
    string PlayerIdxKey = "curPlayerIDX";
    string BackgroundIdxKey = "curBackgroundIDX";
    string MusicIdxKey = "curMusicIDX";
    string ObstacleIdxKey = "curObstacleIDX";
    string ProjectileIdxKey = "curProjectileIDX";

    string HighscoreKey = "curHighscore";
    string HighDistanceKey = "curHighDistance";

    string PlayerUnlocksKey = "curPlayerUnlocks";
    string BackgroundUnlocksKey = "curBackgroundUnlocks";
    string MusicUnlocksKey = "curMusicUnlocks";
    string ObstacleUnlocksKey = "curObstacleUnlocks";

    public PlayerManager player;

    public MessageBox prompt;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeCloud();
    }

    // Register for the event
    void OnEnable()
    {
        CloudServices.KeyValueStoreDidInitialiseEvent += OnKeyValueStoreInitialised;
        CloudServices.KeyValueStoreDidSynchroniseEvent += OnKeyValueStoreDidSynchronise;
        CloudServices.KeyValueStoreDidChangeExternallyEvent += OnKeyValueStoreChanged;
    }

    void OnDisable()
    {
        CloudServices.KeyValueStoreDidInitialiseEvent -= OnKeyValueStoreInitialised;
        CloudServices.KeyValueStoreDidSynchroniseEvent -= OnKeyValueStoreDidSynchronise;
        CloudServices.KeyValueStoreDidChangeExternallyEvent -= OnKeyValueStoreChanged;
    }

    private void OnKeyValueStoreInitialised(bool _success)
    {
        if (_success)
        {
           // prompt.SetPrompt("Cloud Save Initialized", "Successfully initialized in-memory keys and values.");
            Debug.Log("Successfully synchronised in-memory keys and values.");
        }
        else
        {
            prompt.SetPrompt("Failed to Load", "Could not retreive you data, press okay to try again or restart the game.", reinit);
            Debug.Log("Failed to synchronise in-memory keys and values.");
        }
    }

    private void OnKeyValueStoreChanged(eCloudDataStoreValueChangeReason _reason, string[] _changedKeys)
    {
        Debug.Log("Cloud key-value store has been changed.");
        Debug.Log(string.Format("Reason: {0}.", _reason));

        string message = "";

        switch (_reason)
        {
            case eCloudDataStoreValueChangeReason.INITIAL_SYNC:
                message += "Initial Download from cloud server has not happend";
                break;
            case eCloudDataStoreValueChangeReason.SERVER:
                message += "Someone else is using the same cloud service account";
                break;
            case eCloudDataStoreValueChangeReason.QUOTA_VIOLATION:
                message += "Quota violation";
                break;
            case eCloudDataStoreValueChangeReason.STORE_ACCOUNT:
                message += "Signed in with another account";
                break;
        }

        Debug.Log(message);

        prompt.SetPrompt("Failed to Load", $"Could not retreive you data, {message}. \nplease try restarting the game.");
    }

    private void OnKeyValueStoreDidSynchronise(bool _success)
    {
        if (_success)
        {
           // prompt.SetPrompt("Cloud Synchronized", "Successfully synchronised in-memory keys and values.");
            Debug.Log("Successfully synchronised in-memory keys and values.");
        }
        else
        {
            prompt.SetPrompt("Failed to Load", "Could not synchronize you data, press okay to try again or restart the game.", ReSync);
            Debug.Log("Failed to synchronise in-memory keys and values.");
        }
    }

    public void SaveGame()
    {
        NPBinding.CloudServices.SetLong(GearsKey, GearManager.instance.GetGears());
        NPBinding.CloudServices.SetLong(PlayerIdxKey, SwapManager.PlayerIdx);
        NPBinding.CloudServices.SetLong(BackgroundIdxKey, SwapManager.BackgroundIdx);
        NPBinding.CloudServices.SetLong(MusicIdxKey, SwapManager.MusicIdx);
        NPBinding.CloudServices.SetLong(ObstacleIdxKey, SwapManager.ObstacleIdx);
        NPBinding.CloudServices.SetLong(ProjectileIdxKey, SwapManager.ProjectileIdx);

        NPBinding.CloudServices.SetList(PlayerUnlocksKey, SwapManager.PlayerUnlocks);
        NPBinding.CloudServices.SetList(BackgroundUnlocksKey, SwapManager.BackgroundUnlocks);
        NPBinding.CloudServices.SetList(MusicUnlocksKey, SwapManager.MusicUnlocks);
        NPBinding.CloudServices.SetList(ObstacleUnlocksKey, SwapManager.ObstacleUnlocks);

        NPBinding.CloudServices.SetLong(HighscoreKey, player.Score().GetHighscore());
        NPBinding.CloudServices.SetLong(HighDistanceKey, player.Score().GetHighscore(false));
    }

    public void LoadGame()
    {
        try
        {
            GearManager.instance.SetGears((int)NPBinding.CloudServices.GetLong(GearsKey));
            SwapManager.PlayerIdx = (int)NPBinding.CloudServices.GetLong(PlayerIdxKey);
            SwapManager.BackgroundIdx = (int)NPBinding.CloudServices.GetLong(BackgroundIdxKey);
            SwapManager.MusicIdx = (int)NPBinding.CloudServices.GetLong(MusicIdxKey);
            SwapManager.ObstacleIdx = (int)NPBinding.CloudServices.GetLong(ObstacleIdxKey);
            SwapManager.ProjectileIdx = (int)NPBinding.CloudServices.GetLong(ProjectileIdxKey);

            SwapManager.PlayerUnlocks = GetUnlocks(PlayerUnlocksKey);
            SwapManager.BackgroundUnlocks = GetUnlocks(BackgroundUnlocksKey);
            SwapManager.MusicUnlocks = GetUnlocks(MusicUnlocksKey);
            SwapManager.ObstacleUnlocks = GetUnlocks(ObstacleUnlocksKey);

            SwapManager.AddDefaults();

            player.Score().SetHighscores((int)NPBinding.CloudServices.GetLong(HighscoreKey), (int)NPBinding.CloudServices.GetLong(HighDistanceKey));
            player.InitUnlock();
        }
        catch (Exception e)
        {
            prompt.SetPrompt("Error Getting Data!", "There was a critical error while retreiving your data, try restarting the game.");
        }   
    }

    public void DefaultLoad()
    {
        GearManager.instance.SetGears(0);

        SwapManager.PlayerIdx = 0;
        SwapManager.BackgroundIdx = 0;
        SwapManager.MusicIdx = 0;
        SwapManager.ObstacleIdx = 0;

        SwapManager.PlayerUnlocks = new List<int>();
        SwapManager.BackgroundUnlocks = new List<int>();
        SwapManager.MusicUnlocks = new List<int>();
        SwapManager.ObstacleUnlocks = new List<int>();

        SwapManager.AddDefaults();

        player.Score().SetHighscores(0, 0);

        player.InitUnlock();
    }

    public void ResetData()
    {
        DefaultLoad();
        SaveGame();
    }

    private List<int> GetUnlocks(string key)
    {
        var thing = NPBinding.CloudServices.GetList(key);
        if (thing != null)
        {
            List<int> unlocks = new List<int>();

            foreach(var idx in thing)
            {
                unlocks.Add(Convert.ToInt32(idx));
            }

            return unlocks;
        }
        else
            return new List<int>();
    }

    private void ReSync(bool success)
    {
        NPBinding.CloudServices.Synchronise();
    }

    private void reinit(bool success)
    {
        InitializeCloud();
    }

    public void InitializeCloud()
    {
        NPBinding.CloudServices.Initialise();
    }
}
