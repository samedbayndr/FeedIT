using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;

public class Chicken : FarmAnimal
{
    

    /// <summary>
    /// Tavuk döllenmiş ve yumurtlama aşamasında. Eğer isReadyForEggProduction true ise bu flag false olacaktır.
    /// </summary>
    public bool isLayEgg;
    /// <summary>
    /// Döllenmeye hazır mı? Eğer isLayEgg true ise bu flag false olacaktır. 
    /// </summary>
    public bool isReadyForEggProduction;

    public double oneBiteNutrition = 10;



    private Coroutine eatCoroutine;
    private Coroutine layCoroutine;


    void Start()
    {
        GameTimeManager.TenMinuteFinishedEvent.AddListener(checkSatiety);   
        GameTimeManager.MinuteFinishedEvent.AddListener(eat);
        GameTimeManager.DayFinishedEvent.AddListener(layEgg);
        GameTimeManager.TenMinuteFinishedEvent.AddListener(digestAte);
    }



    void Update()
    {
        if (lastState != currentStates.getState())
        {
            switch (lastState)
            {

                //StateList.breeding & StateList.restingOnBarn:
                case StateList.feedingBait:
                    exitFeedingBait();
                    break;
                case StateList.free:
                    exitFree();
                    break;

                //StateList.feedSearching & StateList.pastureAnimalWalking:
                case StateList.producing:
                    exitProducing();
                    break;

              

            }

            //Yeni state alınıyor...
            lastState = currentStates.getState();
            switch (lastState)
            {

                case StateList.feedingBait:

                    break;
                case StateList.free:

                    break;

                case StateList.producing:

                    break;

            }

        }

    }

    #region Exit State Functions

    private void exitProducing()
    {
        Debug.Log("Exit Production State");
    }

    private void exitFree()
    {
        Debug.Log("Exit Free State");
    }

    private void exitFeedingBait()
    {
        Debug.Log("Exit Feeding Bait");
    }


    #endregion


    #region Time based functions

    public void checkSatiety()
    {
            double currentSatiety = this.getSatiety();
            double satietyPercentage = (100 * currentSatiety) / this.maxSatiety;
            if (satietyPercentage <= 20 && currentStates.getState() != StateList.feedingBait)
            {
                Debug.Log(this.gameObject.name + " Yemlendi!!!");

                currentStates.setState(StateList.feedingBait);
            }
            else if (satietyPercentage > 80)
            {
                if (currentStates.getState() == StateList.feedingBait)
                {
                    currentStates.setState(StateList.free);
                }
            }
            
    }

    public void gainWeight(double gainedWeight)
    {
        if (currentWeight + gainedWeight > AnimalMaxWeight.Chicken)
        {
            currentWeight = AnimalMaxWeight.Chicken;
        }
        else
        {
            currentWeight += gainedWeight;
        }
    }

    public void eat()
    {
        if (currentStates.getState() == StateList.feedingBait)
        {
            if (ChickenCoopManager.Instance.decreaseChickenFeed(oneBiteNutrition))
            {
                
                if (this.updateSatiety(oneBiteNutrition))
                {

                    gainWeight((oneBiteNutrition * AnimalMeatEfficiency.Chicken) / 100);
                }

            }
            
        }
    }

    public void digestAte()
    {
        double currentSatiety = this.getSatiety();
        double satietyPercentage = (100 * currentSatiety) / this.maxSatiety;
        if (satietyPercentage > 5)
        {

            double amountOfDigestAte = -oneBiteNutrition;
            if (this.updateSatiety(amountOfDigestAte))
            {
                //Debug.Log(this.gameObject.name + " Sindirdi!!!");
            }
            //Eğer tokluk oranı %5 üstündeyse ve zaten sindirim yaptıysa açlık attritionu bir miktar düşüyor.
            //Kullanıcının ekonomisi zora girdiğinde çiftlik hayvanlarını yaşatacak kadar hesaplama yapıp attritionu kontrol edebilir
            this.decreaseHungryAttrition();
        }
        else if(satietyPercentage <= 5)
        {
            this.increaseHungryAttrition();
        }
        
    }


    public bool fertilize()
    {
        if (isReadyForEggProduction)
        {
            isLayEgg = true;
            isReadyForEggProduction = false;
            return true;
        }
        else
            return false;
    }

    public void layEgg()
    {
        if (isLayEgg == true)
        {
            TableManager.Instance.inventoryTable.egg++;
            FileOperation.SaveTextAsset(TableManager.Instance.inventoryTable,
                TableManager.Instance.inventoryTable.filePath, Extension.Json);
            
            isReadyForEggProduction = true;
            isLayEgg = false;
            
            

        }
    }
    #endregion

    #region State Coroutine



    #endregion

}
