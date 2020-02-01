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
                damage.GotHit(other.gameObject);
                break;
            case "pickup":
                Pickups pickup = other.GetComponent<Pickups>();

                if (pickup)
                {
                    switch (pickup.curPickup)
                    {
                        case Pickups.pickupType.fuelRefill:
                            movement.AddFuel(pickup.GetFuel());
                            Destroy(this.gameObject);
                            break;
                        case Pickups.pickupType.boost:
                            movement.Boost();
                            break;
                        case Pickups.pickupType.sheild:
                            damage.AttatchSheild();
                            Destroy(this.gameObject);
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
