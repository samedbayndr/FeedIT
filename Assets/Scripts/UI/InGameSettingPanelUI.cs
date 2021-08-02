using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSettingPanelUI : MonoBehaviour
{
    #region Singleton Things
    private static InGameSettingPanelUI _instance;
    public static InGameSettingPanelUI Instance
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

    public GameObject InGameSettingPanel;

    public void openInGameSettingPanel()
    {
        InGameSettingPanel.SetActive(true);
        GameTimeManager.Instance.stopTime();
    }
    public void closeInGameSettingPanel()
    {
        InGameSettingPanel.SetActive(false);
        GameTimeManager.Instance.startTime();
    }

    
}
