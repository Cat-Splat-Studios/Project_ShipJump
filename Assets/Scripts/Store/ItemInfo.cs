/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the purchasing of an item
**/

using UnityEngine;
using UnityEngine.UI;

public enum EAssetType
{
    BACKGROUND,
    MUSIC,
    ROCKET,
    OBSTACLE,
    POWERUPS
}

public class ItemInfo : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Text priceText;
    [SerializeField]
    private Text buttonText;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject selected;

    [Header("Info")]
    [SerializeField]
    private EAssetType itemtype;
    [SerializeField]
    private int price;
    [SerializeField]
    private Shop parentShop;

    public int unlockIndex;

    private bool isUnlocked;

    private SwapManager swapper;
    public MessageBox confirmPurchasePrompt;

    private void Start()
    {
        swapper = FindObjectOfType<SwapManager>();
    }

    public void SetPurchase()
    {
        bool value = false;

        switch (itemtype)
        {
            case EAssetType.BACKGROUND:
                value = SwapManager.BackgroundUnlocks.Contains(unlockIndex);
                break;
            case EAssetType.MUSIC:
                value = SwapManager.MusicUnlocks.Contains(unlockIndex);
                break;
            case EAssetType.OBSTACLE:
                value = SwapManager.ObstacleUnlocks.Contains(unlockIndex);
                break;
        }

        isUnlocked = value;

        if (value)
        {
            buttonText.text = "Select";
            SetSelectedeButtonActive();
            priceText.text = "Owned";
        }
        else
        {
            buttonText.text = "Purchase";
            SetPurchaseButtonActive();
            priceText.text = price.ToString();
        }
    }

    public void SetPurchaseButtonActive()
    {
        if(GearManager.instance.CheckGears(price) && !isUnlocked)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void SetSelectedeButtonActive()
    {
        bool active = false;
        switch (itemtype)
        {
            case EAssetType.BACKGROUND:
                if(SwapManager.BackgroundIdx == unlockIndex)
                {
                    active = true;
                }
                break;
            case EAssetType.MUSIC:
                if (SwapManager.MusicIdx == unlockIndex)
                {
                    active = true;
                }
                break;
            case EAssetType.OBSTACLE:
                if (SwapManager.ObstacleIdx == unlockIndex)
                {
                    active = true;
                }
                break;
        }


        selected.SetActive(active);
        button.interactable = !active;
    }

    public void ButtonAction()
    {
        if(isUnlocked)
        {
            // Swap the asset
            switch(itemtype)
            {
                case EAssetType.BACKGROUND:
                    swapper.BackgroundSelect(unlockIndex);
                    break;
                case EAssetType.MUSIC:
                    swapper.MusicSelect(unlockIndex);
                    break;
                case EAssetType.OBSTACLE:
                    swapper.ObstacleSelect(unlockIndex);
                    break;
            }
            parentShop.InitItems();
        }
        else
        {
            confirmPurchasePrompt.SetPrompt("Purchase Item?", price.ToString(), ConfirmPurchase);
        }     
    }

    public void ConfirmPurchase(bool isConfirmed)
    {
        
        if (isConfirmed && GearManager.instance.CheckGears(price))
        {
            GearManager.instance.RemoveGears(price);
            swapper.PurchaseAsset(unlockIndex, itemtype);
            isUnlocked = true;
            parentShop.InitItems();
        }
    }
}
