using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;


[DefaultExecutionOrder(-10000)]
public class TableManager : MonoBehaviour
{
    // Singleton pattern..
    private static TableManager _instance = null;
    public static TableManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                Debug.LogError("TableManager.Instance NULL !!!");

                return _instance;
            }
        }
    }

    [HideInInspector]
    public List<BaseTableClass> allTableObjects;
    public Dictionary<BaseTableClass, string> tableNameDictionary;
    public OwnedBuild ownedBuildTable;
    public OwnedAnimal ownedAnimalTable;
    public OwnedWorker ownedWorkerTable;
    public Lifetime lifetimeTable;
    public Inventory inventoryTable;
    public FarmStock farmStockTable;
    public Economy economyTable;
    public EstateAsset estateAssetTable;
    public AudioSetting audioSettingTable;



    public void Awake()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

        #region Singleton Kodları
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }


        _instance = this;
        #endregion

        createTableInstance();

    }

    public void createTableInstance()
    {
        ownedBuildTable = new OwnedBuild();
        ownedBuildTable.Init();

        ownedAnimalTable = new OwnedAnimal();
        ownedAnimalTable.Init();

        ownedWorkerTable = new OwnedWorker();
        ownedWorkerTable.Init();

        lifetimeTable = new Lifetime();
        lifetimeTable.Init();

        inventoryTable = new Inventory();
        inventoryTable.Init();

        farmStockTable = new FarmStock();
        farmStockTable.Init();

        economyTable = new Economy();
        economyTable.Init();

        estateAssetTable = new EstateAsset();
        estateAssetTable.Init();


        audioSettingTable = new AudioSetting();
        audioSettingTable.Init();


        allTableObjects = new List<BaseTableClass>
        {
            ownedBuildTable,
            ownedAnimalTable,
            ownedWorkerTable,
            lifetimeTable,
            inventoryTable,
            farmStockTable,
            economyTable,
            estateAssetTable,
            audioSettingTable
        };

        tableNameDictionary = new Dictionary<BaseTableClass, string>
        {
            {ownedBuildTable,TableName.OwnedBuild },
            {ownedAnimalTable,TableName.OwnedAnimal},
            {ownedWorkerTable,TableName.OwnedWorker},
            {lifetimeTable,TableName.Lifetime},
            {inventoryTable,TableName.Inventory},
            {farmStockTable,TableName.FarmStock},
            {economyTable,TableName.Economy},
            {estateAssetTable,TableName.EstateAsset},
            {audioSettingTable,TableName.AudioSetting}

        };
    }

    public void saveAllTable()
    { 
        int tableNameClassPropertyCount = typeof(TableName).GetProperties().Length;

        if (tableNameDictionary.Count != allTableObjects.Count && tableNameDictionary.Count != tableNameClassPropertyCount && allTableObjects.Count != tableNameClassPropertyCount)
        {
            Debug.LogError("ERROR!! ERROR!! ERROR!! TableNameDictionary.Count, allTableObject.Count, tableNameClassPropertyCount sayıları arasında uyuşmazlık var!");
            Debug.LogError("ERROR!! ERROR!! ERROR!! TableNameDictionary.Count, allTableObject.Count, tableNameClassPropertyCount sayıları arasında uyuşmazlık var!");
            Debug.LogError("ERROR!! ERROR!! ERROR!! TableNameDictionary.Count, allTableObject.Count, tableNameClassPropertyCount sayıları arasında uyuşmazlık var!");
            Thread.Sleep(100000);
        }

        for (int i = 0; i < allTableObjects.Count; i++)
        {
            string tableName;
            if (tableNameDictionary.TryGetValue(allTableObjects[i],out tableName)) { }
            else
                Debug.Log("Tablo adı tableName Dictionaryde mevcut değil!");

            FileOperation.SaveTextAssetWithBaseTableClass(allTableObjects[i], "JsonDataFiles/" + tableName, Extension.Json);
        }
    }
}

public static class TableName
{
    public const string OwnedBuild = "OwnedBuild";
    public const string OwnedAnimal = "OwnedAnimal";
    public const string OwnedWorker = "OwnedWorker";
    public const string Lifetime = "Lifetime";
    public const string Inventory = "Inventory";
    public const string FarmStock = "FarmStock";
    public const string Economy = "Economy";
    public const string EstateAsset = "EstateAsset";
    public const string AudioSetting = "AudioSetting";



}