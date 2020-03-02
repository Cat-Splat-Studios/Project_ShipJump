using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("showTutorial"))
        {
            Debug.Log("DONT HAVE");
            ToggleTutorial(true);
            PlayerPrefs.SetInt("showTutorial", 1);
        }
        else
        {
            Debug.Log("Do HAVE");
        }

       
    }

    public void ToggleTutorial(bool value)
    {
        tutorial.SetActive(value);
    }
}
