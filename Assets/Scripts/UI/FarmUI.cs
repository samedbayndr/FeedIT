using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmUI : MonoBehaviour
{
    #region Singleton Things
    private static FarmUI _instance;
    public static FarmUI Instance
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

    public GameObject[] subMenus;

    public bool openMenu(GameObject menu)
    {
        bool isRelatedMenuOpened = false;
        foreach (var subMenu in subMenus)
        {
            if (menu == subMenu)
            {
                subMenu.SetActive(true);
                isRelatedMenuOpened = true;
            }
            else
                subMenu.SetActive(false);
        }

        return isRelatedMenuOpened;
    }


    public void closeMenu(GameObject menu)
    {
        foreach (var subMenu in subMenus)
        {
            if (subMenu == menu)
            {
                menu.SetActive(false);
                return;
            }
        }
            Debug.Log(NextGenDebug.HeavyError("Öyle bir menü hiç olmadı ki!"));
        
    }
}
