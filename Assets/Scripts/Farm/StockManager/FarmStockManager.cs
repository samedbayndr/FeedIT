using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FarmStockManager : MonoBehaviour
{
    #region Singleton Things
    private static FarmStockManager _instance;
    public static FarmStockManager Instance
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
            Destroy(this.gameObject);
        }

        _instance = this;
    }
    #endregion

    private FarmStock farmStock;

    public void Start()
    {
        farmStock = TableManager.Instance.farmStockTable;
    }

    public int oneKGWorkerMealCost = 10;
    public void buyWorkerMeal(int buyAmount)
    {
        //TODO:Para kontrolü yap!!
        double cost = buyAmount * oneKGWorkerMealCost;
        if (Player.Instance.getMoney() >= cost)
        {
            //TODO: Parayı düşür
            Player.Instance.decreaseMoney(cost);

            farmStock.workerMeal += buyAmount;
            FileOperation.SaveTextAsset(farmStock, farmStock.filePath, Extension.Json);
        }
        else
        {
            MessageSentenceThrower.Instance.fireMessage("You have not enough money!");
        }
    }

    public int oneKGChickenFeedCost = 5;
    public void buyChickenFeed(int buyAmount)
    {
        double cost = buyAmount * oneKGChickenFeedCost;
        if (Player.Instance.getMoney() >= cost)
        {
            //TODO: Parayı düşür
            Player.Instance.decreaseMoney(cost);
            farmStock.chickenCoopFeed += buyAmount;
            FileOperation.SaveTextAsset(farmStock, farmStock.filePath, Extension.Json);
        }
        else
        {
            MessageSentenceThrower.Instance.fireMessage("You have not enough money!");
        }
    }

    public int oneKGDogFeedCost = 5;
    public void buyDogFeed(int buyAmount)
    {
        double cost = buyAmount * oneKGDogFeedCost;
        if (Player.Instance.getMoney() >= cost)
        {
            //TODO: Parayı düşür
            Player.Instance.decreaseMoney(cost);
            farmStock.dogFeed += buyAmount;
            FileOperation.SaveTextAsset(farmStock, farmStock.filePath, Extension.Json);
        }
        else
        {
            MessageSentenceThrower.Instance.fireMessage("You have not enough money!");
        }
    }

}
