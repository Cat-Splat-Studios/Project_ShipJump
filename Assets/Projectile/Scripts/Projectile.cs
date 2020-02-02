using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    float lifetime;
    [SerializeField]
    float speed;

    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(this.gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0.0f, speed, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "obstacle")
        {
            Destroy(other.gameObject);
        }
    }
}
