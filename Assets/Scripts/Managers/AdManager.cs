/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the unity ads within the game
**/
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoSingleton<AdManager>
{
#pragma warning disable 0649
    [SerializeField]
    private bool testMode = true;
    [SerializeField]
    private int gearReward = 100;

    // Time durations for each ad
    [SerializeField]
    private float normalthresholdTime = 180.0f;
    [SerializeField]
    private float buttonthresholdTime = 240.0f;

    [SerializeField]
    private MessageBox rewardPrompt;

    [SerializeField]
    private GameObject adButton;

    private string gameId = "3468117";
    private string myPlacementId = "video";
    private string myRewardPlacementId = "rewardedVideo";

    // Current time tracking for each ad
    private float currentTimeThreshold = 0.0f;
    private float currentTimeButtonThreshold = 0.0f;

    private int gamesPlayedSinceAd = 0;
    private int gamesPlayedSinceButtonAd = 0;

    private bool buttonShown = false;

    private bool firstAdPlay = false;

    // callback operations on both ads
    private ShowOptions op;
    private ShowOptions op1;

    private bool isTracking = false;

    // Start is called before the first frame update
    void Start()
    {     
        InitAds();
    }

    public void InitAds()
    {
        Advertisement.Initialize(gameId, testMode);
        op = new ShowOptions();
        op1 = new ShowOptions();

        // deprecated way of doing ads, but more clearer way, new way was not working
        op1.resultCallback = OnNormalAdFinish;
        op.resultCallback = OnUnityAdsDidFinish;
    }

    public void ToggleTracking(bool value)
    {
        // Enable tracking for ads (turns on ads)
        isTracking = value;
        currentTimeThreshold = 0.0f;
        currentTimeButtonThreshold = 0.0f;
    }

    private void Update()
    {  
        if (isTracking)
        {
            currentTimeThreshold += Time.deltaTime;
            currentTimeButtonThreshold += Time.deltaTime;
        }  
    }

    public void AdCheck()
    {
        // check if it is time to play normal ad
        if(isTracking)
        {
            if ((!firstAdPlay && gamesPlayedSinceAd > 0) || (currentTimeThreshold > normalthresholdTime && gamesPlayedSinceAd > 1) || gamesPlayedSinceAd >= 5)
            {
                PlayAd();
            }

            ++gamesPlayedSinceAd;
        }       
    }

    public void ButtonCheck()
    {
        if(isTracking)
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
        currentTimeThreshold = 0.0f;
        Time.timeScale = 0.0f;
        Advertisement.Show(myPlacementId, op1);
        
    }

    public void OnUnityAdsDidFinish(ShowResult showResult)
    {
        // reward player and let them know, hide button after
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
        //Advertisement.RemoveListener(this);
    }

    public void OnNormalAdFinish(ShowResult showResult)
    {
        Time.timeScale = 1.0f;
        firstAdPlay = true;
        gamesPlayedSinceAd = 0;
    }

    public void PlayRewardedAd()
    {
        Advertisement.Show(myRewardPlacementId, op);
        Time.timeScale = 0.0f;
    }

    public void OnConfirmReward(bool confirm)
    {
        Time.timeScale = 1.0f;       
    }

    private void ShowButton()
    {
        adButton.SetActive(true);
        buttonShown = true;
    }

    private void HideButton()
    {
        adButton.SetActive(false);
        buttonShown = false;    
    }
}

