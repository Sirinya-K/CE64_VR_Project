using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject normalButtons;
    public GameObject capsButtons;
    public bool caps;

    void Start()
    {
        caps = false;
    }

    public string GetChar()
    {
        string input = inputField.text;
        Debug.Log(input);

        return input;
    }

    public void InsertChar(string c)
    {
        if(inputField.text.Length < 10)
        {
            inputField.text += c;
        }
    }

    public void DeleteChar()
    {
        if(inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    public void InsertSpace()
    {
        inputField.text += " ";
    }

    public void CapsPressed()
    {
        if(!caps)
        {
            normalButtons.SetActive(false);
            capsButtons.SetActive(true);
            caps = true;
        }
        else
        {
            normalButtons.SetActive(true);
            capsButtons.SetActive(false);
            caps = false;
        }
    }
}
