using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour, ISwapper
{
    [Header("Meshes")]
    public GameObject[] meshes;

    void Start()
    {
        SwapIt();
    }

    public void SwapIt()
    {
        foreach(GameObject mesh in meshes)
        {
            mesh.SetActive(false);
        }

        meshes[SwapManager.BackgroundIdx].SetActive(true);
    }

}
