
using UnityEngine;

public class GearManager : MonoSingleton<GearManager>
{
    public Dialog dialog;

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
    }

    public void PurchaseGears(int amount)
    {
        gears += amount;
        dialog.gameObject.SetActive(true);
        dialog.SetError("Gears Purchased!", $"You Purchased {amount} gears.");
    }

    public void IncrementGears()
    {
        levelGears++;
        gears++;
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
