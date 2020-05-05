/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the logic of gears to the player
**/

using UnityEngine;

public class GearManager : MonoSingleton<GearManager>
{
#pragma warning disable 0649
    [SerializeField]
    private MessageBox prompt;

    [SerializeField]
    private UIDelgate ui;

    // Gears player gets in a round
    [HideInInspector]
    public int levelGears { get; private set; }
    
    // Total gears player has
    private int gears;
    private bool isDouble = false;

    public int GetGears()
    {
        return gears;
    }

    public void SetGears(int gears)
    {
        this.gears = gears;
    }

    public void PurchaseGears(int amount)
    {
        gears += amount;    
        prompt.SetPrompt("Gears Purchased!", $"You Purchased {amount} gears.");
        ui.UpdateGearText();
        CloudSaving.instance.SaveGame();
    }

    public void IncrementGears()
    {
        // If player has the "double gears" this round, give them 2
        if (isDouble)
        {
            levelGears += 2;
            gears += 2;
        }           
        else
        {
            levelGears++;
            gears++;
        }

        ui.SetGameGearText(levelGears);
    }

    public void ToggleDoubleGears(bool value)
    {
        isDouble = value;
    }

    public void RewardGears(int amount)
    {
        // Reward for watching ads - Double Gears
        AddGears(amount);
        ToggleDoubleGears(true);   
    }

    public void AddGears(int amount)
    {
        gears += amount;
        ui.UpdateGearText();
    }

    public void RemoveGears(int amount)
    {
        gears -= amount;
        ui.UpdateGearText();
    }
    public void ResetLevelGears()
    {
        levelGears = 0;
        ui.SetGameGearText(levelGears);
    }

    public bool CheckGears(int amount)
    {
        return gears >= amount;
    }
}
