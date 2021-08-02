using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepBarnManager : MonoBehaviour
{
    public int oneBuildCapacity = 10;
    public int currentCapacity;
    
    private static SheepBarnManager _instance;
    public static SheepBarnManager Instance
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
        if (buildEntity.buildName == BuildName.SheepBarn)
        {
            decreaseCapacity(ChickenCoopManager.Instance.oneBuildCapacity);
        }
    }

    public bool areThereCapacity()
    {
        int totalBarnMember = TableManager.Instance.inventoryTable.sheep + TableManager.Instance.inventoryTable.ram + +TableManager.Instance.inventoryTable.lamb;
        if (totalBarnMember < currentCapacity)
            return true;
        else
            return false;
    }

    void Start()
    {
        BuildManager.Instance.destroyBuildEvent.AddListener(destroyBuild);

        GameTimeManager.DayFinishedEvent.AddListener(isTherePlaceForAllSheeps);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void isTherePlaceForAllSheeps()
    {
        AnimalManager.reCalculateAnimalInventory();

        int sheepBarnMemberCount = TableManager.Instance.inventoryTable.lamb + TableManager.Instance.inventoryTable.ram +
                          TableManager.Instance.inventoryTable.sheep;

        if (sheepBarnMemberCount > SheepBarnManager.Instance.currentCapacity)
        {
            List<Animal> sheepBarnMembers = new List<Animal>();
            sheepBarnMembers.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Sheep));
            sheepBarnMembers.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Ram));
            sheepBarnMembers.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Lamb));
            int diff = sheepBarnMemberCount - SheepBarnManager.Instance.currentCapacity;

            for (int i = 0; i < diff; i++)
            {
                sheepBarnMembers[i].increaseHomelessAttrition();
            }

            LogPanelUI.Instance.addLog(diff + " barn member left outside");
            
            //int randomSheepBarnMember = UnityEngine.Random.Range(0, sheepBarnMemberCount) % sheepBarnMemberCount;


        }
    }
}
