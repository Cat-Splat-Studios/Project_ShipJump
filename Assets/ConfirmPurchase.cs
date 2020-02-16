using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPurchase : MonoBehaviour
{
    [SerializeField]
    private Text priceText;

    private ItemInfo currentinfo;

    public void SetReference(ItemInfo info, int price)
    {
        currentinfo = info;
        priceText.text = price.ToString();
    }

    public void Confirm()
    {
        currentinfo.ConfirmPurchase();
        this.gameObject.SetActive(false);
    }

    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }
}
