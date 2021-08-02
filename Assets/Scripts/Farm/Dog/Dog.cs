using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dog : Animal
{

    public State currentStates { get; set; } = new State();
    public StateList lastState;
    public double oneBiteNutrition = 10;
    public NavMeshAgent dogAgent;

    public float stopSpeed = 0f;
    public float walkingSpeed = 10f;
    protected bool isOnTheWay { get; set; }

    protected virtual void Start()
    {
        dogAgent = GetComponent<NavMeshAgent>();
        animalLocation = AnimalLocation.onDogHouse;

        lastState = StateList.free;
        currentStates.setState(StateList.free);


        GameTimeManager.TenMinuteFinishedEvent.AddListener(checkSatiety);
        GameTimeManager.MinuteFinishedEvent.AddListener(eat);
        GameTimeManager.TenMinuteFinishedEvent.AddListener(digestAte);
    }

    public void setPosition(Vector3 newPos)
    {
        dogAgent.enabled = false;
        this.transform.position = newPos;
        dogAgent.enabled = true;

    }


    public void goToRest(Vector3 restPosition)
    {
        dogAgent.ResetPath();
        dogAgent.isStopped = true;
        setPosition(restPosition);
        currentStates.setState(StateList.free);
    }

    public void checkSatiety()
    {
        double currentSatiety = this.getSatiety();
        double satietyPercentage = (100 * currentSatiety) / this.maxSatiety;
        if (satietyPercentage <= 20 && currentStates.getState() != StateList.feedingBait)
        {
            //Debug.Log(this.gameObject.name + " Yemlendi!!!");

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

    public void digestAte()
    {
        double currentSatiety = this.getSatiety();
        double satietyPercentage = (100 * currentSatiety) / this.maxHealth;

        if (satietyPercentage > 5)
        {
            //Otlakta daha yavaş acıkıyor...
            double amountOfDigestAte = 0d;
            if (this.animalLocation == AnimalLocation.onPasture)
            {
                amountOfDigestAte = (-oneBiteNutrition) / 8;
            }
            else if (this.animalLocation == AnimalLocation.onDogHouse)
            {
                amountOfDigestAte = -oneBiteNutrition;
            }
            if (this.updateSatiety(amountOfDigestAte))
            {
                //Debug.Log(this.gameObject.name + " Sindirdi!!!");
            }

            this.decreaseHungryAttrition();
        }
        else if (satietyPercentage <= 5)
        {
            this.increaseHungryAttrition();
        }
    }

    public void eat()
    {
        if (currentStates.getState() == StateList.feedingBait && animalLocation == AnimalLocation.onDogHouse)
        {
            if (DogHouseManager.Instance.decreaseFeedStock(oneBiteNutrition))
            {
                if (this.updateSatiety(oneBiteNutrition))
                {

                }
            }

        }
    }

}
