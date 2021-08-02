using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Singleton Things

    private static InventoryManager _instance;

    public static InventoryManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }

    #endregion
    private Inventory inventory;
    void Start()
    {
        inventory = TableManager.Instance.inventoryTable;
    }

    #region MEAT CATEGORY

    

    

    public double sheepMeatPrice = 10;
    public void sellSheepMeat(int quantity = 1)
    {
        if (inventory.sheepMeat - quantity < 0)
            return;

        inventory.sheepMeat -= quantity;
        Player.Instance.addMoney(quantity * sheepMeatPrice);
        FileOperation.SaveTextAsset(inventory, inventory.filePath, Extension.Json);

        SellProductUI.Instance.updateSellableInventoryUI();
    }

    public double ramMeatPrice = 10;
    public void sellRamMeat(int quantity = 1)
    {
        if (inventory.ramMeat - quantity < 0)
            return;

        inventory.ramMeat -= quantity;
        Player.Instance.addMoney(quantity * ramMeatPrice);
        FileOperation.SaveTextAsset(inventory, inventory.filePath, Extension.Json);

        SellProductUI.Instance.updateSellableInventoryUI();
    }

    public double chickenMeatPrice = 10;
    public void sellChickenMeat(int quantity = 1)
    {
        if (inventory.chickenMeat - quantity < 0)
            return;

        inventory.chickenMeat -= quantity;
        Player.Instance.addMoney(quantity * chickenMeatPrice);
        FileOperation.SaveTextAsset(inventory, inventory.filePath, Extension.Json);

        SellProductUI.Instance.updateSellableInventoryUI();
    }

    public double roosterMeatPrice = 10;
    public void sellRoosterMeat(int quantity = 1)
    {
        if (inventory.roosterMeat - quantity < 0)
            return;

        inventory.roosterMeat -= quantity;
        Player.Instance.addMoney(quantity * roosterMeatPrice);
        FileOperation.SaveTextAsset(inventory, inventory.filePath, Extension.Json);

        SellProductUI.Instance.updateSellableInventoryUI();
    }

    #endregion

    #region Animal

    public double lambPrice = 150;
    public void sellLamb(int quantity = 1)
    {
        if (inventory.lamb - quantity < 0)
            return;

        inventory.lamb -= quantity;
        Player.Instance.addMoney(quantity * lambPrice);

        //Kuzulardan growthRate en yüksek olanı büyüğünü bul
        Animal biggestLamb = AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Lamb).OrderByDescending(a=>a.currentGrowthRate).ToList()[0];
        biggestLamb.killAnimal();
        
        FileOperation.SaveTextAsset(inventory, inventory.filePath, Extension.Json);

        SellProductUI.Instance.updateSellableInventoryUI();
    }

    #endregion

    #region Animal's Product

    public double eggPrice = 10;
    public void sellEgg(int quantity = 1)
    {
        if (inventory.egg - quantity < 0)
            return;
        inventory.egg -= quantity;
        Player.Instance.addMoney(quantity * eggPrice);
        FileOperation.SaveTextAsset(inventory, inventory.filePath, Extension.Json);

        SellProductUI.Instance.updateSellableInventoryUI();
    }

    public double woolPrice = 10;
    public void sellWool(int quantity = 1)
    {
        if (inventory.wool - quantity < 0)
            return;
        inventory.wool -= quantity;
        Player.Instance.addMoney(quantity * woolPrice);
        FileOperation.SaveTextAsset(inventory, inventory.filePath, Extension.Json);

        SellProductUI.Instance.updateSellableInventoryUI();
    }
    #endregion
}
