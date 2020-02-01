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
        slowdown
    }

    [SerializeField]
    public pickupType curPickup;

    [SerializeField]
    UIDelgate canvasScript;

    [SerializeField]
    float fuelAmount;

    bool sheildActive;

    [SerializeField]
    int boostForce;

    [SerializeField]
    int slowForce;


    // Start is called before the first frame update
    void Start()
    {
        canvasScript = GameObject.FindGameObjectWithTag("UI").GetComponent<UIDelgate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            switch (curPickup)
            {
                case pickupType.fuelRefill:
                    //Mathf.Lerp(canvasScript.curFuel, canvasScript.curFuel + fuelAmount, 1);
                    canvasScript.curFuel += fuelAmount;
                    Destroy(this.gameObject);
                    break;
                case pickupType.boost:
                    //do some physics on the player based on boostForce
                    break;
                case pickupType.sheild:
                    //send reference to the player to activate sheild
                    break;
                case pickupType.slowdown:
                    //do some physics on the player based on slowForce;
                    break;
            }
        }
        
    }
}
