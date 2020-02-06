using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerDamage damage;
    private AudioManager audio;

    public AudioClip[] pickupSounds;
    

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        damage = GetComponent<PlayerDamage>();
        audio = FindObjectOfType<AudioManager>();
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
                            //Mathf.Lerp(canvasScript.curFuel, canvasScript.curFuel + fuelAmount, 1);
                            //canvasScript.curFuel += fuelAmount;
                            movement.AddFuel(pickup.GetFuel());
                            audio.PlaySound(pickupSounds[0]);
                            Destroy(other.gameObject);
                            break;
                        case Pickups.pickupType.boost:
                            //do some physics on the player based on boostForce
                            movement.SetBoost();
                            audio.PlaySound(pickupSounds[1]);
                            Destroy(other.gameObject);
                            break;
                        case Pickups.pickupType.sheild:
                            //send reference to the player to activate sheild
                            damage.AttatchSheild();
                            audio.PlaySound(pickupSounds[2]);
                            Destroy(other.gameObject);
                            break;
                        case Pickups.pickupType.coin:
                            movement.AddCoin();
                            audio.PlaySound(pickupSounds[3]);
                            Destroy(other.gameObject);
                            break;
                        case Pickups.pickupType.projectile:
                            movement.EnableShoot();
                            Destroy(other.gameObject);
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
