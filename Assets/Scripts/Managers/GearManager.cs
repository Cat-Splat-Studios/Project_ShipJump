
using UnityEngine;

public class GearManager : MonoSingleton<GearManager>
{
    [SerializeField]
    private MessageBox prompt;

    [HideInInspector]
    public int levelGears { get; private set; }
    
    private UIDelgate ui;

    private int gears;
    private bool isDouble = false;

    private void Start()
    {
        ui = FindObjectOfType<UIDelgate>();
    }
    public int GetGears()
    {
        return gears;
    }

    public void SetGears(int gears)
    {
        this.gears = gears;
       // ui.UpdateGearText();
    }

    public void PurchaseGears(int amount)
    {
        gears += amount;    
        prompt.SetPrompt("Gears Purchased!", $"You Purchased {amount} gears.");
        ui.UpdateGearText();
        CloudSaving.instance.SaveGame();
        //SaveManager.instance.SaveToCloud();
    }

    public void IncrementGears()
    {
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
    }

    public void ToggleDoubleGears(bool value)
    {
        isDouble = value;
    }

    public void RewardGears(int amount)
    {
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
    }

    public bool CheckGears(int amount)
    {
        return gears >= amount;
    }

    //private void SaveGears()
    //{
    //    SaveManager.instance.SaveToCloud();
    //}
}
