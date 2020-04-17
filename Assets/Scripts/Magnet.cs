using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public GameObject player;
    private void Update()
    {
        transform.position = player.transform.position;
    }

    public void Done()
    {
        transform.position = Vector3.zero;
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "pickup")
        {
            other.gameObject.GetComponent<Pickups>().Pull(player);
        }
    }
}
