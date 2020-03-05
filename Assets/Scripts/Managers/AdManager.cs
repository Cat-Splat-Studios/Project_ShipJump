/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all the unity ads within the game
**/

using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoSingleton<AdManager>
{
    [SerializeField]
    private bool testMode = true;
    [SerializeField]
    private int gearReward = 200;
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

    private float currentTimeThreshold = 0.0f;
    private float currentTimeButtonThreshold = 0.0f;

    private int gamesPlayedSinceAd = 0;

    private bool buttonShown = false;

    // callback operations on both ads
    private ShowOptions op;
    private ShowOptions op1;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        op = new ShowOptions();
        op1 = new ShowOptions();
        op1.resultCallback = OnNormalAdFinish;
        op.resultCallback = OnUnityAdsDidFinish;
    }

    private void Update()
    {   
        currentTimeThreshold += Time.deltaTime;      
        currentTimeButtonThreshold += Time.deltaTime;  
    }

    public void AdCheck()
    {
        // check if it is time to play normal ad
        if (currentTimeThreshold > normalthresholdTime && gamesPlayedSinceAd > 1)
        {
            PlayAd();
        }     
    }

    public void ButtonCheck()
    {
        // check if it is time to show the button for reward ad
        if (currentTimeButtonThreshold > buttonthresholdTime && buttonShown == false)
        {
            ShowButton();
        }
        else
        {
            if (buttonShown)
            {
                currentTimeButtonThreshold = 0.0f;
            }
            HideButton();      
        }

        ++gamesPlayedSinceAd;
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
            GearManager.instance.AddGears(gearReward);
            SaveManager.instance.SaveToCloud();
            rewardPrompt.SetPrompt("Reward!", $"You have been rewarded {gearReward} gears for watching the ad.", OnConfirmReward);
        }

        HideButton();
        currentTimeButtonThreshold = 0.0f;
        gamesPlayedSinceAd = 0;
        //Advertisement.RemoveListener(this);
    }

    public void OnNormalAdFinish(ShowResult showResult)
    {
        Time.timeScale = 1.0f;
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

