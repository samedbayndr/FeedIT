using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class FinishedGrassEvent : UnityEvent<Tussock>
{

}
public class Tussock : MonoBehaviour
{
    public BoxCollider tussockCollider;

    public double maxHealth = 1000;
    public int maxVisitor = 5;

    public List<Grass> childGrass;
    public double currentHealth;

    public Coroutine FeedVisitorCoroutine;
    public Coroutine UpdateTussockCoroutine;

    //Events
    public static UnityEvent UpdateGrasslandEvent = new UnityEvent();
    public FinishedGrassEvent finishedGrassEvent = new FinishedGrassEvent();

    public List<PastureAnimal> visitorAnimals = new List<PastureAnimal>();

    public void Start()
    {

        tussockCollider = this.GetComponent<BoxCollider>();

        maximizeHealth();

        if (childGrass.Count == 0)
            Debug.Log(NextGenDebug.HeavyError("Child grasses not setted!!"));

        FeedVisitorCoroutine = StartCoroutine(feedVisitorRoutine());
        UpdateTussockCoroutine = StartCoroutine(updateTussockRoutine());
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(MagicAnimalType.PastureAnimal))
        {
            //TODO: Hayvanları visitor olarak kabul et yada başka bir otlak bulması için yönlendir.
            

            PastureAnimal visitorAnimal = collider.GetComponent<PastureAnimal>();
            if (visitorAnimal.currentStates.getState() != StateList.shepherdFollowing)
                AddVisitor(visitorAnimal);
        }
        else
        {
            Debug.Log(NextGenDebug.NormalError("It is not Animal!!"));
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(MagicAnimalType.PastureAnimal))
        {

            PastureAnimal visitorAnimal = isVisitorExist(collider.GetComponent<PastureAnimal>());
            if (visitorAnimal != null)
                AddVisitor(visitorAnimal);
        }
        else
        {
            Debug.Log(NextGenDebug.NormalError("It is not Animal!!"));
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(MagicAnimalType.PastureAnimal))
        {

            PastureAnimal visitorAnimal = collider.GetComponent<PastureAnimal>();
            visitorAnimals.Remove(visitorAnimal);
            finishedGrassEvent.RemoveListener(visitorAnimal.exitFromTussock);
        }
        else
        {
            Debug.Log(NextGenDebug.NormalError("It is not Animal!!"));
        }
    }

    public PastureAnimal isVisitorExist(PastureAnimal pastureAnimal)
    {
        if (visitorAnimals.Contains(pastureAnimal))
            return null;
        else
            return pastureAnimal;

    }

    //Visitor Functions
    public void AddVisitor(PastureAnimal visitorAnimal)
    {
        if (visitorAnimals.Count < maxVisitor && visitorAnimal.currentStates.getState() == StateList.feedSearching)
        {
            visitorAnimals.Add(visitorAnimal);
            visitorAnimal.GetComponent<NavMeshAgent>().speed = 0f;
            finishedGrassEvent.AddListener(visitorAnimal.exitFromTussock);
            visitorAnimal.fedTussock = this;
            visitorAnimal.currentStates.setState(StateList.eating);
        }
        else
        {
            //TODO: BAŞKA BİR OTLAĞAAA
            //Burada bu tussock visitable olmaktan çıkarılıyor.
            visitorAnimal.exitFromTussock(this);
            Debug.Log(NextGenDebug.NormalError("Find another Tussock."));
        }

    }

    public bool isTussockAvailable()
    {
        bool isHealthAvailable = (((this.maxHealth * 10) / 100) < getHealth()); 
        if (visitorAnimals.Count < maxVisitor && isHealthAvailable)
            return true;
        else
            return false;
    }
    

    //Health Functions
    public double getHealth()
    {
        return currentHealth;
    }

    public void setHealth(double health)
    {
        this.currentHealth = health;
    }
    public bool updateHealth(double amountOfIncrease)
    {
        double consumabilityCheckValue = currentHealth + amountOfIncrease;
        if (consumabilityCheckValue > 0)
        {
            currentHealth += amountOfIncrease;
            return true;
        }
        else if (consumabilityCheckValue == 0)
        {
            currentHealth += amountOfIncrease;
            return false;
        }
        else
            return false;
    }

    public bool isHealthMaximum()
    {
        if (currentHealth == maxHealth) return true;
        else return false;
    }

    public bool maximizeHealth()
    {
        currentHealth = maxHealth;
        return true;
    }

    public bool consumeGrass(double amountOfEating)
    {
        amountOfEating = -amountOfEating;
        if (amountOfEating > 0)
        {
            //Burada ot öbeğinin health değeri eksiye düşüyor.
            return false;
        }

        if (updateHealth(amountOfEating))
        {
            return true;
        }
        else
        {
            
            finishedGrassEvent.Invoke(this);
            return false;
        }


    }

    IEnumerator feedVisitorRoutine()
    {
        float feedingInterval = 1f;
        //while (visitorAnimals.Count > 0 && currentHealth > 0)
        //{
        //    eatGrass(visitorAnimals[]);
        //    feedEvent?.Invoke(oneBiteNutrition);

        //    manageChildGrass();

        //    yield return new WaitForSeconds(feedingInterval);
        //}
        while (true)
        {
            if (visitorAnimals.Count > 0 && this.currentHealth > 0)
            {
                for (int j = 0; j < visitorAnimals.Count; j++)
                {
                    if (consumeGrass(visitorAnimals[j].oneBiteNutrition))
                    {
                        visitorAnimals[j].eat(visitorAnimals[j].oneBiteNutrition);
                    }
                    else
                    {
                        Debug.Log(NextGenDebug.NormalError("No grass!"));
                    }

                }
                
            }

            yield return new WaitForSeconds(feedingInterval);
        }
    }


    public IEnumerator updateTussockRoutine()
    {
        while (true)
        {

            if (currentHealth < 0)
                currentHealth = 0;

            double diffHealth = maxHealth - currentHealth;
            double oneGrassHealth = maxHealth / childGrass.Count;
            int invisibleGrassCount = (int)Math.Floor(diffHealth / oneGrassHealth);


            if (invisibleGrassCount > 0)
            {
                if (invisibleGrassCount >= childGrass.Count)
                {

                    childGrass.ForEach(delegate (Grass grass) { grass.setInvisible(); });
                }
                else
                {


                    for (int i = 0; i < invisibleGrassCount; i++)
                    {
                        childGrass[i].setInvisible();
                    }

                    int visibleGrassCount = (childGrass.Count - (childGrass.Count - invisibleGrassCount));
                    for (; visibleGrassCount < childGrass.Count; visibleGrassCount++)
                    {
                        childGrass[visibleGrassCount].setVisible();
                    }

                }
            }
            else
            {
                childGrass.ForEach(delegate (Grass grass) { grass.setVisible(); });
            }

            //Burada Grassland UI elementleri de güncellenmiş oluyor.
            UpdateGrasslandEvent.Invoke();

            yield return new WaitForSeconds(0.5f);
        }
    }

}


