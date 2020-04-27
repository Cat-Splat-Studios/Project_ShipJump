/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle all of the player collisions
**/

using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Pickup Sounds")]
    public AudioClip[] pickupSounds;

    public float pickupVol = 0.5f;

    public Vector3 smallHitBox;
    public Vector3 largeHitBox;

    private PlayerManager player;
    private new AudioManager audio;
    private BoxCollider boxCollide;

    private void Start()
    {
        // Find References
        player = GetComponent<PlayerManager>();
        audio = FindObjectOfType<AudioManager>();
        boxCollide = GetComponent<BoxCollider>();
    }

    public void SetHitBox(bool isLarge)
    {
        // Adjust collider depending on rocket size
        if(isLarge)
        {
            boxCollide.size = largeHitBox;
        }
        else
        {
            boxCollide.size = smallHitBox;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "obstacle":
                player.PlayerDamage().GotHit(other.gameObject);
                break;
            case "pickup":
                Pickups pickup = other.GetComponent<Pickups>();
                if (pickup)
                {
                    switch (pickup.GetPickupType())
                    {
                        case EPickupType.FUEL:
                            // Add fuel to player
                            player.Fuel().AddFuel(pickup.GetFuel());
                            audio.PlaySound(pickupSounds[0], pickupVol);
                            DestroyObject(other.gameObject);
                            break;
                        case EPickupType.BOOST:
                            // Do some physics on the player based on boostForce
                            player.Boost().BoostOn();
                            player.SetBoost(true);
                            audio.PlaySound(pickupSounds[1], 0.8f);
                            DestroyObject(other.gameObject);
                            break;
                        case EPickupType.SHIELD:
                            // Send reference to the player to activate sheild
                            player.PlayerDamage().AttatchShield();
                            audio.PlaySound(pickupSounds[2], pickupVol);
                            DestroyObject(other.gameObject);
                            break;
                        case EPickupType.GEAR:
                            // Add gears
                            GearManager.instance.IncrementGears();
                            audio.PlaySound(pickupSounds[3], 0.35f);
                            DestroyObject(other.gameObject);
                            break;
                        case EPickupType.PROJECTILE:
                            // Allow player to shoot
                            player.PlayerShoot().EnableShoot();
                            audio.PlaySound(pickupSounds[4], 0.65f);
                            DestroyObject(other.gameObject);
                            break;
                    }
                }
                break;
        }   
    }

    private void DestroyObject(GameObject obj)
    {
        // Destroy the object collected
        IPoolObject poolObj = obj.GetComponent<IPoolObject>();

        if (poolObj != null)
        {
            Pool.RemoveObject(poolObj.GetPoolName(), obj);
        }
    }
}
