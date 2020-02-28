/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: Handles the swaping of Backgrounds
**/

using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour, ISwapper
{
    [Header("Meshes")]
    public GameObject[] meshes;

    public Text[] textColorChanging;

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

        Color textColor;

        if (SwapManager.BackgroundIdx != 0)
            textColor = Color.black; 
        else
            textColor = Color.white;

        foreach(Text text in textColorChanging)
        {
            text.color = textColor;
        }
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
