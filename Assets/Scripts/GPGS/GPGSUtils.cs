/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: Utility class for all of the Google Play Service calls
**/

using UnityEngine;

//google stuff
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;

public enum EPlayServiceError : byte
{
    None = 0,
    Timeout = 1,
    NotAuthenticated = 2,
    SaveGameNotEneabled = 4,
    CloudSaveNameNotSet = 8
}

public class GPGSUtils : MonoSingleton<GPGSUtils>
{
    private string cloudSaveName = "rr_saveState";

    public Dialog dialog;
    public UIDelgate ui;
    // Start is called before the first frame update

    public override void Init()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        SignIn();
    }

    public void SignIn()
    {
        try
        {
            PlayGamesPlatform.Instance.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Authenticated");
                    ui.HasAuthenitcated();
                    SaveManager.instance.LoadFromCloud();
                }
                else
                {
                    Debug.Log("NOPE! not Authenticated");
                    dialog.gameObject.SetActive(true);
                    dialog.SetError("Error", "Did not Authenticate");
                    SaveManager.instance.DefaultLoad();
                    ui.HasAuthenitcated();
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void OpenCloudSave(Action<SavedGameRequestStatus, ISavedGameMetadata> callback, Action<EPlayServiceError> errorCallback = null)
    {
        // TODO: Error Handling 
        EPlayServiceError error = EPlayServiceError.None;

        //if (!Social.localUser.authenticated)
        //    error |= EPlayServiceError.NotAuthenticated;
        //if (PlayGamesClientConfiguration.DefaultConfiguration.EnableSavedGames)
        //    error |= EPlayServiceError.SaveGameNotEneabled; 
        //if (string.IsNullOrEmpty(cloudSaveName))
        //    error |= EPlayServiceError.CloudSaveNameNotSet;

        //if(error != EPlayServiceError.None)
        //{
        //    errorCallback?.Invoke(error);
        //    ErrorDiaolog errorDialog = FindObjectOfType<ErrorDiaolog>();
        //    errorDialog.gameObject.SetActive(true);
        //    errorDialog.SetError("Error", error.ToString());
        //    return;
        //}

        try
        {
            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.OpenWithAutomaticConflictResolution(cloudSaveName, DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseLongestPlaytime, callback);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            dialog.gameObject.SetActive(true);
            dialog.SetError("Error YALL", e.Message);
            //callback.Invoke(SavedGameRequestStatus.InternalError, null);
        }
        
    }

    public void SubmitScore(int score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_highest_kilometers_traveled, (bool Success) =>
        {
            Debug.Log("Score Added to Highscore");
            ui.LeaderBoard();
        });
    }

    public void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }


}
