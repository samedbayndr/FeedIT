using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstateAsset : BaseTableClass, IBaseTable<EstateAsset>
{
    [NonSerialized]
    public string filePath = "JsonDataFiles/" + TableName.EstateAsset;

    public bool Area1 = false;
    public bool Area2 = false;
    public bool Area3 = false;


    public void Init()
    {
        string jsonString;
        jsonString = FileOperation.LoadTextAsset(filePath, Extension.Json);
        var loadObj = JsonUtility.FromJson<EstateAsset>(jsonString);
        Create(loadObj);
    }


    public void Create(EstateAsset estateAsset)
    {
        if (estateAsset == null)
        {
            estateAsset = new EstateAsset();

        }

        this.Area1 = estateAsset.Area1; 
        this.Area2 = estateAsset.Area2;
        this.Area3 = estateAsset.Area3;

    }

}