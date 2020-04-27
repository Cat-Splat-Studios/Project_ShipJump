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
            

        // set certain text colour according to background
        Color textColor;

        if (idx != 0)
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
