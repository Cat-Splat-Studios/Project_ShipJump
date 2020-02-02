using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSky : MonoBehaviour
{
    private float length, startPOS;
    public GameObject camera;
    public float scrollEffect;

    void Start()
    {
        camera = FindObjectOfType<Camera>().gameObject;
        startPOS = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        float temp = camera.transform.position.y * (1 - scrollEffect);
        float distance = camera.transform.position.y * (scrollEffect);
        Debug.Log(distance + " Distance Value");

        transform.position = new Vector3(transform.position.x, startPOS + distance, transform.position.z);

        if (temp > (startPOS + length))
        {
            startPOS += length;
        }
        else if (temp < (startPOS - length))
        {
            startPOS -= length;
        }
    }
}