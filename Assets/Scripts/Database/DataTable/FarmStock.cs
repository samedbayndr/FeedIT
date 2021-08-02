using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmStock : BaseTableClass, IBaseTable<FarmStock>
{
    [NonSerialized]
    public string filePath = "JsonDataFiles/" + TableName.FarmStock;


    public double chickenCoopFeed;
    public double dogFeed;
    public double workerMeal;

    public void Init()
    {
        string jsonString;
        jsonString = FileOperation.LoadTextAsset(filePath, Extension.Json);
        var loadObj = JsonUtility.FromJson<FarmStock>(jsonString);
        Create(loadObj);
    }


    public void Create(FarmStock farmStock)
    {
        if (farmStock == null)
        {
            farmStock = new FarmStock();

        }
        this.chickenCoopFeed = farmStock.chickenCoopFeed;
        this.workerMeal = farmStock.workerMeal;
    }
}