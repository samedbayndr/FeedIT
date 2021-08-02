using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogHouseManager : MonoBehaviour
{

    private static DogHouseManager _instance;
    public static DogHouseManager Instance
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

        GameTimeManager.DayFinishedEvent.AddListener(isTherePlaceForAllDogs);
    }


    public void increaseCapacity()
    {
        currentCapacity += oneBuildCapacity;
    }

    public void decreaseCapacity(int decreaseCapacity)
    {
        currentCapacity -= decreaseCapacity;
    }

    public void increaseFeedStock()
    {
        farmStock.dogFeed += oneMealStockPackage;
        FileOperation.SaveTextAsset(farmStock, farmStock.filePath, Extension.Json);
    }

    public bool decreaseFeedStock(double eatenAmount)
    {
       
        if (farmStock.dogFeed - eatenAmount > 0)
        {
            farmStock.dogFeed -= eatenAmount;
            FileOperation.SaveTextAsset(farmStock, farmStock.filePath, Extension.Json);
            return true;
        }

        return false;
    }


    public void destroyBuild(BuildEntity buildEntity)
    {
        if (buildEntity.buildName == BuildName.DogHouse)
        {
            decreaseCapacity(WorkerHouseManager.Instance.oneBuildCapacity);
        }
    }

    public bool areThereCapacity()
    {
        int totalDogHouseMember = TableManager.Instance.inventoryTable.dog;
        if (totalDogHouseMember < currentCapacity)
            return true;
        else
            return false;
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void isTherePlaceForAllDogs()
    {
        AnimalManager.reCalculateAnimalInventory();

        int dogHouseCount = TableManager.Instance.inventoryTable.dog;

        if (dogHouseCount > this.currentCapacity)
        {
            List<Animal> dogHouseMembers = new List<Animal>();
            dogHouseMembers.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Kangal));
            

            int diff = dogHouseCount - this.currentCapacity;

            for (int i = 0; i < diff; i++)
            {
                dogHouseMembers[i].increaseHomelessAttrition();
            }

            LogPanelUI.Instance.addLog(diff + " dog left outside");
            Debug.Log(NextGenDebug.HeavyError("Kümes kapasitesinin üstündesin"));
            //int randomCoopMember = UnityEngine.Random.Range(0, coopMemberCount) % coopMemberCount;

        }
    }
}
