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

    public Text[] textToColour;

    public void SwapIt()
    {

        // Get current player background index and switch to it
        int idx = SwapManager.BackgroundIdx;
       
        foreach(GameObject mesh in meshes)
        {
            mesh.SetActive(false);
        }

        // Check to make sure it is unlock
        if (SwapManager.BackgroundUnlocks.Contains(idx))
            meshes[idx].SetActive(true);
        else
        {
            meshes[0].SetActive(true);
            SwapManager.BackgroundIdx = 0;
        }

        Color textColour;

        if (idx == 0)
            textColour = Color.white;
        else
            textColour = Color.black;

        foreach (var text in textToColour)
            text.color = textColour;
            
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
