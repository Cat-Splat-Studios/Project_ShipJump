/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To Display the information on the rocket to player when selected
**/

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public struct RocketCardImage
{
    public int unlockIdx;
    public Sprite image;
}

public class RocketCard : MonoBehaviour
{
    [SerializeField]
    private PlayerManager player;
    [SerializeField]
    private MessageBox confirmPurchase;
    [SerializeField]
    private RocketCardImage[] images;

    [Header("Card UI")]
    public Text priceText;
    public Image card;
    public Button actionButton;

    private Dictionary<int, Sprite> cardImages;

    private int price;

    public void InitImageList()
    {
        // Load images to dictionary for easy reference
        cardImages = new Dictionary<int, Sprite>();
        foreach (var img in images)
        {
            cardImages.Add(img.unlockIdx, img.image);
        }
    }

    public void InitCardBuy(int rocketIdx, int rocketPrice)
    {
        // Card View when not unlocked
        card.sprite = cardImages[rocketIdx];
        price = rocketPrice;
        priceText.text = price.ToString();
        actionButton.interactable = true;
    }

    public void InitCardView(int rocketIdx)
    {
        // Card view when unlocked
        card.sprite = cardImages[rocketIdx];
        priceText.text = "Owned";
        actionButton.interactable = false;
    }

    public void BuyRocket()
    {
        // Prompt purchase of rocket
        confirmPurchase.SetPrompt($"Purchase Rocket?", price.ToString(), onPurchased);
    }

    public void onPurchased(bool success)
    {
        // When player confirms purchase
        if(success)
        {
            player.RocketPurchaseConfirm(success);
            Back();
        }
    }

    public void Back()
    {
        this.gameObject.SetActive(false);
    }
}
