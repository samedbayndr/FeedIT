using System;
using System.Collections.Generic;
using UnityEngine;



public class WorkerType
{
    public const string NormalWorker = "NormalWorker";
}

[Serializable]
public class WorkerEntity
{
    public string workerId;
    public string workerType;
    public double satiety;
    public double health;
    public double attrition;
    public double salary;
    public int startDayOfWork;
    public double efficiencyModifier;

    public WorkerEntity(string workerId, string workerType, double satiety, double health, double attrition, double salary, int startDayOfWork, double efficiencyModifier)
    {
        this.workerId = workerId;
        this.workerType = workerType;
        this.satiety = satiety;
        this.health = health;
        this.attrition = attrition;
        this.salary = salary;
        this.startDayOfWork = startDayOfWork;
        this.efficiencyModifier = efficiencyModifier;
    }
}


public class OwnedWorker : BaseTableClass, IBaseTable<OwnedWorker>
{
    [NonSerialized]
    public string filePath = "JsonDataFiles/" + TableName.OwnedWorker;

    public List<WorkerEntity> AllWorkers = new List<WorkerEntity>();

    public void Init()
    {
        string jsonString;
        jsonString = FileOperation.LoadTextAsset(filePath, Extension.Json);
        var loadObj = JsonUtility.FromJson<OwnedWorker>(jsonString);
        Create(loadObj);
    }


    public void Create(OwnedWorker ownedWorker)
    {
        if (ownedWorker == null)
        {
            ownedWorker = new OwnedWorker();
        }

        this.AllWorkers = ownedWorker.AllWorkers;

    }
}