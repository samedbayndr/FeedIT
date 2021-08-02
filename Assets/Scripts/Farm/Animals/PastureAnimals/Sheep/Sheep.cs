using System;
using UnityEngine;

public enum SheepReProductionStatus
{
    ReadyForPregnant,
    Pregnant
}
public class Sheep : SheepGroup
{
    public Animator sheepAnimator;
    public SheepReProductionStatus reProductionStatus;
    protected override void Start()
    {
        base.Start();

        if (getBreeding() > 0)
            reProductionStatus = SheepReProductionStatus.Pregnant;
        else
            reProductionStatus = SheepReProductionStatus.ReadyForPregnant;

        sheepAnimator = GetComponent<Animator>();
        //Gece full save esnasında zaten wool miktarı kayıt altına alınıyor. Çakışma yaşanmaması için sabah sekize alındı.
        GameTimeManager.MorningEightEvent.AddListener(gainWool);

    }



    protected override void Update()
    {
        base.Update();

        sheepAnimator.SetFloat("SheepSpeed", animalAgent.desiredVelocity.magnitude);
    }

    //Koyun hamileliği başlıyor
    public bool fertilize()
    {
        if (reProductionStatus == SheepReProductionStatus.ReadyForPregnant && this.animalLocation == AnimalLocation.onBarn)
        {
            reProductionStatus = SheepReProductionStatus.Pregnant;
            GameTimeManager.DayFinishedEvent.AddListener(pregnationProcess);
            return true;
        }
        else
            return false;
    }

    //Hayvan döllendikten sonra hamilelik başlıyor..
    // Her günün sonunda pregnantProgressionRate kadar doğum ilerliyor.
    public void pregnationProcess()
    {
        if (reProductionStatus == SheepReProductionStatus.Pregnant)
        {
            double pregnantProgressionRate = maxBreedingRate / 3;
            this.currentBreedingRate = Math.Ceiling(this.currentBreedingRate + pregnantProgressionRate);
            if (this.currentBreedingRate > maxBreedingRate)
                this.maximizeBreedingRate();

            if (this.isBreedingRateMaximum())
            {
                GameTimeManager.DayFinishedEvent.RemoveListener(pregnationProcess);
                GameTimeManager.DayFinishedEvent.AddListener(laborPains);
            }
        }
    }


    // Doğum sancılarından dolayı hayvan painTenacityDay değişkenine atanan değer kadar yaşayacaktır.
    // Eğer doğum olmazsa painTenacityDay sonunda hayvan ölecektir.
    private int laborPainDayCount = 0;
    private int painTenacityDay = 5;
    public void laborPains()
    {
        laborPainDayCount++;
        if (laborPainDayCount == painTenacityDay)
        {
            GameTimeManager.DayFinishedEvent.RemoveListener(laborPains);
            this.killAnimal();
        }
        else if (laborPainDayCount >= Math.Round((double)painTenacityDay / 2))
        {
            LogPanelUI.Instance.addLog("A sheep will die in "+ (painTenacityDay - laborPainDayCount) + " days if you don't give birth");
            Debug.Log((painTenacityDay - laborPainDayCount ) + " gün sonra koyun ölecek!");
        }
        
    }




}
