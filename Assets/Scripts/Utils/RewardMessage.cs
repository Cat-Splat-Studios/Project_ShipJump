using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardMessage : MessageBox
{
    public Text rewardTitle;
    public Text rewardDescription;
    public Text rewardDuration;
    public Image icon;

    public void SetRewardDisplay(string title, string desc, string dur)
    {
        rewardTitle.text = title;
        rewardDescription.text = desc;
        rewardDuration.text = dur;
    }
}
