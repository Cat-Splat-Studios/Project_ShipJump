﻿/** 
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

    public int HighScore { get; set; }

    public int PlayerIdx { get; set; }
    public int BackgroundIdx { get; set; }
    public int MusicIdx { get; set; }
    public int ObstacleIdx { get; set; }
    public int ProjectileIdx { get; set; }

    public int EmergencyFuelCount { get; set; }
    public int DoubleShieldCount { get; set; }

    public List<int> PlayerUnlocks { get; set; }

    public List<int> BackgroundUnlocks { get; set; }

    public List<int> MusicUnlocks { get; set; }

    public List<int> ObstacleUnlocks { get; set; }

    public List<int> ProjectileUnlocks { get; set; }

}

public class SaveManager : MonoSingleton<SaveManager>
{
    [SerializeField]
    private MessageBox prompt;
    [SerializeField]
    private UIDelgate ui;
    [SerializeField]
    private PlayerManager player;
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

    public void SaveToCloud()
    {
        if(PlayGamesPlatform.Instance.IsAuthenticated())
        {
            MapToState();
            GPGSUtils.instance.OpenCloudSave(OnSaveResponse);
        }
    }

    public void LoadFromCloud()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
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

        state.EmergencyFuelCount = 0;
        state.DoubleShieldCount = 0;

        MapToView();

        player.InitUnlock();
        ui.UpdateInfoText();
    }

    public void SetHighScore(int score)
    {
        state.HighScore = score; 
    }

    public int GetHighscore()
    {
        return state.HighScore;
    }

    /** Helper Methods**/

    // Load Callbacks
    private void OnLoadResponse(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.ReadBinaryData(meta, LoadCallBack);
        }
        else
        {
            Debug.Log("Did not load: " + status);
            prompt.SetPrompt("Did not load", status.ToString());
            DefaultLoad();

            ui.HasAuthenitcated();
            ui.UpdateInfoText();

            player.InitUnlock();
        }
    }

    private void LoadCallBack(SavedGameRequestStatus status, byte[] data)
    {
        if(data.Length > 0)
        {
            state = DeserializeState(data);
            MapToView();
            SwapManager.instance.SwapInit();
        }
        else
        {
            DefaultLoad();
        }

        ui.HasAuthenitcated();
        ui.UpdateInfoText();

        player.InitUnlock();

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
        }
        else
        {
            SaveCallBack(status, meta);
        }

    }
    private void SaveCallBack(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            //prompt.SetPrompt("Saved!", "Player State Successfully saved");
        }
        else
        {
            prompt.SetPrompt("Did not save", status.ToString());
        }
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

        SwapManager.EmergencyFuelCount = state.EmergencyFuelCount;
        SwapManager.DoubleShieldCount = state.DoubleShieldCount;

        SwapManager.PlayerUnlocks = state.PlayerUnlocks;
        SwapManager.BackgroundUnlocks = state.BackgroundUnlocks;
        SwapManager.MusicUnlocks = state.MusicUnlocks;
        SwapManager.ObstacleUnlocks = state.ObstacleUnlocks;

        SwapManager.instance.AddDefaults();
    }

    // maps local info back to state for saving
    private void MapToState()
    {
        state.Gears = GearManager.instance.GetGears();

        state.PlayerIdx = SwapManager.PlayerIdx;
        state.BackgroundIdx = SwapManager.BackgroundIdx;
        state.MusicIdx = SwapManager.MusicIdx;
        state.ObstacleIdx = SwapManager.ObstacleIdx;

        // local save for current assets
        PlayerPrefs.SetInt("playerIdx", SwapManager.PlayerIdx);
        PlayerPrefs.SetInt("backgroundIdx", SwapManager.BackgroundIdx);
        PlayerPrefs.SetInt("musicIdx", SwapManager.MusicIdx);
        PlayerPrefs.SetInt("obstacleIdx", SwapManager.ObstacleIdx);

        state.EmergencyFuelCount = SwapManager.EmergencyFuelCount;
        state.DoubleShieldCount = SwapManager.DoubleShieldCount;

        state.PlayerUnlocks = SwapManager.PlayerUnlocks;
        state.BackgroundUnlocks = SwapManager.BackgroundUnlocks;
        state.MusicUnlocks = SwapManager.MusicUnlocks;
        state.ObstacleUnlocks = SwapManager.ObstacleUnlocks;
    }

}
