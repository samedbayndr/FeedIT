
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChickenCoopInformationPanel : MonoBehaviour
{
    //Bu değişkenin modify ettiği gameobject daima invisible olmalı!!
    [SerializeField] private TextMeshProUGUI animalIdHiddenText;

    [SerializeField] private TextMeshProUGUI isLayEgg;
    [SerializeField] private TextMeshProUGUI satietyPercentage;
    [SerializeField] private TextMeshProUGUI attritionPercentage;
    [SerializeField] private TextMeshProUGUI weightPercentage;
    [SerializeField] private Button actionButton;
    //[SerializeField] private TMP_Dropdown actionDropdown;

    private string _animalId;
    private bool _isLayEgg;
    private double _satiety;
    private double _attrition;
    private double _weight;

    public bool IsLayEgg
    {
        get
        {
            return _isLayEgg;
        }
        set
        {
            _isLayEgg = value;
            if (_isLayEgg == true)
            {
                isLayEgg.SetText("True");
            }
            else if (_isLayEgg == false)
            {
                isLayEgg.SetText("False");
            }
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
        if (AnimalManager.Instance.cutAnimal(animalId))
            ChickenCoopUI.Instance.refreshMember(animalId).updateChickenCoopMemberUI();
    }
}
