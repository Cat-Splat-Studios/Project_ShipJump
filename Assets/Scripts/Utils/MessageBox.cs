
using System;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    [SerializeField]
    private GameObject box;

    [Header("Texts")]
    [SerializeField]
    private Text textTitle;
    [SerializeField]
    private Text textDescription;

    protected Action<bool> onMessageAction;

    public void SetPrompt(string title, string description, Action<bool> callback = null)
    {
        Display();
        textTitle.text = title;
        textDescription.text = description;

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
    }

    protected void Close()
    {
        box.SetActive(false);
    }

}
