using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerHouseManager : MonoBehaviour
{

    private static WorkerHouseManager _instance;
    public static WorkerHouseManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }

    public int oneBuildCapacity = 1;
    public int currentCapacity;
    private FarmStock farmStock;
    //Kilogram
    private double oneMealStockPackage = 10;
    void Start()
    {
        BuildManager.Instance.destroyBuildEvent.AddListener(destroyBuild);
        farmStock = TableManager.Instance.farmStockTable;

        GameTimeManager.DayFinishedEvent.AddListener(isTherePlaceForAllEmployees);
    }


    public void increaseCapacity()
    {
        currentCapacity += oneBuildCapacity;
    }

    public void decreaseCapacity(int decreaseCapacity)
    {
        currentCapacity -= decreaseCapacity;
    }

    public void increaseMealStock()
    {
        farmStock.workerMeal += oneMealStockPackage;
        FileOperation.SaveTextAsset(farmStock, farmStock.filePath, Extension.Json);
    }

    public bool decreaseMealStock(double eatenAmount)
    {
       
        if (farmStock.workerMeal - eatenAmount > 0)
        {
            farmStock.workerMeal -= eatenAmount;
            FileOperation.SaveTextAsset(farmStock, farmStock.filePath, Extension.Json);
            return true;
        }

        return false;
    }


    public void destroyBuild(BuildEntity buildEntity)
    {
        if (buildEntity.buildName == BuildName.WorkerHouse)
        {
            decreaseCapacity(WorkerHouseManager.Instance.oneBuildCapacity);
        }
    }

    public bool areThereCapacity()
    {
        int totalBarnMember = TableManager.Instance.ownedWorkerTable.AllWorkers.Count;
        if (totalBarnMember < currentCapacity)
            return true;
        else
            return false;
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void isTherePlaceForAllEmployees()
    {
        int workerCount = WorkerManager.Instance.allWorkerList.Count;
        if (workerCount > WorkerHouseManager.Instance.currentCapacity)
        {
            int diff = workerCount - WorkerHouseManager.Instance.currentCapacity;

            for (int i = 0; i < diff; i++)
            {
                WorkerManager.Instance.allWorkerList[i].increaseHomelessAttrition();
            }
            LogPanelUI.Instance.addLog(diff + " worker left outside");
            //int randomWorker = UnityEngine.Random.Range(0, workerCount) % workerCount;


        }
    }
}
