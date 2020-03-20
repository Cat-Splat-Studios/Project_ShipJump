/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the state of the tutorial screen
**/

using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial;

    // Start is called before the first frame update
    void Start()
    {
        // show the first time the player plays
        if(!PlayerPrefs.HasKey("showTutorial"))
        {
            ToggleTutorial(true);
            PlayerPrefs.SetInt("showTutorial", 1);
        } 
    }

    public void ToggleTutorial(bool value)
    {
        tutorial.SetActive(value);
    }
}
