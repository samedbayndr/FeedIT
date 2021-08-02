using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ram : SheepGroup
{
    public int maxFertilizePower = 8;
    public int currentFertilizePower;


    public Animator ramAnimator;
    protected override void Start()
    {
        base.Start();
        currentFertilizePower = maxFertilizePower;

        GameTimeManager.DayFinishedEvent.AddListener(resetFertilizePower);
        GameTimeManager.MorningEightEvent.AddListener(impregnate);

        ramAnimator = GetComponent<Animator>();
        //Gece full save esnasında zaten wool miktarı kayıt altına alınıyor. Çakışma yaşanmaması için sabah sekize alındı.
        GameTimeManager.MorningEightEvent.AddListener(gainWool);

    }

    protected override void Update()
    {
        base.Update();

        ramAnimator.SetFloat("PastureAnimalSpeed", animalAgent.desiredVelocity.magnitude);
    }

    public void impregnate()
    {
        if (animalLocation != AnimalLocation.onBarn)
            return;

        List<Sheep> availableSheepList = AnimalManager.Instance.getFertilizableSheeps();
        for (int i = 0; i < availableSheepList.Count; i++)
        {
            if (currentFertilizePower > 0)
            {
                if (availableSheepList[i].fertilize())
                {
                    currentFertilizePower--;
                }

            }
        }
    }

    public void resetFertilizePower()
    {
        currentFertilizePower = maxFertilizePower;
    }
}
