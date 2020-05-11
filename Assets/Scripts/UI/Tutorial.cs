/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle the state of the tutorial screen
**/

using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject tutorial;


    public Image foreground;
    public Sprite[] images;

    public Button rightbutton;
    public Button leftbutton;

    public GameObject okayButton;

    public MessageBox prompt;

    private int currentIdx = 0;

    [HideInInspector]
    public bool isEarlyAdopt;
    [HideInInspector]
    public bool hasTut;
    [HideInInspector]
    public int tutNumber;


    private void OnEnable()
    {
        hasTut = PlayerPrefs.HasKey("tutorialV2");
        if (hasTut)
            tutNumber = PlayerPrefs.GetInt("showTutorial", 0);

        if (!hasTut || tutNumber == 1)
        {
            ToggleTutorial(true);

            if (tutNumber == 1)
            {
                prompt.SetPrompt("Early Adopter!", "Thank you for your support! We've completely reset the game and gifted you 10,000 gears for playing version 1");
                GearManager.instance.AddGears(10000);
                NPBinding.CloudServices.SetLong("curGears", GearManager.instance.GetGears());
            }

            PlayerPrefs.SetInt("tutorialV2", 1);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerPrefs.SetInt("showTutorial", 1);
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

            if (tutNumber == 2)
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
}
