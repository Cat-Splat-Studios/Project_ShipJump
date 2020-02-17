/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: A manger to handle the Google Save logic in the game
**/

using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

// Serializable class to hold all the info for saved game
[System.Serializable]
public class SaveState
{
    public int Gears { get; set; }

    public int PlayerIdx { get; set; }
    public int BackgroundIdx { get; set; }
    public int MusicIdx { get; set; }
    public int ObstacleIdx { get; set; }
    public int ProjectileIdx { get; set; }

    public List<int> PlayerUnlocks { get; set; }

    public List<int> BackgroundUnlocks { get; set; }

    public List<int> MusicUnlocks { get; set; }

    public List<int> ObstacleUnlocks { get; set; }

    public List<int> ProjectileUnlocks { get; set; }

}

public class SaveManager : MonoSingleton<SaveManager>
{
    public Dialog dialog;
    [HideInInspector]
    public SaveState state;

    private BinaryFormatter formatter;

    private void Awake()
    {
        // initialization
        formatter = new BinaryFormatter();
        state = new SaveState();

        state.PlayerUnlocks = new List<int>();
        state.BackgroundUnlocks = new List<int>();
        state.MusicUnlocks = new List<int>();
        state.ObstacleUnlocks = new List<int>();
    }

    public void SaveToCloud(Action<EPlayServiceError> errorCallback = null)
    {
        MapToState();
        GPGSUtils.instance.OpenCloudSave(OnSaveResponse);  
    }

    public void LoadFromCloud(Action<EPlayServiceError> errorCallback = null)
    {
        GPGSUtils.instance.OpenCloudSave(OnLoadResponse);
    }
  
    // Incase loading from save fails, create an initial state
    public void DefaultLoad()
    {
        state.Gears = 0;

        state.PlayerIdx = 0;
        state.BackgroundIdx = 0;
        state.MusicIdx = 0;
        state.BackgroundIdx = 0;

        state.PlayerUnlocks.Add(0);
        state.BackgroundUnlocks.Add(0);
        state.MusicUnlocks.Add(0);
        state.ObstacleUnlocks.Add(0);

        MapToView();

        FindObjectOfType<PlayerManager>().InitUnlock();
    }

    /** Helper Methods**/

    // Load Callbacks
    private void OnLoadResponse(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.ReadBinaryData(meta, LoadCallBack);

            Debug.Log("LOADED");
            dialog.SetError("Loaded!", status.ToString());
        }
        else
        {
            Debug.Log("Did not load: " + status);

            dialog.gameObject.SetActive(true);
            dialog.SetError("Did not load", status.ToString());
            DefaultLoad();
        }

        FindObjectOfType<PlayerManager>().InitUnlock();
    }
    private void LoadCallBack(SavedGameRequestStatus status, byte[] data)
    {
        state = DeserializeState(data);
        MapToView();
    }

    // Save Callbacks
    private void OnSaveResponse(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            byte[] data = SerializeState();
            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
                .WithUpdatedDescription("Last save: " + DateTime.Now.ToString())
                .Build();

            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.CommitUpdate(meta, update, data, SaveCallBack);
            dialog.SetError("Saved!", status.ToString());
        }
        else
        {
            Debug.Log("Did not save: " + status);
            dialog.gameObject.SetActive(true);
            dialog.SetError("Did not save", status.ToString());
        }

    }
    private void SaveCallBack(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        Debug.Log("Did not save: " + status);
    }

    // Serializing and Deserialzing save data
    private byte[] SerializeState()
    {
        using (MemoryStream ms = new MemoryStream())
        {
            formatter.Serialize(ms, state);
            return ms.GetBuffer();
        }
    }

    private SaveState DeserializeState(byte[] data)
    {
        using (MemoryStream ms = new MemoryStream(data))
        {
            return (SaveState)formatter.Deserialize(ms);
        }
    }

    // Mappers to different variables
    private void MapToView()
    {
        GearManager.instance.SetGears(state.Gears);

        SwapManager.PlayerIdx = state.PlayerIdx;
        SwapManager.BackgroundIdx = state.BackgroundIdx;
        SwapManager.MusicIdx = state.MusicIdx;
        SwapManager.BackgroundIdx = state.BackgroundIdx;

        SwapManager.PlayerUnlocks = state.PlayerUnlocks;
        SwapManager.BackgroundUnlocks = state.BackgroundUnlocks;
        SwapManager.MusicUnlocks = state.MusicUnlocks;
        SwapManager.ObstacleUnlocks = state.ObstacleUnlocks;

        SwapManager.instance.AddDefaults();
    }

    private void MapToState()
    {
        state.Gears = SwapManager.Gears;

        state.PlayerIdx = SwapManager.PlayerIdx;
        state.BackgroundIdx = SwapManager.BackgroundIdx;
        state.MusicIdx = SwapManager.MusicIdx;
        state.BackgroundIdx = SwapManager.BackgroundIdx;

        state.PlayerUnlocks = SwapManager.PlayerUnlocks;
        state.BackgroundUnlocks = SwapManager.BackgroundUnlocks;
        state.MusicUnlocks = SwapManager.MusicUnlocks;
        state.ObstacleUnlocks = SwapManager.ObstacleUnlocks;
    }

}
