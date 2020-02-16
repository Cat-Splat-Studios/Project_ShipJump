using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour, IStoreListener
{

    private IStoreController controller;
    private IExtensionProvider extensions;

    // Start is called before the first frame update
    void Start()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct("1000_gears_google", ProductType.Consumable);
        builder.AddProduct("5000_gears_google", ProductType.Consumable);
        builder.AddProduct("10000_gears_google", ProductType.Consumable);

        UnityPurchasing.Initialize(this,builder);
    }

    public void BuyGears(string id)
    {
        Product product = controller.products.WithID(id);

        if(product != null && product.availableToPurchase)
        {
            controller.InitiatePurchase(product);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log($"Failed Purchasing {i}: {p}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        return PurchaseProcessingResult.Complete;
    }

}
