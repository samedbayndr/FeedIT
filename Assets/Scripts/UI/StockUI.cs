using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockUI : MonoBehaviour
{
    #region Singleton Things
    private static StockUI _instance;
    public static StockUI Instance
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

    public GameObject StockBuyPanel;
    public void openStockBuyPanel()
    {
        if (FarmUI.Instance.openMenu(StockBuyPanel))
        {
            //--//
        }
        else
        {
            Debug.Log(NextGenDebug.NormalError("Menü yok!! "));
        }

    }
}
