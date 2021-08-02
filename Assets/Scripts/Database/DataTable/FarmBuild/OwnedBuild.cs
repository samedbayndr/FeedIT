using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]


public class BuildEntity
{
    
    public string buildName;
    public string id;
    public double maintenanceCost;
    public double buildCost;
    public float posX, posY, posZ;
    public float rotX, rotY, rotZ, rotW;
    public BuildEntity(string buildName, string id, double maintenanceCost, double buildCost, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, float rotW)
    {
        this.buildName = buildName;
        this.id = id;
        this.maintenanceCost = maintenanceCost;
        this.buildCost = buildCost;
        this.posX = posX;
        this.posY = posY;
        this.posZ = posZ;
        this.rotX = rotX;
        this.rotY = rotY;
        this.rotZ = rotZ;
        this.rotW = rotW;
    }





}


public class OwnedBuild : BaseTableClass, IBaseTable<OwnedBuild>
{
    [NonSerialized]
    public string filePath = "JsonDataFiles/" + TableName.OwnedBuild;

    public List<BuildEntity> AllBoughtBuilds;

    public void Init()
    {
        string jsonString;
        jsonString = FileOperation.LoadTextAsset(filePath, Extension.Json);
        var loadObj = JsonUtility.FromJson<OwnedBuild>(jsonString);
        Create(loadObj);
    }


    public void Create(OwnedBuild farmBuild)
    {
        try
        {
            this.AllBoughtBuilds = farmBuild.AllBoughtBuilds;
        }
        catch (Exception e)
        {
            this.AllBoughtBuilds = new List<BuildEntity>();
            Debug.Log("Veri yok!");
        }
       
    }
}
