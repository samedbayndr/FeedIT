using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    #region Singleton Things
    private static Player _instance;
    public static Player Instance
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

    
    public UnityEvent moneyNotificationEvent;
    private Economy economy;


    public Economy Economy
    {
        get
        {
            return economy;
        }
        set
        {
            economy = value;
        }
    }

    public void Start()
    {
        #region Money related things
        economy = TableManager.Instance.economyTable;
        moneyNotificationEvent.AddListener(updateMoneyOnDatabase);
        moneyNotificationEvent.AddListener(GeneralGameUIManager.Instance.updateMoneyUI);
        moneyNotificationEvent.Invoke();
        #endregion
    }

    #region Money related functions
    private void updateMoneyOnDatabase()
    {
        FileOperation.SaveTextAsset(TableManager.Instance.economyTable, TableManager.Instance.economyTable.filePath,
            Extension.Json);
    }

    public double getMoney()
    {
        return Economy.money;
    }



    public bool addMoney(double amount)
    {
        if (amount == 0)
        {
            Debug.Log(NextGenDebug.HeavyError("Bir yerde, bir şeylerin kazancı 0 olmalı!!!!"));
            return false;
        }
        if (Player.Instance.Economy.money + amount < 0)
            return false;

        Player.Instance.Economy.money += amount;
        moneyNotificationEvent.Invoke();
        GeneralGameUIManager.Instance.fireMoneyNotification(amount);

        Truck.Instance.Income += amount;
        return true;
    }

    public bool decreaseMoney(double amount)
    {
        if (amount == 0)
        {
            Debug.Log(NextGenDebug.HeavyError("Bir yerde, bir şeylerin costu 0 olmalı!!!!"));
            return false;
        }
        if (Player.Instance.Economy.money - amount < 0)
            return false;

        Player.Instance.Economy.money -= amount;
        moneyNotificationEvent.Invoke();
        GeneralGameUIManager.Instance.fireMoneyNotification(-amount);

        Truck.Instance.Outcome += amount;
        return true;
    }

    #endregion

    #region Camera related functions

     

    #endregion



}
