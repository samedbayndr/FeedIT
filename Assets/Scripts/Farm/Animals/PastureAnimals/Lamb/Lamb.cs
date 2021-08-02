using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class Lamb : SheepGroup
{

    public Animator lambAnimator;
    protected override void Start()
    {
        base.Start();
        GameTimeManager.DayFinishedEvent.AddListener(growthProcess);

        lambAnimator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        lambAnimator.SetFloat("SheepSpeed", animalAgent.desiredVelocity.magnitude);
    }

    public void growthProcess()
    {
        double growthProgressionRate = maxBreedingRate / 3;
        this.currentGrowthRate = Math.Ceiling(this.currentGrowthRate + growthProgressionRate);
        if (this.currentGrowthRate > maxGrowthRate)
            this.maximizeGrowthRate();

        if (this.isGrowthRateMaximum())
        {
            GameTimeManager.DayFinishedEvent.RemoveListener(growthProcess);
            evolution();
        }
    }

    public void evolution()
    {
        if (sex == AnimalSex.male)
        {
            GameObject newRaAnimal = Instantiate(AnimalManager.Instance.ramPrefab, AnimalManager.Instance.RamParent.transform);
            Ram newRam = newRaAnimal.GetComponent<Ram>();

            string animalRaId = IdGenerator.generateGUID();
            newRam.animalId = animalRaId;
            AnimalEntity newRaEntity = new AnimalEntity(animalRaId, AnimalType.Ram, 0, 0, 100,
                newRam.currentSatiety, newRam.maxHealth, newRam.currentWeight, 0);

            TableManager.Instance.ownedAnimalTable.AllAnimals.Add(newRaEntity);
            TableManager.Instance.inventoryTable.ram++;
        }
        else if(sex == AnimalSex.female)
        {
            GameObject newShAnimal = Instantiate(AnimalManager.Instance.sheepPrefab, AnimalManager.Instance.SheepParent.transform);
            Sheep newSheep = newShAnimal.GetComponent<Sheep>();

            string animalShId = IdGenerator.generateGUID();
            newSheep.animalId = animalShId;
            AnimalEntity newShEntity = new AnimalEntity(animalShId, AnimalType.Sheep, 1, 0, 100,
                newSheep.currentSatiety, newSheep.maxHealth, newSheep.currentWeight, 0);

            TableManager.Instance.ownedAnimalTable.AllAnimals.Add(newShEntity);
            TableManager.Instance.inventoryTable.sheep++;
        }

        this.killAnimal();
    }


}