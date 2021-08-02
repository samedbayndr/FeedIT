using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region Singleton Things
    private static UIManager _instance;
    public static UIManager Instance
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

    public void OnEnabled()
    {
        
        //Grassland Events Subscription
        //Grassland.GrasslandGuestEvent.AddListener(enterGrassland);
        //Grassland.GrasslandGuestLeaveEvent.AddListener(exitGrassland);
    }


    public GameObject FarmUIGO;
    public GameObject AnimalUIGO;

    //FarmUI


    //GrasslandUI
    





    //Animal UI
}
