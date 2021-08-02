using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenCoopManager : MonoBehaviour
{
    public int oneBuildCapacity = 10;
    public int currentCapacity;

    public double onePackageFeed = 20;
    FarmStock farmStock;

    public double chickenOneBiteNutrition = 0.05;
    public double roosterOneBiteNutrition = 0.075;


    private static ChickenCoopManager _instance;
    public static ChickenCoopManager Instance
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


    // Start is called before the first frame update
    void Start()
    {
        farmStock = TableManager.Instance.farmStockTable;

        //Bir bina silindiğinde..
        BuildManager.Instance.destroyBuildEvent.AddListener(destroyBuild);

        GameTimeManager.DayFinishedEvent.AddListener(isTherePlaceForAllChickens);

    }

    // Update is called once per frame
    void Update()
    {
    }

    
    public bool areThereCapacity()
    {
        int totalCoopMember = TableManager.Instance.inventoryTable.chicken + TableManager.Instance.inventoryTable.rooster;
        if (totalCoopMember < currentCapacity)
            return true;
        else
            return false;
    }

    public void increaseCapacity()
    {
        currentCapacity += oneBuildCapacity;
    }

    public void decreaseCapacity(int decreaseCapacity)
    {
        currentCapacity -= decreaseCapacity;
    }


    public void destroyBuild(BuildEntity buildEntity)
    {
        if (buildEntity.buildName == BuildName.ChickenCoop)
        {
            decreaseCapacity(ChickenCoopManager.Instance.oneBuildCapacity);
        }
    }


    public void increaseChickenFeed()
    {
        farmStock.chickenCoopFeed += onePackageFeed;
        FileOperation.SaveTextAsset(farmStock, farmStock.filePath, Extension.Json);
    }

    public bool decreaseChickenFeed(double eatenAmount)
    {

        if (farmStock.chickenCoopFeed - eatenAmount > 0)
        {
            farmStock.chickenCoopFeed -= eatenAmount;
            FileOperation.SaveTextAsset(farmStock, farmStock.filePath, Extension.Json);
            return true;
        }

        return false;

    }

    public void isTherePlaceForAllChickens()
    {
        AnimalManager.reCalculateAnimalInventory();

        int coopMemberCount = TableManager.Instance.inventoryTable.chicken + TableManager.Instance.inventoryTable.rooster;

        if (coopMemberCount > ChickenCoopManager.Instance.currentCapacity)
        {
            List<Animal> coopMembers = new List<Animal>();
            coopMembers.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Rooster));
            coopMembers.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Chicken));

            int diff = coopMemberCount - ChickenCoopManager.Instance.currentCapacity;

            for (int i = 0; i < diff; i++)
            {
                coopMembers[i].increaseHomelessAttrition();
            }

            LogPanelUI.Instance.addLog(diff + " coop member left outside");
            Debug.Log(NextGenDebug.HeavyError("Kümes kapasitesinin üstündesin"));
            //int randomCoopMember = UnityEngine.Random.Range(0, coopMemberCount) % coopMemberCount;

        }
    }

}
