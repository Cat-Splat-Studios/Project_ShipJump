using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatePrompt : MonoBehaviour
{
    public GameObject update;

    public int updateVer;

    // Start is called before the first frame update
    void Start()
    {
        // show the first time the player plays
        if (!PlayerPrefs.HasKey("update") || PlayerPrefs.GetInt("update") != updateVer)
        {
            ToggleUpdate(true);
            PlayerPrefs.SetInt("update", updateVer);
        }
        else
        {
            Debug.Log("Do HAVE");
        }
    }

    public void ToggleUpdate(bool value)
    {
        update.SetActive(value);
    }
}
