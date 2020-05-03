/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To hold the information and logic for purchasing rockets in the shop view
**/

using UnityEngine;
using UnityEngine.UI;

public class RocketStoreCard : MonoBehaviour
{
    public PlayerManager player;
    public int rocketIdxPrice;
    public int rocketIdxUnlock;

    public Text priceText;
    public Button purchaseButton;

    public MessageBox purchaseBox;

    private int price;

    private void Start()
    {
        CardCheck();
    }

    public void CardCheck()
    {
        bool isUnlock = SwapManager.PlayerUnlocks.Contains(rocketIdxUnlock);

        if (isUnlock)
        {
            priceText.text = "Owned";
            purchaseButton.interactable = false;
            purchaseButton.gameObject.SetActive(false);
        }
        else
        {
            price = SwapManager.rocketPrices[rocketIdxPrice];
            priceText.text = price.ToString();
            purchaseButton.gameObject.SetActive(true);

            if(GearManager.instance.CheckGears(price))
                purchaseButton.interactable = true;
            else
                purchaseButton.interactable = false;
        }
    }

    public void PurchaseRocket()
    {
        purchaseBox.SetPrompt($"Purchase Rocket?", price.ToString(), onPurchased);
    }

    public void onPurchased(bool success)
    {
        if(success)
        {
            SwapManager.instance.PurchaseAsset(rocketIdxUnlock, EAssetType.ROCKET);
            GearManager.instance.RemoveGears(price);
            priceText.text = "Owned";
            purchaseButton.interactable = false;
            purchaseButton.gameObject.SetActive(false);
        }
    }
}
