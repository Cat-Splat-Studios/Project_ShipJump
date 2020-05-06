/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the state of the tutorial screen
**/

using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial;

    public MessageBox prompt;

    public Image foreground;
    public Sprite[] images;

    public Button rightbutton;
    public Button leftbutton;

    public GameObject okayButton;

    private int currentIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        // show the first time the player plays
        if(!PlayerPrefs.HasKey("showTutorial") || PlayerPrefs.GetInt("showTutorial") == 1)
        {
            ToggleTutorial(true);
            PlayerPrefs.SetInt("showTutorial", 2);
        }
        
        if(PlayerPrefs.GetInt("showTutorial") == 1)
        {
            prompt.SetPrompt("Early Adopter!", "Thank you for being an early supporter! We've completely reset the game and gifted you 10,000 gears for supporting us!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.DeleteKey("showTutorial");
        }
    }

    public void ToggleTutorial(bool value)
    {
        tutorial.SetActive(value);

        if(value)
        {
            currentIdx = 0;
            foreground.sprite = images[currentIdx];
            leftbutton.interactable = false;
            rightbutton.interactable = true;

            if (PlayerPrefs.GetInt("showTutorial") == 2)
                okayButton.SetActive(true);
        }
    }

    public void Next()
    {
        if (currentIdx + 1 < images.Length)
        {
            currentIdx++;         
            foreground.sprite = images[currentIdx];

            if (currentIdx == 1)
                leftbutton.interactable = true;

            if (currentIdx == images.Length - 1)
            {
                rightbutton.interactable = false;
                if (!okayButton.activeSelf)
                    okayButton.SetActive(true);
            }
                
        }
    }

    public void Prev()
    {
        if (currentIdx - 1 >= 0)
        {
            currentIdx--;
            foreground.sprite = images[currentIdx];

            if (currentIdx == 0)
                leftbutton.interactable = false;

            if (currentIdx == images.Length - 2)
            {
                rightbutton.interactable = true;
            }

        }
    }

    private void EarlyAdopt(bool success)
    {
        if(success)
        {
            GearManager.instance.AddGears(10000);
        }
    }
}
