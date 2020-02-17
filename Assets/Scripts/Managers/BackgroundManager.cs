/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: Handles the swaping of Backgrounds
**/

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

    public void Preview(int idx)
    {
        foreach (GameObject mesh in meshes)
        {
            mesh.SetActive(false);
        }

        meshes[idx].SetActive(true);
    }
}
