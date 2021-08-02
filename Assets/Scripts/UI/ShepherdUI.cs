using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShepherdUI : MonoBehaviour
{
    #region Singleton Things
    private static ShepherdUI _instance;
    public static ShepherdUI Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
    }
    #endregion

    public GameObject ShepherdUIPanel;
    public Button goRestBtn;
    public Button blowWhistleBtn;

    public void Start()
    {
    }

    public void openShepherdUI()
    {
        goRestBtn.interactable = false;
        ShepherdUIPanel.SetActive(true);
    }

    public void closeShepherdUI()
    {
        goRestBtn.interactable = false;
        ShepherdUIPanel.SetActive(false);
    }
}
