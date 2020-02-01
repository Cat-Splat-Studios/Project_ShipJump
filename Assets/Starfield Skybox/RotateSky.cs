using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class RotateSky : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    MeshRenderer background;

    void Start()
    {
        background = GetComponent<MeshRenderer>();
        Debug.Log(background);
    }

    void Update()
    {
        Vector2 offset = background.material.mainTextureOffset;

        offset.y += Time.deltaTime * rotateSpeed;

        background.material.mainTextureOffset = offset;
    }
}
