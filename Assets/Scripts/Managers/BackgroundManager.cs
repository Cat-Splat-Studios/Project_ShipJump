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
        int idx = 0;
        if(PlayerPrefs.HasKey("backgroundIdx"))
        {
            idx = PlayerPrefs.GetInt("backgroundIdx");
        }
        else
        {
            idx = SwapManager.BackgroundIdx;
        }

        foreach(GameObject mesh in meshes)
        {
            mesh.SetActive(false);
        }

        meshes[idx].SetActive(true);

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
