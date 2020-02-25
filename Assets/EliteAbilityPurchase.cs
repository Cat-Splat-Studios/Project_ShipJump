using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                description = "Use once per round to regain fuel in desperate times!";
                break;
            case EAbilityType.DOUBLESHIELDS:
                title = "Purchase Double Shields";
                description = "Use once per round to enhanced shields that can take two hits!";
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
        }
    }
}
