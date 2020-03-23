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

    private bool isOffline = false;

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
                    isOffline = false;
                    ui.HasAuthenitcated();
                    SaveManager.instance.LoadFromCloud();
                    ui.toggleOnlineButtons(true);
                    AdManager.instance.ToggleTracking(true);
                }
                else
                {
                    if(!isOffline)
                    {
                        ui.HasAuthenitcated();
                        SaveManager.instance.DefaultLoad();
                        OfflineMode();
                    }
                    else
                    {
                        prompt.SetPrompt("Could Not sign In", "Authentication has failed.");
                    }         
                }
            });
          
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public bool CheckAuth()
    {
        bool result = PlayGamesPlatform.Instance.IsAuthenticated();

        if (!result)
        {
            OfflineMode();
        }

        return result;
    }

    public void OpenCloudSave(Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
    {
        if(CheckAuth())
        {
            try
            {
                var platform = (PlayGamesPlatform)Social.Active;
                platform.SavedGame.OpenWithAutomaticConflictResolution(cloudSaveName, DataSource.ReadNetworkOnly, ConflictResolutionStrategy.UseLongestPlaytime, callback);
            }
            catch (Exception e)
            {
                prompt.SetPrompt("Error: Open Save File", e.Message);
            }
        }      
    }

    public void SubmitScore(int score)
    {
        if(CheckAuth())
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
        if (CheckAuth())
            Social.ShowLeaderboardUI();
    }

    private void OfflineMode()
    {
        // custom code here for when your application is offline
        if(!isOffline)
        {
            isOffline = true;
            // disable all online buttons
            ui.toggleOnlineButtons(false);

            // disable ads
            AdManager.instance.ToggleTracking(false);

            // display message
            prompt.SetPrompt("Could Not sign In", "All progress will not be saved.\n You can attemp to sign in again at the settings screen.");        
        }
 
    }
}
