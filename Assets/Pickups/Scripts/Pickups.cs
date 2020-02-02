using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public enum pickupType
    {
        fuelRefill,
        sheild,
        boost,
        coin,
        slowdown
    }

    [SerializeField]
    public pickupType curPickup;

    [SerializeField]
    UIDelgate canvasScript;

    [SerializeField]
    float fuelAmount = 10.0f;

    bool sheildActive;

    [SerializeField]
    float boostForce;

    [SerializeField]
    int slowForce;


    // Start is called before the first frame update
    void Start()
    {
      //  canvasScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UIDelgate>();
    }

    // Update is called once per frame
  
    public float GetFuel()
    {
        return fuelAmount;
    }

    public float GetBoost()
    {
        return boostForce;
    }
}
