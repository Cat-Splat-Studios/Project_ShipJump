using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSelect : MonoBehaviour
{
    public EMovementOptions moveOption;
    public PlayerManager player;
    public GameObject selectedText;
    public ControlSelect[] otherControls;

    public Button btn;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("moveOption"))
        {
            EMovementOptions playerOption = (EMovementOptions)PlayerPrefs.GetInt("moveOption");
            if (moveOption == playerOption)
            {
                SelectOption();
            }
        }        
        else
        {
            EMovementOptions playerOption = EMovementOptions.TAP;
            if (moveOption == playerOption)
            {
                SelectOption();
                PlayerPrefs.SetInt("moveOption", (int)moveOption);
            }
        }
    }

    public void SelectOption()
    {
        foreach (ControlSelect ctrl in otherControls)
        {
            ctrl.Renable();
        }

        player.PlayerMovement().SetMoveOptions(moveOption);
        btn.interactable = false;
        selectedText.SetActive(true);
    }

    public void Renable()
    {
        selectedText.SetActive(false);
        btn.interactable = true;
    }
}
