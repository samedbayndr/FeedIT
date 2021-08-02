using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AnimalType
{
    public const string Ram = "Ram";
    public const string Sheep = "Sheep";
    public const string Lamb = "Lamb";
    public const string Chicken = "Chicken";
    public const string Rooster = "Rooster";
    public const string Kangal = "Kangal";

}

[Serializable]
public class AnimalEntity
{
    public string animalId;
    public string animalType;
    public int sex;
    public double breedingRate;
    public double growthRate;
    public double satiety;
    public double health;
    public double weight;
    public double attrition;
    public double wool;
    public AnimalEntity(string animalId, string animalType, int sex, double breedingRate, double growthRate, double satiety, double health, double weight, double attrition)
    {
        this.animalId = animalId;
        this.animalType = animalType;
        this.sex = sex;
        this.breedingRate = breedingRate;
        this.growthRate = growthRate;
        this.satiety = satiety;
        this.health = health;
        this.weight = weight;
        this.attrition = attrition;
    }
}


public class OwnedAnimal : BaseTableClass, IBaseTable<OwnedAnimal>
{
    [NonSerialized]
    public string filePath = "JsonDataFiles/" + TableName.OwnedAnimal;

    public List<AnimalEntity> AllAnimals = new List<AnimalEntity>();

    public void Init()
    {
        string jsonString;
        jsonString = FileOperation.LoadTextAsset(filePath, Extension.Json);
        var loadObj = JsonUtility.FromJson<OwnedAnimal>(jsonString);
        Create(loadObj);
    }


    public void Create(OwnedAnimal ownedAnimal)
    {
        if (ownedAnimal == null)
        {
            ownedAnimal = new OwnedAnimal();
        }

        this.AllAnimals = ownedAnimal.AllAnimals;

    }
}