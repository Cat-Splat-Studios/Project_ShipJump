/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the player projectile logic
**/

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float lifetime;
    [SerializeField]
    private float speed;

    private Rigidbody rb;
    
    void Start()
    {
        // Find References
        rb = GetComponent<Rigidbody>();

        speed = FindObjectOfType<PlayerMovement>().GetUpSpeed() + speed;

        // Destroy particle after lifetime, will destroy any obstacle in its way
        Destroy(this.gameObject, lifetime);
    }

    void Update()
    {
        rb.velocity = new Vector3(0.0f, speed, 0.0f);
    }
}
