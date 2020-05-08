/** 
* Author: Hisham Ata, Matthew Douglas
* Purpose: To Handle the billing process utilizing the Voxel Native Plugin
**/

using UnityEngine;
using VoxelBusters.NativePlugins;

public class BillingManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    GameObject store;


    BillingProduct[] myProducts;
    BillingProduct[] requestedProducts;

    [SerializeField]
    GameObject[] storeButtons;

    public MessageBox prompt;

    private void Start()
    {
        myProducts = NPSettings.Billing.Products;
        RequestBillingProducts();
    }

    public void RequestBillingProducts()
    {
        NPBinding.Billing.RequestForBillingProducts(NPSettings.Billing.Products);

        // At this point you can display an activity indicator to inform user that task is in progress
    }

    private void OnEnable()
    {
        // Register for callbacks
        Billing.DidFinishRequestForBillingProductsEvent += OnDidFinishProductsRequest;
        Billing.DidFinishProductPurchaseEvent += OnDidFinishTransaction;
    }

    private void OnDisable()
    {
        // Deregister for callbacks
        Billing.DidFinishRequestForBillingProductsEvent -= OnDidFinishProductsRequest;
        Billing.DidFinishProductPurchaseEvent -= OnDidFinishTransaction;
    }

    private void OnDidFinishProductsRequest(BillingProduct[] _regProductsList, string _error)
    {
        // Hide activity indicator

        // Handle response
        if (_error != null)
        {
            // Something went wrong
        }
        else
        {
            requestedProducts = _regProductsList;

            //If the products did get requested properly - buttons will become active depending on if the products that
            //are setup in the NPSettings are the same as the products set up in the backend(Google Play Console or Apple Developer Console)
            for (int i = 0; i < myProducts.Length; i++)
            {
                for (int j = 0; j < requestedProducts.Length; j++)
                {
                    if (myProducts[i].ProductIdentifier == requestedProducts[j].ProductIdentifier)
                    {
                        storeButtons[i].SetActive(true);
                    }
                }
            }
        }
    }

    //Buy function that will be linked to the OnClick function of the unity button
    //Need to ensure it is setup the same way as the products show up in the NPSettings.
    public void Buy(int whichItem)
    {
        BuyItem(myProducts[whichItem]);
    }

    public void BuyItem (BillingProduct _product)
    {
        
        //For non-consumable items - if it has already been purchased - you can add it back to the user. Does not need to be used for rocket recover at the moment.
        if (NPBinding.Billing.IsProductPurchased(_product))
        {
            
        // Show alert message that item is already purchased

        return;
        }

        // Call method to make purchase
        NPBinding.Billing.BuyProduct(_product);

    // At this point you can display an activity indicator to inform user that task is in progress
    }

    private void OnDidFinishTransaction(BillingTransaction _transaction)
    {
        if (_transaction != null)
        {
            if (_transaction.VerificationState == eBillingTransactionVerificationState.SUCCESS)
            {
                if (_transaction.TransactionState == eBillingTransactionState.PURCHASED)
                {
                    // Your code to handle purchased products
                    switch(_transaction.ProductIdentifier)
                    {
                        case "1000_gears":
                            GearManager.instance.AddGears(1000);
                            prompt.SetPrompt("Gears Purchased!", "You have purchased 1000 Gears.");
                            break;
                        case "5000_gears":
                            GearManager.instance.AddGears(5000);
                            prompt.SetPrompt("Gears Purchased!", "You have purchased 5000 Gears.");
                            break;
                        case "10000_gears":
                            GearManager.instance.AddGears(10000);
                            prompt.SetPrompt("Gears Purchased!", "You have purchased 10000 Gears.");
                            break;
                    }
                }
            }
        }
    }
}
