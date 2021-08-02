using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AnimalLocation
{
    onBarn,
    onPasture,
    onDogHouse
}
public enum AnimalSex
{
    male,
    female
}
public static class MagicAnimalType
{
    public const string PastureAnimal = "PastureAnimal"; 
    public const string FarmAnimal = "FarmAnimal";
    
}

public static class AnimalMeatEfficiency
{
    public const double Sheep = 35;
    public const double Ram = 45;
    public const double Chicken = 15;
    public const double Rooster = 15;

    public static double calculateAnimalMeatReward(double weight, double efficiency)
    {
        return ((weight * efficiency) / 100);
    }
}

public static class AnimalMaxWeight
{
    public const double Sheep = 100;
    public const double Ram = 150;
    public const double Chicken = 6;
    public const double Rooster = 10;
}

public class Animal : MonoBehaviour
{
    private const double hungryAttrition = 0.10;
    private const double homelessAttrition = 1;
    public const double maxAttritionLimit = 10;

    public string animalId;
    public AnimalSex sex;
    public AnimalLocation animalLocation;
    public double maxHealth;
    public double currentHealth;

    public double maxSatiety;
    public double currentSatiety;

    public double currentBreedingRate;
    public const double maxBreedingRate = 100;

    public double currentGrowthRate;
    public const double maxGrowthRate = 100;

    public double currentWeight;
    public double currentAttrition;

    //Health Functions
    public double getHealth()
    {
        return currentHealth;
    }

    public void updateHealth(double health)
    {
        this.currentHealth += health;
    }

    public bool isHealthMaximum()
    {
        if (currentHealth == maxHealth) return true;
        else return false;
    }

    public void maximizeHealth()
    {
        currentHealth = maxHealth;
    }

    // Feed Functions
    public double getSatiety()
    {
        return currentSatiety;
    }


    public bool updateSatiety(double food)
    {
        //Eksiye düşmemesi için kontrol
        if (this.currentSatiety + food < 0)
        {
            return false;
        }
        
        this.currentSatiety += food;
        if (this.currentSatiety > maxSatiety)
        {
            maximizeSatiety();
            return false;
        }

        return true;
    }

    public bool isSatietyMaximum()
    {
        if (currentSatiety == maxSatiety) return true;
        else return false;
    }

    public void maximizeSatiety()
    {
        currentSatiety = maxSatiety;
    }


    // Growth Functions
    public double getGrowthRate()
    {
        return currentGrowthRate;
    }

    public void updateGrowthRate(double rate)
    {
        this.currentGrowthRate += rate;
    }

    public bool isGrowthRateMaximum()
    {
        if (currentGrowthRate == maxGrowthRate) return true;
        else return false;
    }

    public void maximizeGrowthRate()
    {
        currentGrowthRate = maxGrowthRate;
    }



    // Breeding Functions

    public double getBreeding()
    {
        return currentBreedingRate;
    }

    public void updateBreeding(double rate)
    {
        this.currentBreedingRate += rate;
    }

    public bool isBreedingRateMaximum()
    {
        if (currentBreedingRate == maxBreedingRate) return true;
        else return false;
    }

    public void maximizeBreedingRate()
    {
        currentBreedingRate = 100;
    }

    
    

    IEnumerator feedIntervalRoutine(double amountOfFood,int feedInterval = 1, int pTime = 10)
    {
        int currentTime = 0;
        if (currentTime >= pTime || amountOfFood <= 0 || feedInterval <= 0)
        {
            Debug.Log(NextGenDebug.NormalError("I'm Hungry!"));
            yield return new WaitForEndOfFrame();
        }
        while (currentTime < pTime)
        {
            updateSatiety(amountOfFood/pTime);
            yield return new WaitForSeconds(feedInterval);
        }
    }


    //Hungry Attrition
    public void increaseHungryAttrition()
    {
        this.currentAttrition += hungryAttrition;
        checkAttritionLimit();
    }

    public void increaseHomelessAttrition()
    {
        currentAttrition += homelessAttrition;
        checkAttritionLimit();

    }

    public void decreaseHungryAttrition()
    {
        if (currentAttrition == 0)
        {
            return;
        }
        this.currentAttrition -= (hungryAttrition * 2);
        if (this.currentAttrition < 0)
        {
            this.currentAttrition = 0;
        }
    }

    public void checkAttritionLimit()
    {
        if (currentAttrition >= maxAttritionLimit)
        {
            killAnimal();
        }
    }

    public void killAnimal()
    {
        removeAnimalEntity();
        Destroy(this.gameObject);
        AnimalManager.Instance.DieAnimalEvent.Invoke();
    }

    void OnApplicationQuit()
    {
        saveAnimalEntity();
    }


    //Refactoring sırasında bu fonksiyonu AnimalManagera al ve her hayvan tipine göre ele al.
    public void saveAnimalEntity()
    {
        AnimalEntity animalEntity =
            TableManager.Instance.ownedAnimalTable.AllAnimals.Find(a => a.animalId == this.animalId);
        
        animalEntity.health = this.currentHealth;
        animalEntity.breedingRate = this.currentBreedingRate;
        animalEntity.growthRate = this.currentGrowthRate;
        animalEntity.satiety = this.currentSatiety;
        animalEntity.weight = this.currentWeight;
        animalEntity.attrition = this.currentAttrition;

        //Refactoring esnasında burası olmaması gerekiyor.
        if (animalEntity.animalType == AnimalType.Sheep || animalEntity.animalType == AnimalType.Ram)
        {
            animalEntity.wool = (this as SheepGroup).wool;
        }

        FileOperation.SaveTextAsset(TableManager.Instance.ownedAnimalTable,
            TableManager.Instance.ownedAnimalTable.filePath, Extension.Json);
    }

    void removeAnimalEntity()
    {
        AnimalEntity animalEntity =
            TableManager.Instance.ownedAnimalTable.AllAnimals.Find(a => a.animalId == this.animalId);
        TableManager.Instance.ownedAnimalTable.AllAnimals.Remove(animalEntity);

        FileOperation.SaveTextAsset(TableManager.Instance.ownedAnimalTable,
            TableManager.Instance.ownedAnimalTable.filePath, Extension.Json);

        LogPanelUI.Instance.addLog(animalEntity.animalType + " die");
        AnimalManager.reCalculateAnimalInventory();
    }
    
}
