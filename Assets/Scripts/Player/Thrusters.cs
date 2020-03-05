/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the state of the thrusters of the current rocket
**/

using UnityEngine;

public class Thrusters : MonoBehaviour
{
    [SerializeField]
    private GameObject boostThrusters;

    [SerializeField]
    private GameObject[] thrusters;

    public void BoostToggle(bool value)
    {
       boostThrusters.SetActive(value);      
    }

    public void ThrusterToggle(bool value)
    {
        foreach(GameObject obj in thrusters)
        {
            obj.SetActive(value);
        }
    }
}
