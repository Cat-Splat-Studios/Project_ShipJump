/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the player projectile logic
**/

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private float lifetime;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody rb;
    
    void Start()
    {
        // Set speed based on how fast the player is going to make sure its going faster and shoots forward
        speed = FindObjectOfType<PlayerMovement>().GetUpSpeed() + speed;

        // Destroy particle after lifetime, will destroy any obstacle in its way
        Destroy(this.gameObject, lifetime);
    }

    void Update()
    {
        rb.velocity = new Vector3(0.0f, speed, 0.0f);
    }
}
