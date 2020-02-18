using UnityEngine.SceneManagement;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;

public class GPGSAuth : MonoBehaviour
{
    public Dialog dialog;

    private void Awake()
    {

        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
        .EnableSavedGames()
        .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        //SignIn();

    }

  
}
