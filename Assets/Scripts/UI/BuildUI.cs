using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    #region Singleton Things
    private static BuildUI _instance;
    public static BuildUI Instance
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

    public GameObject BuildBuyPanel;
    public GameObject buildingDecisionPanel;
    public Button buyBuildMenuBtn;
    public void openBuildBuyPanel()
    {
        if (FarmUI.Instance.openMenu(BuildBuyPanel))
        {
            //--//
        }
        else
        {
            Debug.Log(NextGenDebug.NormalError("Menü yok!! "));
        }
            
    }

}
