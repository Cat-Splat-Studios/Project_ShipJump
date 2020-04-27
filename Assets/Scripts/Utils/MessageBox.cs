/** 
* Author: Matthew Douglas, Hisham Ata
* Purpose: To handle logic of a message box (displaying information to player)
**/

using System;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField]
    private GameObject box;
    [SerializeField]
    private GameObject boxBackground;

    [Header("Texts")]
    [SerializeField]
    private Text textTitle;
    [SerializeField]
    private Text textDescription;


    protected Action<bool> onMessageAction;

    public void SetPrompt(string title, string description, Action<bool> callback = null)
    {
        // Display the message with the correct text
        Display();
        textTitle.text = title;
        textDescription.text = description;

        // Enable callback methods
        if (callback != null)
        {
            onMessageAction = callback;
        }
    }

    public void Confirm()
    {
        if (onMessageAction != null)
        {
            onMessageAction.Invoke(true);
        }

        Close();
    }

    public void Cancel()
    {
        if (onMessageAction != null)
        {
            onMessageAction.Invoke(false);
        }

        Close();
    }

    protected void Display()
    {
        box.SetActive(true);
        boxBackground.SetActive(true);
    }

    protected void Close()
    {
        box.SetActive(false);
        boxBackground.SetActive(false);
    }
}
