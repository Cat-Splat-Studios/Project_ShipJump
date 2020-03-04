
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

    private bool buttonShown = false;

    ShowOptions op;
    ShowOptions op1;

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
            if (buttonShown)
            {
                currentTimeButtonThreshold = 0.0f;
            }
            HideButton();      
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
        if (showResult == ShowResult.Finished)
        {
            GearManager.instance.AddGears(gearReward);
            SaveManager.instance.SaveToCloud();
            rewardPrompt.SetPrompt("Reward!", $"You have been rewarded {gearReward} gears for watching the ad.", OnConfirmReward);
        }

        HideButton();
        currentTimeButtonThreshold = 0.0f;
        //Advertisement.RemoveListener(this);
    }

    public void OnNormalAdFinish(ShowResult showResult)
    {
        Time.timeScale = 1.0f;
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

