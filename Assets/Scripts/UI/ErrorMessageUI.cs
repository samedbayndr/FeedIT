using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorMessageUI : MonoBehaviour
{
    private static ErrorMessageUI _instance;
    public static ErrorMessageUI Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }


    public GameObject errorPanel;
    public TextMeshProUGUI errorMessageText;
    public GameObject okButton;

    public void openErrorPanel(string message)
    {
        if (message != string.Empty)
        {
            prepareErrorMessagePanel(message);
        }
    }

    public void prepareErrorMessagePanel(string message)
    {
        if (message != string.Empty)
        {
            if (errorPanel.activeSelf == false)
            {
                errorPanel.SetActive(true);
            }
            
            errorMessageText.SetText(message);
            
        }
    }

    public void closeErrorPanel()
    {
        errorMessageText.SetText("");
        errorPanel.SetActive(false);
    }
}
