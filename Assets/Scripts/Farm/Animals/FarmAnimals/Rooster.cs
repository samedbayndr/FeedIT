using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rooster : FarmAnimal
{
    public double oneBiteNutrition = 30;

    public int maxFertiliazePower = 8;
    public int currentFertiliazePower;
    void Start()
    {
        GameTimeManager.TenMinuteFinishedEvent.AddListener(checkSatiety);
        GameTimeManager.MinuteFinishedEvent.AddListener(eat);
        GameTimeManager.DayFinishedEvent.AddListener(resetFertiliazePower);
        GameTimeManager.MorningEightEvent.AddListener(impregnate);
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

    
    public void checkSatiety()
    {
        double currentSatiety = this.getSatiety();
        double satietyPercentage = (100 * currentSatiety) / this.maxSatiety;
        if (satietyPercentage <= 20)
        {
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
        if (currentWeight + gainedWeight > AnimalMaxWeight.Rooster)
        {
            currentWeight = AnimalMaxWeight.Rooster;

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
                    gainWeight((oneBiteNutrition * AnimalMeatEfficiency.Rooster) / 100);
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
        else if (satietyPercentage <= 5)
        {
            this.increaseHungryAttrition();
        }

    }

    

    public void impregnate()
    {
        List<Chicken> availableChickenList = AnimalManager.Instance.getFertilizableChickens();
        for (int i = 0; i < availableChickenList.Count; i++)
        {
            if (currentFertiliazePower > 0)
            {
                availableChickenList[i].fertilize();
                currentFertiliazePower--;
            }
        }
    }

    public void resetFertiliazePower()
    {
        currentFertiliazePower = maxFertiliazePower;
    }

}
