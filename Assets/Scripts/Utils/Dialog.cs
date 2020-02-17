using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Text messageText;
    public Text titleText;
    public void SetError(string title, string message)
    {
        titleText.text = title;
        messageText.text = message;
    }

    public void Confirm()
    {
        this.gameObject.SetActive(false);
    }
}
