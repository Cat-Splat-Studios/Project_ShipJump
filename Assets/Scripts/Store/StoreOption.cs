using UnityEngine;

public class StoreOption : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject content;
    public bool isItemInfo;

    public void ItemCheck()
    {
        foreach(Transform child in content.transform)
        {
            if (isItemInfo)
                child.GetComponent<ItemInfo>().SetPurchase();
            else
                child.GetComponent<RocketStoreCard>().CardCheck();
        }
    }
}
