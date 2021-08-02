
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SheepBarnInformationPanel : MonoBehaviour
{
    //Bu değişkenin modify ettiği gameobject daima invisible olmalı!!
    [SerializeField] private TextMeshProUGUI animalIdHiddenText;

    [SerializeField] private TextMeshProUGUI breedingRatePercentage;
    [SerializeField] private TextMeshProUGUI growthRatePercentage;
    [SerializeField] private TextMeshProUGUI satietyPercentage;
    [SerializeField] private TextMeshProUGUI attritionPercentage;
    [SerializeField] private TextMeshProUGUI weightPercentage;
    [SerializeField] private TextMeshProUGUI woolPercentage;
    [SerializeField] private Button actionButton;
    [SerializeField] private TMP_Dropdown actionDropdown;

    private string _animalId;
    private double _breedingRate;
    private double _growthRate;
    private double _satiety;
    private double _attrition;
    private double _weight;
    private double _wool;

    public double BreedingRate
    {
        get
        {
            return _breedingRate;
        }
        set
        {
            _breedingRate = value;
            breedingRatePercentage.SetText(_breedingRate + "%");
        }
    }
    public double GrowthRate
    {
        get
        {
            return _growthRate;
        }
        set
        {
            _growthRate = value;
            growthRatePercentage.SetText(_growthRate.ToString("F") + "%");
        }
    }
    public double Satiety
    {
        get
        {
            return _satiety;
        }
        set
        {
            _satiety = value;
            satietyPercentage.SetText(_satiety.ToString("F") + "%");
        }
    }
    public double Attrition
    {
        get
        {
            return _attrition;
        }
        set
        {
            _attrition = value;
            attritionPercentage.SetText(_attrition.ToString("F") + "%");
        }
    }
    public double Weight
    {
        get
        {
            return _weight;
        }
        set
        {
            _weight = value;
            weightPercentage.SetText(_weight.ToString("F") + "%");
        }
    }
    public double Wool
    {
        get
        {
            return _wool;
        }
        set
        {
            _wool = value;
            woolPercentage.SetText(_wool.ToString("F") + "%");
        }
    }
    public string AnimalId
    {
        get
        {
            return _animalId;
        }
        set
        {
            _animalId = value;
            animalIdHiddenText.SetText(_animalId);
        }
    }


    public void performAction()
    {
        string action = actionDropdown.options[actionDropdown.value].text;
        string animalId = animalIdHiddenText.text;
        Animal curAnimal = AnimalManager.Instance.getAnimal(animalId);
        AnimalEntity curAnimalEntity = AnimalManager.Instance.getAnimalEntity(animalId);

        switch (action)
        {
            case "Shearing":
                if (curAnimalEntity.animalType == AnimalType.Sheep || curAnimalEntity.animalType == AnimalType.Ram)
                {
                    if (AnimalManager.Instance.shearingWool(animalId))
                        SheepBarnUI.Instance.updateSheepBarnUI();
                }

                //(curAnimal as SheepGroup)?.shearing(); 
                break;
            case "Breeding":
                if (AnimalManager.Instance.breeding(animalId))
                    SheepBarnUI.Instance.refreshMember(animalId).updateSheepBarnUI();
                break;
            case "Cutting":
                if (AnimalManager.Instance.cutAnimal(animalId))
                    SheepBarnUI.Instance.refreshMember(animalId).updateSheepBarnUI();
                break;
        }

        //Panel Update ediliyor...

    }
}
