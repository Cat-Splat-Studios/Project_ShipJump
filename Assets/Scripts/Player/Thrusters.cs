using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrusters : MonoBehaviour
{
    [SerializeField]
    private GameObject boostThrusters;

    public void BoostToggle(bool value)
    {
       boostThrusters.SetActive(value);      
    }
}
