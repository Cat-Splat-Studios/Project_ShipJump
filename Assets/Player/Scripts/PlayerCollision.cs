using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerDamage damage;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        damage = GetComponent<PlayerDamage>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "obstacle":
                damage.GotHit();
                break;
            case "pickup":
                Pickups pickup = other.GetComponent<Pickups>();

                if (pickup)
                {
                    switch (pickup.curPickup)
                    {
                        case Pickups.pickupType.fuelRefill:
                            //Mathf.Lerp(canvasScript.curFuel, canvasScript.curFuel + fuelAmount, 1);
                            //canvasScript.curFuel += fuelAmount;
                            Destroy(this.gameObject);
                            break;
                        case Pickups.pickupType.boost:
                            //do some physics on the player based on boostForce
                            break;
                        case Pickups.pickupType.sheild:
                            //send reference to the player to activate sheild
                            break;
                        case Pickups.pickupType.slowdown:
                            //do some physics on the player based on slowForce;
                            break;
                    }
                }
                break;
        }   
    }
}
