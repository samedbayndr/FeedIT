using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : BaseTableClass, IBaseTable<Lifetime>
{
    [NonSerialized]
    public string filePath = "JsonDataFiles/" + TableName.Lifetime;


    public int day;
    public int hour;
    public int minute;

    public void Init()
    {
        string jsonString;
        jsonString = FileOperation.LoadTextAsset(filePath, Extension.Json);
        var loadObj = JsonUtility.FromJson<Lifetime>(jsonString);
        Create(loadObj);
    }


    public void Create(Lifetime lifetime)
    {
        if (lifetime == null)
        {
            lifetime = new Lifetime();
            
        }
        this.day = lifetime.day;
        this.hour = lifetime.hour;
        this.minute = lifetime.minute;
    }
}