
using UnityEngine;

public class GearManager : MonoSingleton<GearManager>
{
    public MessageBox prompt;

    [HideInInspector]
    public int levelGears { get; private set; }
    
    private UIDelgate ui;

    private int gears;

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
    }

    public void IncrementGears()
    {
        levelGears++;
        gears++;
    }

    public void AddGears(int amount)
    {
        gears += amount;
        ui.UpdateInfoText();
    }

    public void RemoveGears(int amount)
    {
        gears -= amount;
        ui.UpdateInfoText();
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
