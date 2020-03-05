/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the purchase logic of elite abilities (for multiple purchases)
**/

using UnityEngine;
using UnityEngine.UI;

public class AbilityPurchase : MonoBehaviour
{
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Text amountText;
    [SerializeField]
    private Button buyButton;

    private int price;
    private int amount;
    private int totalPrice;

    public void SetPrice(int price)
    {
        this.price = price;
        amount = 1;

        CalculateTotal();
    }

    public void AddToAmount()
    {
        if (amount <= 10)
        {
            amount++;
            CalculateTotal();
        }
    }

    public void SubtractFromAmount()
    {
        if (amount > 1)
        {
            amount--;
            CalculateTotal();
        }
    }

    public int GetAmountPurchased()
    {
        return amount;
    }

    public int GetTotalPrice()
    {
        return totalPrice;
    }

    private void CalculateTotal()
    {
        totalPrice = price * amount;
        CheckPrice();
        UpdateText();
    }

    private void UpdateText()
    {
        priceText.text = totalPrice.ToString();
        amountText.text = amount.ToString();
    }

    private void CheckPrice()
    {
        if (totalPrice > GearManager.instance.GetGears())
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }
}
