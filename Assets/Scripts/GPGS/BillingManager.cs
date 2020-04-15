using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;

public class BillingManager : MonoBehaviour
{

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
            // Inject code to display received products
        }
    }

    public void BuyItem (BillingProduct _product)
    {
        if (NPBinding.Billing.IsProductPurchased(_product.ProductIdentifier))
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
                }
            }
        }
    }
}
