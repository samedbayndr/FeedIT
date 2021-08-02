using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : BaseTableClass, IBaseTable<Inventory>
{
    [NonSerialized]
    public string filePath = "JsonDataFiles/" + TableName.Inventory;

    public int sheep;
    public int ram;
    public int lamb;
    public int chicken;
    public int rooster;


    public int egg;

    public int dog;

    //public kilogram things
    public double chickenMeat;
    public double roosterMeat;
    public double ramMeat;
    public double sheepMeat;
    public double wool;


    public void Init()
    {
        string jsonString;
        jsonString = FileOperation.LoadTextAsset(filePath, Extension.Json);
        var loadObj = JsonUtility.FromJson<Inventory>(jsonString);
        Create(loadObj);
    }


    public void Create(Inventory inventory)
    {
        if (inventory == null)
        {
            inventory = new Inventory();

        }
        this.sheep = inventory.sheep;
        this.ram = inventory.ram;
        this.lamb = inventory.lamb;
        this.chicken = inventory.chicken;
        this.rooster = inventory.rooster;


        this.egg = inventory.egg;
        
        this.dog = inventory.dog;

        this.chickenMeat = inventory.chickenMeat;
        this.roosterMeat = inventory.roosterMeat;
        this.ramMeat = inventory.ramMeat;
        this.sheepMeat = inventory.sheepMeat;
        this.wool = inventory.wool;
    }


}