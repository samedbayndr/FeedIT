using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class DogHouseUI : MonoBehaviour
{
    #region Singleton Things
    private static DogHouseUI _instance;
    public static DogHouseUI Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
    }
    #endregion

    public GameObject DogHousePanel;
    public GameObject MemberWindowContentParent;
    public void openDogHousePanel()
    {
        generateGeneralUIData();

        //GameTimeManager.MinuteFinishedEvent.AddListener(updateChickenCoopUI);
        if (FarmUI.Instance.openMenu(DogHousePanel))
        {

            //--//
        }
        else
        {
            Debug.Log(NextGenDebug.NormalError("Menü yok!! "));
        }

    }

    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {

                if (raycastHit.collider.tag == "DogHouse" && raycastHit.collider.transform.childCount == 0)
                {
                    openDogHousePanel();
                }
            }
        }

    }


    public TextMeshProUGUI totalMember;
    public Slider dogCapacitySlider;
    public TextMeshProUGUI totalKangalMember;
    public TextMeshProUGUI totalFeed;
    public TextMeshProUGUI monthlyFeedConsumption;

    public void generateGeneralUIData()
    {
        int totalKangal;
        totalKangal = AnimalManager.Instance.KangalParent.transform.childCount;
        int totalDogHouseMember = totalKangal;

        totalMember.SetText(totalDogHouseMember.ToString());
        dogCapacitySlider.value = (totalDogHouseMember * 100) / DogHouseManager.Instance.currentCapacity;
        totalKangalMember.SetText(totalKangal.ToString());
        totalFeed.SetText(TableManager.Instance.farmStockTable.dogFeed.ToString("F1"));
        monthlyFeedConsumption.SetText(calculateFeedConsumption(30).ToString("F1"));
    }

    public double calculateFeedConsumption(int day)
    {
        int totalKangal;
        totalKangal = AnimalManager.Instance.KangalParent.transform.childCount;
        double kangalMaxSatiety = 500;
        double totalOneBiteNutrition = (kangalMaxSatiety * totalKangal);

        double calculatedConsumption = totalOneBiteNutrition * day;
        if (calculatedConsumption < 0)
        {
            return 0;
        }

        return calculatedConsumption;
    }

    public void buyOneMonthDogFeed()
    {
        double consumption = calculateFeedConsumption(30);
        FarmStockManager.Instance.buyDogFeed((int)consumption);
    }



    //----------------------------------------------------------------
    //Member Window
    //Member Window
    public void updateDogHouseMemberUI()
    {
        if (MemberWindowContentParent.activeSelf == true)
        {
            updateUIData();
        }
        else
        {
            //GameTimeManager.MinuteFinishedEvent.RemoveListener(updateUIData);
        }
    }

    public GameObject dogHouseWorkerInformationPanelPrefab;

    //Hayvanlar..
    private List<Animal> listedKangals = new List<Animal>();

    //Hayvanların bilgi panelleri
    private List<DogHouseInformationPanel> kangalInfos = new List<DogHouseInformationPanel>();


    public void generateMemberUIData()
    {
        for (int i = 0; i < MemberWindowContentParent.transform.childCount; i++)
        {
            Destroy(MemberWindowContentParent.transform.GetChild(i).gameObject);
        }

        listedKangals = AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Kangal);
        kangalInfos.Clear();

        float onePanelHeight = 45;
        int totalDogHouseAnimalCount = listedKangals.Count;
        float plusMemberWindow = onePanelHeight * totalDogHouseAnimalCount;
        MemberWindowContentParent.GetComponent<RectTransform>().sizeDelta = new Vector2(MemberWindowContentParent.GetComponent<RectTransform>().sizeDelta.x, (plusMemberWindow));


        if (listedKangals.Count != 0)
        {
            for (int i = 0; i < listedKangals.Count; i++)
            {
                Kangal curKangal = listedKangals[i] as Kangal;
                GameObject newKangalPanel = Instantiate(dogHouseWorkerInformationPanelPrefab, MemberWindowContentParent.transform);
                DogHouseInformationPanel newDogInformationPanel =
                    newKangalPanel.GetComponent<DogHouseInformationPanel>();
                kangalInfos.Add(newDogInformationPanel);
                //---MUST--//
                newDogInformationPanel.AnimalId = curKangal.animalId;
                newDogInformationPanel.Satiety = (curKangal.currentSatiety * 100) / curKangal.maxSatiety;
                newDogInformationPanel.Attrition = (curKangal.currentAttrition * 100) / Animal.maxAttritionLimit;


            }
        }
        
    }



    //WorkerHouseUI dönüş tipinin sebebi chain function olarak kullanabilmek.
    public DogHouseUI refreshMember(string member)
    {
        for (var i = 0; i < kangalInfos.Count; i++)
        {
            var dhip = kangalInfos[i];
            if (dhip.AnimalId == member)
            {
                DestroyImmediate(dhip.gameObject);
                kangalInfos.Remove(dhip);
            }
        }

        return this;
    }

    public void updateUIData()
    {


        if (kangalInfos.Count != 0)
        {
            //Worker listesi yenileniyor.
            listedKangals = AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Kangal);

            for (int i = 0; i < kangalInfos.Count; i++)
            {
                Animal curKangal = listedKangals[i] as Animal;
                if (curKangal != null)
                {
                    kangalInfos[i].Satiety = (curKangal.currentSatiety * 100) / curKangal.maxSatiety;
                    kangalInfos[i].Attrition = (curKangal.currentAttrition * 100) / Animal.maxAttritionLimit;
                }
            }
        }

    }



}
