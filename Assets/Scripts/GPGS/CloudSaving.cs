using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;

public class CloudSaving : MonoBehaviour
{
    string GearsKey = "curGears";
    string PlayerIdxKey = "curPlayerIDX";
    string BackgroundIdxKey = "curBackgroundIDX";
    string MusicIdxKey = "curMusicIDX";
    string ObstacleIdxKey = "curObstacleIDX";
    string ProjectileIdxKey = "curProjectileIDX";

    string EmergencyFuelCount = "curEmergencyFuelCount";
    string DoubleShieldCount = "curDoubleShieldCount";

    string PlayerUnlocksKey = "curPlayerUnlocks";
    string BackgroundUnlocksKey = "curBackgroundUnlocks";
    string MusicUnlocksKey = "curMusicUnlocks";
    string ObstacleUnlocksKey = "curObstacleUnlocks";
    string ProjectileUnlocksKey = "curProjectileUnlocks";

    // Start is called before the first frame update
    void Awake()
    {
        NPBinding.CloudServices.Initialise();
    }

    // Register for the event
    void OnEnable()
    {
        CloudServices.KeyValueStoreDidInitialiseEvent += OnKeyValueStoreInitialised;
    }

    void OnDisable()
    {
        CloudServices.KeyValueStoreDidInitialiseEvent -= OnKeyValueStoreInitialised;
    }

    private void OnKeyValueStoreInitialised(bool _success)
    {
        if (_success)
        {
            Debug.Log("Successfully synchronised in-memory keys and values.");
        }
        else
        {
            Debug.Log("Failed to synchronise in-memory keys and values.");
        }
    }

    void SaveGame()
    {
        NPBinding.CloudServices.SetLong(GearsKey, SwapManager.Gears);
        NPBinding.CloudServices.SetLong(PlayerIdxKey, SwapManager.PlayerIdx);
        NPBinding.CloudServices.SetLong(BackgroundIdxKey, SwapManager.BackgroundIdx);
        NPBinding.CloudServices.SetLong(MusicIdxKey, SwapManager.MusicIdx);
        NPBinding.CloudServices.SetLong(ObstacleIdxKey, SwapManager.ObstacleIdx);
        NPBinding.CloudServices.SetLong(ProjectileIdxKey, SwapManager.ProjectileIdx);

        NPBinding.CloudServices.SetList(PlayerUnlocksKey, SwapManager.PlayerUnlocks);
        NPBinding.CloudServices.SetList(BackgroundUnlocksKey, SwapManager.BackgroundUnlocks);
        NPBinding.CloudServices.SetList(MusicUnlocksKey, SwapManager.MusicUnlocks);
        NPBinding.CloudServices.SetList(ObstacleUnlocksKey, SwapManager.ObstacleUnlocks);
        NPBinding.CloudServices.SetList(ProjectileUnlocksKey, SwapManager.ProjectileUnlocks);
    }
}
