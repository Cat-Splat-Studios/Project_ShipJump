/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To collect unused objects
**/

using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "pickup" || other.tag == "obstacle")
        {
            IPoolObject poolobj = other.GetComponent<IPoolObject>();

            if(poolobj != null)
            {
                Pool.RemoveObject(poolobj.GetPoolName(), other.gameObject);
            }
            else
            {
                Debug.Log("UH POH");
            }
            
        }
    }
}
