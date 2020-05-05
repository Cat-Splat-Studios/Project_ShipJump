/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the unity ads within the game
**/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdService : MonoSingleton<AdService>, IUnityAdsListener
{
  
    public bool testMode = true;

    public int gearReward = 100;

    // Time durations for each ad

    public float normalthresholdTime = 180.0f;

    public float buttonthresholdTime = 240.0f;


    public MessageBox rewardPrompt;


    public Button adButton;
    public GameObject watchAd;

#if UNITY_IOS
    private string gameId = "3468116";
#elif UNITY_ANDROID
    private string gameId = "3468117";
#endif

    private string myPlacementId = "video";
    private string myRewardPlacementId = "rewardedVideo";

    // Current time tracking for each ad
    private float currentTimeThreshold = 0.0f;
    private float currentTimeButtonThreshold = 0.0f;

    private int gamesPlayedSinceAd = 0;
    private int gamesPlayedSinceButtonAd = 0;

    private bool buttonShown = false;

    private bool firstAdPlay = false;

    private bool isTracking = false;

    void Start()
    {
        InitAds();
    }
    private void Update()
    {
        if (isTracking)
        {
            currentTimeThreshold += Time.deltaTime;
            currentTimeButtonThreshold += Time.deltaTime;
        }
    }

    public void ToggleTracking(bool value)
    {
        // Enable tracking for ads (turns on ads)
        isTracking = value;
        currentTimeThreshold = 0.0f;
        currentTimeButtonThreshold = 0.0f;
    }

    public void AdCheck()
    {
        // check if it is time to play normal ad
        if (isTracking)
        {
            if ((!firstAdPlay && gamesPlayedSinceAd >= 0) || (currentTimeThreshold > normalthresholdTime && gamesPlayedSinceAd > 1) || gamesPlayedSinceAd >= 5)
            {
                PlayAd();
            }

            ++gamesPlayedSinceAd;
        }
    }

    public void ButtonCheck()
    {
        if (isTracking)
        {
            // check if it is time to show the button for reward ad
            if ((currentTimeButtonThreshold > buttonthresholdTime || gamesPlayedSinceButtonAd >= 4) && buttonShown == false)
            {
                ShowButton();
            }

            ++gamesPlayedSinceButtonAd;
        }

    }

    private void PlayAd()
    {
        firstAdPlay = true;
        Time.timeScale = 0.0f;
        Advertisement.Show(myPlacementId);
    }

    public void InitAds()
    {
        if(adButton)
        {
            adButton.interactable = Advertisement.IsReady(myRewardPlacementId);
            adButton.onClick.AddListener(PlayRewardedAd);
        }

        Advertisement.AddListener(this);

        Advertisement.Initialize(gameId, testMode);
        
    }

    public void PlayRewardedAd()
    {
        Time.timeScale = 0.0f;
        Advertisement.Show(myRewardPlacementId);
    }

    public void OnUnityAdsDidError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == myRewardPlacementId)
        {
            if (showResult == ShowResult.Finished)
            {
                GearManager.instance.RewardGears(gearReward);
                CloudSaving.instance.SaveGame();
                //SaveManager.instance.SaveToCloud();
                rewardPrompt.SetPrompt("Reward!", $"You have been rewarded {gearReward} gears\nAND\nDouble the Gears for next run!", OnConfirmReward);
            }

            HideButton();
            currentTimeButtonThreshold = 0.0f;
            gamesPlayedSinceButtonAd = 0;
        }

        Time.timeScale = 1.0f;
      
    }

    public void OnConfirmReward(bool confirm)
    {
        //
    }

    public void OnUnityAdsDidStart(string placementId)
    {
      //
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == myRewardPlacementId)
            adButton.interactable = true;
    }

    private void ShowButton()
    {
        watchAd.SetActive(true);
        buttonShown = true;
    }

    private void HideButton()
    {
        watchAd.SetActive(false);
        buttonShown = false;
    }
}
