using System;
using System.Collections;
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
            DefaultLoad();
            Debug.Log("Failed to synchronise in-memory keys and values.");
        }
    }

    public void SaveGame()
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

    public void LoadGame()
    {
        SwapManager.Gears = (int)NPBinding.CloudServices.GetLong(GearsKey);
        SwapManager.PlayerIdx= (int)NPBinding.CloudServices.GetLong(PlayerIdxKey);
        SwapManager.BackgroundIdx = (int)NPBinding.CloudServices.GetLong(BackgroundIdxKey);
        SwapManager.MusicIdx = (int)NPBinding.CloudServices.GetLong(MusicIdxKey);
        SwapManager.ObstacleIdx = (int)NPBinding.CloudServices.GetLong(ObstacleIdxKey);
        SwapManager.ProjectileIdx = (int)NPBinding.CloudServices.GetLong(ProjectileIdxKey);

        SwapManager.PlayerUnlocks = NPBinding.CloudServices.GetList(PlayerUnlocksKey) as List<int>;
        SwapManager.BackgroundUnlocks = NPBinding.CloudServices.GetList(BackgroundUnlocksKey) as List<int>;
        SwapManager.MusicUnlocks = NPBinding.CloudServices.GetList(MusicUnlocksKey) as List<int>;
        SwapManager.ObstacleUnlocks = NPBinding.CloudServices.GetList(ObstacleUnlocksKey) as List<int>;
        SwapManager.ProjectileUnlocks = NPBinding.CloudServices.GetList(ProjectileUnlocksKey) as List<int>;

#if UNITY_EDITOR
        DefaultLoad();
#endif
    }

    public void DefaultLoad()
    {
        GearManager.instance.SetGears(0);

        SwapManager.PlayerIdx = 0;
        SwapManager.BackgroundIdx = 0;
        SwapManager.MusicIdx = 0;
        SwapManager.BackgroundIdx = 0;

        SwapManager.PlayerUnlocks = new List<int>();
        SwapManager.BackgroundUnlocks = new List<int>();
        SwapManager.MusicUnlocks = new List<int>();
        SwapManager.ObstacleUnlocks = new List<int>();

        FindObjectOfType<PlayerManager>().InitUnlock();
        FindObjectOfType<UIDelgate>().UpdateInfoText();
    }


}
