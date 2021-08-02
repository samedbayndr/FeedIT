using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildName
{
    public const string SheepBarn = "SheepBarn";
    public const string ChickenCoop = "ChickenCoop";
    public const string DogHouse = "DogHouse";
    public const string WorkerHouse = "WorkerHouse";
}



public class DestroyBuildEvent : UnityEvent<BuildEntity>
{

}


public class BuildManager : MonoBehaviour
{
    private static BuildManager _instance;

    public static BuildManager Instance
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

    public GameObject farmBuildParent;
    public GameObject sheepBarnPrefab;
    public GameObject chickenCoopPrefab;
    public GameObject workerHousePrefab;
    public GameObject dogHousePrefab;

    public DestroyBuildEvent destroyBuildEvent = new DestroyBuildEvent();
    public void Start()
    {
        List<BuildEntity> allBuilds = TableManager.Instance.ownedBuildTable.AllBoughtBuilds;
        if (allBuilds == null)
        {
            return;
        }

        if (allBuilds.Count == 0)
        {
            return;
        }
        for (int i = 0; i < allBuilds.Count; i++)
        {
            BuildEntity spawnableBuild = allBuilds[i];
            switch (spawnableBuild.buildName)
            {
                case BuildName.SheepBarn:
                    GameObject sheepBarnBuild = Instantiate(sheepBarnPrefab,
                        new Vector3(spawnableBuild.posX, spawnableBuild.posY, spawnableBuild.posZ),
                        new Quaternion(spawnableBuild.rotX, spawnableBuild.rotY, spawnableBuild.rotZ,
                            spawnableBuild.rotW), farmBuildParent.transform);
                    Destroy(sheepBarnBuild.transform.GetChild(0).gameObject);

                    //Yaratılan binaya id ve maintenance cost üzerindeki "Build" adlı componente atanıyor.
                    Build currentSBBuildComponent = sheepBarnBuild.GetComponent<Build>();

                    currentSBBuildComponent.id = spawnableBuild.id;
                    currentSBBuildComponent.maintenanceCost = spawnableBuild.maintenanceCost;
                    currentSBBuildComponent.buildCost = spawnableBuild.buildCost;

                    updateBuildCapacity(spawnableBuild.buildName);

                    break;


                case BuildName.ChickenCoop:
                    GameObject chickenCoopBuild = Instantiate(chickenCoopPrefab,
                        new Vector3(spawnableBuild.posX, spawnableBuild.posY, spawnableBuild.posZ),
                        new Quaternion(spawnableBuild.rotX, spawnableBuild.rotY, spawnableBuild.rotZ,
                            spawnableBuild.rotW), farmBuildParent.transform);
                    Destroy(chickenCoopBuild.transform.GetChild(0).gameObject);

                    //Yaratılan binaya id ve maintenance cost üzerindeki "Build" adlı componente atanıyor.
                    Build currentCPBuildComponent = chickenCoopBuild.GetComponent<Build>();
                    currentCPBuildComponent.id = spawnableBuild.id;
                    currentCPBuildComponent.maintenanceCost = spawnableBuild.maintenanceCost;
                    currentCPBuildComponent.buildCost = spawnableBuild.buildCost;
                    updateBuildCapacity(spawnableBuild.buildName);

                    break;
                case BuildName.WorkerHouse:
                    GameObject workerHouseBuild = Instantiate(workerHousePrefab,
                        new Vector3(spawnableBuild.posX, spawnableBuild.posY, spawnableBuild.posZ),
                        new Quaternion(spawnableBuild.rotX, spawnableBuild.rotY, spawnableBuild.rotZ,
                            spawnableBuild.rotW), farmBuildParent.transform);
                    Destroy(workerHouseBuild.transform.GetChild(0).gameObject);

                    //Yaratılan binaya id ve maintenance cost üzerindeki "Build" adlı componente atanıyor.
                    Build currentWHBuildComponent = workerHouseBuild.GetComponent<Build>();
                    currentWHBuildComponent.id = spawnableBuild.id;
                    currentWHBuildComponent.maintenanceCost = spawnableBuild.maintenanceCost;
                    currentWHBuildComponent.buildCost = spawnableBuild.buildCost;
                    updateBuildCapacity(spawnableBuild.buildName);

                    break;
                case BuildName.DogHouse:
                    GameObject dogHouseBuild = Instantiate(dogHousePrefab,
                        new Vector3(spawnableBuild.posX, spawnableBuild.posY, spawnableBuild.posZ),
                        new Quaternion(spawnableBuild.rotX, spawnableBuild.rotY, spawnableBuild.rotZ,
                            spawnableBuild.rotW), farmBuildParent.transform);
                    Destroy(dogHouseBuild.transform.GetChild(0).gameObject);

                    //Yaratılan binaya id ve maintenance cost üzerindeki "Build" adlı componente atanıyor.
                    Build currentDHBuildComponent = dogHouseBuild.GetComponent<Build>();
                    currentDHBuildComponent.id = spawnableBuild.id;
                    currentDHBuildComponent.maintenanceCost = spawnableBuild.maintenanceCost;
                    currentDHBuildComponent.buildCost = spawnableBuild.buildCost;
                    updateBuildCapacity(spawnableBuild.buildName);
                    break;
            }
        }
    }


    float clickCount = 0;
    float lastClicktime = 0;
    float clickdelay = 0.50f;

    public void Update()
    {
        //TODO:BURAYA EĞER KULLANICI İNŞA MODUNDAYSA KOŞULUNU EKLE!!!

        //if (Input.GetMouseButtonUp(1))
        //{
        //    clickCount++;

        //    if (clickCount == 1) lastClicktime = Time.time;

        //    if (clickCount > 1 && Time.time - lastClicktime < clickdelay)
        //    {
        //        clickCount = 0;
        //        lastClicktime = 0;
        //        GameObject build = findBuildGO();
        //        if (build != null)
        //        {
        //            if (removeBuild(build))
        //            {
        
        //            }
        //        }
        //    }
        //    else if (Time.time - lastClicktime > clickdelay) clickCount = 0;

        //}
        if (Input.GetMouseButtonUp(1))
        {
            GameObject build = findBuildGO();
            if (build != null)
            {
                if (removeBuild(build))
                {
                    LogPanelUI.Instance.addLog("You demolished a building");
                }
            }
        }

    }

    public GameObject findBuildGO()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            GameObject hitGO = raycastHit.collider.gameObject;
            if (hitGO.layer == LayerMask.NameToLayer("Build"))
            {
                return hitGO;
            }
        }

        return null;
    }

    public bool removeBuild(GameObject buildGO)
    {
        Build removableBuild = buildGO.GetComponent<Build>();
        if (removableBuild != null)
        {
            for (int i = 0; i < TableManager.Instance.ownedBuildTable.AllBoughtBuilds.Count; i++)
            {
                if (TableManager.Instance.ownedBuildTable.AllBoughtBuilds[i].id == removableBuild.id)
                {
                    Destroy(buildGO);
                    //Destroy Build Event Fire
                    destroyBuildEvent.Invoke(TableManager.Instance.ownedBuildTable.AllBoughtBuilds[i]);

                    TableManager.Instance.ownedBuildTable.AllBoughtBuilds.Remove(TableManager.Instance.ownedBuildTable.AllBoughtBuilds[i]);
                    FileOperation.SaveTextAsset(TableManager.Instance.ownedBuildTable,
                        TableManager.Instance.ownedBuildTable.filePath, Extension.Json);

                    // TODO: BURADA KULLANICIYA SİLDİĞİ BİNA KADAR PARA EKLE!!!
                    Player.Instance.addMoney(removableBuild.buildCost / 2);

                    return true;
                }
            }
        }

        return false;
    }


    public void updateBuildCapacity(string buildName)
    {
        switch (buildName)
        {
            case BuildName.SheepBarn:
                SheepBarnManager.Instance.increaseCapacity();
                break;
            case BuildName.ChickenCoop:
                ChickenCoopManager.Instance.increaseCapacity();
                break;
            case BuildName.WorkerHouse:
                WorkerHouseManager.Instance.increaseCapacity();
                break;
            case BuildName.DogHouse:
                DogHouseManager.Instance.increaseCapacity();
                break;
            default:
                Debug.Log(NextGenDebug.HeavyError("Eklenmeyen bina var!!! Bina Adı: " + buildName));
                break;
        }
    }
}
