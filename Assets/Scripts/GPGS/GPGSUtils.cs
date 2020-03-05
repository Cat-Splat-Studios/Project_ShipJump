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


public class GPGSUtils : MonoSingleton<GPGSUtils>
{
    private string cloudSaveName = "rr_saveState";

    public MessageBox prompt;
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
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    SaveManager.instance.LoadFromCloud();
                }
                else
                {
                    prompt.SetPrompt("Could Not sign In", "You can still play!\n However... your progress will not be saved.");
                    ui.HasAuthenitcated();
                    SaveManager.instance.DefaultLoad();
                }
            });
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void OpenCloudSave(Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
    {
        try
        {
            var platform = (PlayGamesPlatform)Social.Active;
            platform.SavedGame.OpenWithAutomaticConflictResolution(cloudSaveName, DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseLongestPlaytime, callback);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            prompt.SetPrompt("Error: Open Save File", e.Message);
        }    
    }

    public void SubmitScore(int score)
    {
        if(PlayGamesPlatform.Instance.IsAuthenticated())
        {
            Social.ReportScore(score, GPGSIds.leaderboard_highest_kilometers_traveled, (bool Success) =>
            {
                Debug.Log("Score Added to Highscore");
                ui.LeaderBoard();
            });
        }     
    }

    public void ShowLeaderboard()
    {
        if (PlayGamesPlatform.Instance.IsAuthenticated())
            Social.ShowLeaderboardUI();
    }
}
