using System.Collections;
using System.Collections.Generic;
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
