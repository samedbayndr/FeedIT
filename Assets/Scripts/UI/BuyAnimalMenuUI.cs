using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyAnimalMenuUI : MonoBehaviour
{
    #region Singleton Things
    private static BuyAnimalMenuUI _instance;
    public static BuyAnimalMenuUI Instance
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

    public GameObject BuyAnimalPanel;
    public void openBuyAnimalPanel()
    {
        if (FarmUI.Instance.openMenu(BuyAnimalPanel))
        {
            //--//
        }
        else
        {
            Debug.Log(NextGenDebug.NormalError("Menü yok!! "));
        }
            
    }

}
