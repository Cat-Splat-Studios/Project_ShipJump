
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoSingleton<AdManager>
{

    public bool testMode = true;
    public int gearReward = 200;
    public float thresholdTime = 180.0f;

    public MessageBox rewardPrompt;

    private string gameId = "3468117";
    private string myPlacementId = "rewardedVideo";

    private float currentTimeThreshold = 0.0f;

    private bool AdWait = true;

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
        if(AdWait)
        {
            currentTimeThreshold += Time.deltaTime;      
        }
    }

    public void AdCheck()
    {
        if (currentTimeThreshold > thresholdTime)
        {
            PlayAd();
        }
    }

    private void PlayAd()
    {
        AdWait = false;
        Advertisement.Show(myPlacementId, op);
        Time.timeScale = 0.0f;
    }

    public void OnUnityAdsDidFinish(ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            GearManager.instance.AddGears(gearReward);
            SaveManager.instance.SaveToCloud();
            currentTimeThreshold = 0.0f;
            rewardPrompt.SetPrompt("Reward!", $"You have been rewarded {gearReward} gears for watching the ad.", OnConfirmReward);
        }
        
        AdWait = true;
        //Advertisement.RemoveListener(this);
    }

    public void OnConfirmReward(bool confirm)
    {
        Time.timeScale = 1.0f;       
    }
}

