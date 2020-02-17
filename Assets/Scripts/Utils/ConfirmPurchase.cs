using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmPurchase : MonoBehaviour
{
    [SerializeField]
    private Text priceText;

    private Action<bool> hasPurchased;


    public void SetReference(Action<bool> hasPurchased, int price)
    {
        priceText.text = price.ToString();
        this.hasPurchased = hasPurchased;
    }

    public void Confirm()
    {
        hasPurchased.Invoke(true);
        this.gameObject.SetActive(false);
    }

    public void Cancel()
    {
        hasPurchased.Invoke(false);
        this.gameObject.SetActive(false);
    }
}
