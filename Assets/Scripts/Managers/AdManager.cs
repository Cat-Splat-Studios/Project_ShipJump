
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoSingleton<AdManager>
{

    public bool testMode = true;
    public int gearReward = 200;
    public float normalthresholdTime = 180.0f;
    public float buttonthresholdTime = 240.0f;

    public MessageBox rewardPrompt;

    public GameObject adButton;

    private string gameId = "3468117";
    private string myPlacementId = "video";
    private string myRewardPlacementId = "rewardedVideo";

    private float currentTimeThreshold = 0.0f;
    private float currentTimeButtonThreshold = 0.0f;

    private bool buttonAdWait = true;
    private bool buttonShown = false;

    ShowOptions op;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameId, testMode);
        op = new ShowOptions();
        op.resultCallback = OnUnityAdsDidFinish;
    }

    private void Update()
    {
     
        currentTimeThreshold += Time.deltaTime;      
        

        if(buttonAdWait)
        {
            currentTimeButtonThreshold += Time.deltaTime;
        }
    }

    public void AdCheck()
    {

        if (currentTimeThreshold > normalthresholdTime)
        {
            PlayAd();
        }   
    }

    public void ButtonCheck()
    {
        if (currentTimeButtonThreshold > buttonthresholdTime && buttonShown == false)
        {
            ShowButton();
        }
        else
        {
            HideButton();
            if (buttonShown)
            {
                currentTimeButtonThreshold = 0.0f;
            }
        }
    }

    private void PlayAd()
    {
        currentTimeThreshold = 0.0f;
        Advertisement.Show(myPlacementId, op);
       
    }

    public void OnUnityAdsDidFinish(ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            GearManager.instance.AddGears(gearReward);
            SaveManager.instance.SaveToCloud();
            buttonthresholdTime = 0.0f;
            rewardPrompt.SetPrompt("Reward!", $"You have been rewarded {gearReward} gears for watching the ad.", OnConfirmReward);
        }

        HideButton();
        buttonAdWait = true;
        //Advertisement.RemoveListener(this);
    }

    public void PlayRewardedAd()
    {
        buttonAdWait = false;
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

