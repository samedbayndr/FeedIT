using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : BaseTableClass, IBaseTable<Economy>
{
    [NonSerialized]
    public string filePath = "JsonDataFiles/" + TableName.Economy;

    public double money;
    public double monthlyExpenses;
    

    public void Init()
    {
        string jsonString;
        jsonString = FileOperation.LoadTextAsset(filePath, Extension.Json);
        var loadObj = JsonUtility.FromJson<Economy>(jsonString);
        Create(loadObj);
    }


    public void Create(Economy economy)
    {
        if (economy == null)
        {
            economy = new Economy();

        }

        this.money = economy.money;
        this.monthlyExpenses = economy.monthlyExpenses;

    }

}