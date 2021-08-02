
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DogHouseInformationPanel : MonoBehaviour
{
    //Bu değişkenin modify ettiği gameobject daima invisible olmalı!!
    [SerializeField] private TextMeshProUGUI animalIdHiddenText;
    [SerializeField] private TextMeshProUGUI satietyPercentage;
    [SerializeField] private TextMeshProUGUI attritionPercentage;
    [SerializeField] private TextMeshProUGUI weightPercentage;
    [SerializeField] private Button actionButton;
    //[SerializeField] private TMP_Dropdown actionDropdown;

    private string _animalId;
    private double _satiety;
    private double _attrition;
    private double _weight;


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
        string animalId = animalIdHiddenText.text;
        AnimalEntity entity = AnimalManager.Instance.getAnimalEntity(animalId);
        AnimalManager.Instance.getAnimalWithEntity(entity).killAnimal();
        DogHouseUI.Instance.refreshMember(animalId);
    }
}
