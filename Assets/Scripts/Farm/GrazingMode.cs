using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class GrazingMode : MonoBehaviour
{
    private static GrazingMode _instance;

    public static GrazingMode Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }

    public void Start()
    {
        AnimalManager.Instance.DieAnimalEvent.AddListener(updateHerdAnimalsOnPastureList);
        AnimalManager.Instance.DieAnimalEvent.AddListener(updateDogsOnPastureList);
    }
    public GameObject SpawnArea;
    public GameObject ShepherdSpawnPos;
    public GameObject AnimalSpawnInitPos;
    public List<PastureAnimal> herdAnimalsOnPasture;
    public List<Dog> dogsOnPasture;


    public float whistleHeardDistance = 20f;
    public float shepherdVisiblityDistance = 50f;

    //Kangal modifier
    public double forceFollowModifier = 1;
    public void getHerdAvailableAnimal()
    {
        if (WorkerManager.Instance.currentShepherd == null)
            return;

        List<Animal> allHerdAnimals = new List<Animal>();

        allHerdAnimals.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Sheep));
        allHerdAnimals.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Ram));
        allHerdAnimals.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Lamb));

        List<PastureAnimal> availableAnimals = new List<PastureAnimal>();
        for (int i = 0; i < allHerdAnimals.Count; i++)
        {
            if (!allHerdAnimals[i].isSatietyMaximum())
            {
                if (!allHerdAnimals[i].isBreedingRateMaximum())
                {   
                    PastureAnimal curAnimal = allHerdAnimals[i] as PastureAnimal;
                    curAnimal.currentStates.setState(StateList.pastureAnimalIdle);
                    curAnimal.animalLocation = AnimalLocation.onPasture;
                    curAnimal.playAnimalVoice();
                    availableAnimals.Add(curAnimal);
                }
            }

        }

        if(availableAnimals.Count != 0)
            spawnPastureAnimals(availableAnimals);
        //CurrentShepherd işçiye available animal listesi veriliyor.
        //!!!!!!BUNA REFACTORING SONRASINDA HALA İHTİYACIN VARSA BURADA BİR PROBLEM VAR HACIT!!!!!!!
       // WorkerManager.Instance.currentShepherd.GetComponent<ShepherdTestScript>().herdAnimalList = availableAnimals;
        herdAnimalsOnPasture = availableAnimals;

    }


    public void getShepherdDogs()
    {
        if (WorkerManager.Instance.currentShepherd == null)
            return;


        List<Animal> allDogs = new List<Animal>();
        List<Animal> allKangalDogs = new List<Animal>();

        allKangalDogs = AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Kangal);
        //Kangal follow force calculating
        forceFollowModifier = Math.Pow(Kangal.baseForceFollowModifier, allKangalDogs.Count);


        allDogs.AddRange(allKangalDogs);
        
        List<Dog> availableDogs = new List<Dog>();
        for (int i = 0; i < allDogs.Count; i++)
        {
            if (!allDogs[i].isSatietyMaximum())
            {
                if ((allDogs[i].getSatiety()*100)/ allDogs[i].maxSatiety > 15)
                {
                    Dog curDog = allDogs[i] as Dog;
                    
                    curDog.currentStates.setState(StateList.shepherdFollowing);
                    curDog.animalLocation = AnimalLocation.onPasture;
                    availableDogs.Add(curDog);
                }
            }

        }

        if (availableDogs.Count != 0)
            spawnDogs(availableDogs);
        //CurrentShepherd işçiye available animal listesi veriliyor.
        //!!!!!!BUNA REFACTORING SONRASINDA HALA İHTİYACIN VARSA BURADA BİR PROBLEM VAR HACIT!!!!!!!
        // WorkerManager.Instance.currentShepherd.GetComponent<ShepherdTestScript>().herdAnimalList = availableAnimals;

        dogsOnPasture = availableDogs;
    }

    public void updateHerdAnimalsOnPastureList()
    {
        if (herdAnimalsOnPasture.Count != 0)
        {
            for (int i = 0; i < herdAnimalsOnPasture.Count; i++)
            {
                if (herdAnimalsOnPasture[i] == null)
                {
                    herdAnimalsOnPasture.Remove(herdAnimalsOnPasture[i]);
                    i--;
                }
            }
        }
    }
    public void updateDogsOnPastureList()
    {
        if (dogsOnPasture.Count != 0)
        {
            for (int i = 0; i < dogsOnPasture.Count; i++)
            {
                if (dogsOnPasture[i] == null)
                {
                    dogsOnPasture.Remove(dogsOnPasture[i]);
                    i--;
                }
            }
        }
    }
    public void spawnPastureAnimals(List<PastureAnimal> allAvailableHerdAnimals)
    {
        int baseColumn = 5;
        int tempHerdAnimalCount = allAvailableHerdAnimals.Count;
        int row = (int)Math.Ceiling((double)tempHerdAnimalCount / baseColumn);
        int column = 0;

        if (tempHerdAnimalCount >= baseColumn)
            column = baseColumn;
        else
            column = tempHerdAnimalCount;
        

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                 allAvailableHerdAnimals[(i * baseColumn) + j].setPosition(
                        new Vector3(AnimalSpawnInitPos.transform.position.x + j, 0,
                            AnimalSpawnInitPos.transform.position.z + i));

                 tempHerdAnimalCount--;
            }

            if (tempHerdAnimalCount / baseColumn < 1)
                column = tempHerdAnimalCount;
            
        }
    }

    public void spawnDogs(List<Dog> allAvailableDogs)
    {
        int baseColumn = 5;
        int tempDogCount = allAvailableDogs.Count;
        int row = (int)Math.Ceiling((double)tempDogCount / baseColumn);
        int column = 0;

        if (tempDogCount >= baseColumn)
            column = baseColumn;
        else
            column = tempDogCount;


        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                allAvailableDogs[(i * baseColumn) + j].setPosition(
                    new Vector3((ShepherdSpawnPos.transform.position.x+2) + j, 0,
                        (ShepherdSpawnPos.transform.position.z + 2) + i));

                tempDogCount--;
            }

            if (tempDogCount / baseColumn < 1)
                column = tempDogCount;

        }
    }

    public void goGrazing()
    {
        if (!WorkerManager.Instance.areThereAvailableWorker())
            return;

        WorkerManager.Instance.getAvailableWorkerForShepherd();
        ShepherdUI.Instance.openShepherdUI();
        SheepBarnUI.Instance.GoGrazeBtn.SetActive(false);
        WorkerHouseUI.Instance.GoPastureBtn.SetActive(false);
        SkillManager.Instance.makeSkillsUsable();
    }



}
