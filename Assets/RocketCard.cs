using System.Collections;
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
    public PlayerManager player;
    public MessageBox confirmPurchase;
    public RocketCardImage[] images;

    [Header("Card UI")]
    public Text priceText;
    public Image card;
    public Button actionButton;

    private Dictionary<int, Sprite> cardImages;

    private int price;
    private string rocketName;

    public void InitImageList()
    {
        cardImages = new Dictionary<int, Sprite>();
        foreach (var img in images)
        {
            cardImages.Add(img.unlockIdx, img.image);
        }
    }

    public void InitCardBuy(int rocketIdx, int rocketPrice)
    {
        card.sprite = cardImages[rocketIdx];
        price = rocketPrice;
        priceText.text = price.ToString();
        actionButton.interactable = true;
    }

    public void InitCardView(int rocketIdx)
    {
        card.sprite = cardImages[rocketIdx];
        priceText.text = "Owned";
        actionButton.interactable = false;
    }

    public void BuyRocket()
    {
        confirmPurchase.SetPrompt($"Purchase Rocket?", price.ToString(), onPurchased);
    }

    public void onPurchased(bool success)
    {
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
