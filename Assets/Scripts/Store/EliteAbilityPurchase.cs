/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the elite ability items in the store
**/

using UnityEngine;

public enum EAbilityType
{
    EMERGENCYFUEL,
    DOUBLESHIELDS
}

public class EliteAbilityPurchase : MonoBehaviour
{
    public EAbilityType type;
    public int price;
    public MessageBox messageBox;
    public AbilityPurchase purchase;

    public void SelectPurchase()
    {
        string title = "";
        string description = "";

        switch (type)
        {
            case EAbilityType.EMERGENCYFUEL:
                title = "Purchase Emergency Fuel";
                break;
            case EAbilityType.DOUBLESHIELDS:
                title = "Purchase Double Shields";
                break;
        }

        messageBox.SetPrompt(title, description, onPurchase);
        purchase.SetPrice(price);
    }

    public void onPurchase(bool confirm)
    {
        if(confirm)
        {
            switch (type)
            {
                case EAbilityType.EMERGENCYFUEL:
                    SwapManager.EmergencyFuelCount += purchase.GetAmountPurchased();
                    break;
                case EAbilityType.DOUBLESHIELDS:
                    SwapManager.DoubleShieldCount += purchase.GetAmountPurchased();
                    break;
            }

            GearManager.instance.RemoveGears(purchase.GetTotalPrice());

            SaveManager.instance.SaveToCloud();
        }
    }
}
