using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class ChickenCoopUI : MonoBehaviour
{
    #region Singleton Things
    private static ChickenCoopUI _instance;
    public static ChickenCoopUI Instance
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

    public GameObject ChickenCoopPanel;
    public GameObject MemberWindowContentParent;
    public void openChickenCoopPanel()
    {
        generateGeneralUIData();

        //GameTimeManager.MinuteFinishedEvent.AddListener(updateChickenCoopUI);
        if (FarmUI.Instance.openMenu(ChickenCoopPanel))
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

                if (raycastHit.collider.tag == "ChickenCoop" && raycastHit.collider.transform.childCount == 0)
                {
                    openChickenCoopPanel();
                }
            }
        }

    }


    //General Window

    public TextMeshProUGUI totalMember;
    public Slider coopCapacitySlider;
    public TextMeshProUGUI totalChickenMember;
    public TextMeshProUGUI totalRoosterMember;
    public TextMeshProUGUI totalFeed;
    public TextMeshProUGUI monthlyFeedConsumption;

    public void generateGeneralUIData()
    {
        int totalChicken;
        int totalRooster;
        totalChicken = AnimalManager.Instance.ChickenParent.transform.childCount;
        totalRooster = AnimalManager.Instance.RoosterParent.transform.childCount;
        int totalCoopMember = totalRooster + totalChicken;

        totalMember.SetText(totalCoopMember.ToString());
        coopCapacitySlider.value = (totalCoopMember * 100) / ChickenCoopManager.Instance.currentCapacity;
        totalChickenMember.SetText(totalChicken.ToString());
        totalRoosterMember.SetText(totalRooster.ToString());
        totalFeed.SetText(TableManager.Instance.farmStockTable.chickenCoopFeed.ToString("F1"));
        monthlyFeedConsumption.SetText(calculateFeedConsumption(30).ToString("F1"));
    }
    
    public double calculateFeedConsumption(int day)
    {
        int totalChicken;
        int totalRooster;
        totalChicken = AnimalManager.Instance.ChickenParent.transform.childCount;
        totalRooster = AnimalManager.Instance.RoosterParent.transform.childCount;
        double chickenMaxSatiety = 10;
        double roosterMaxSatiety = 15;
        //int digestTimePerDay = (oneDayToMinute / 10);
        double totalOneBiteNutrition = (chickenMaxSatiety * totalChicken +
                                        roosterMaxSatiety * totalRooster);

        double calculatedConsumption = totalOneBiteNutrition * day;
        if (calculatedConsumption < 0)
        {
            return 0;
        }

        return calculatedConsumption;
    }

    public void buyOneMonthChickenFeed()
    {
        double consumption = calculateFeedConsumption(30);
        FarmStockManager.Instance.buyChickenFeed((int)consumption);
    }




    //----------------------------------------------------------------
    //Member Window
    //Member Window

    public ChickenCoopUI refreshMember(string member)
    {
        List<ChickenCoopInformationPanel> allInfos = new List<ChickenCoopInformationPanel>();
        allInfos.AddRange(chickenInfos);
        allInfos.AddRange(roosterInfos);
        for (var i = 0; i < allInfos.Count; i++)
        {
            var ccip = allInfos[i];
            if (ccip.AnimalId == member)
            {
                DestroyImmediate(ccip.gameObject);
                allInfos.Remove(ccip);
            }
        }

        return this;
    }


    public void updateChickenCoopMemberUI()
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

    public GameObject chickenCoopChickenInformationPanelPrefab;
    public GameObject chickenCoopRoosterInformationPanelPrefab;


    //Hayvanlar..
    private List<Animal> listedChickens = new List<Animal>();
    private List<Animal> listedRooster = new List<Animal>();

    //Hayvanların bilgi panelleri
    private List<ChickenCoopInformationPanel> chickenInfos = new List<ChickenCoopInformationPanel>();
    private List<ChickenCoopInformationPanel> roosterInfos = new List<ChickenCoopInformationPanel>();


    private int totalChickenCoopAnimalCount;
    public void generateMemberUIData()
    {
        for (int i = 0; i < MemberWindowContentParent.transform.childCount; i++)
        {
            Destroy(MemberWindowContentParent.transform.GetChild(i).gameObject);
        }

        listedChickens = AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Chicken);
        listedRooster = AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Rooster);
        chickenInfos.Clear();
        roosterInfos.Clear();

        float onePanelHeight = 45;
        totalChickenCoopAnimalCount = (listedChickens.Count + listedRooster.Count);
        float plusMemberWindow = onePanelHeight * totalChickenCoopAnimalCount;
        MemberWindowContentParent.GetComponent<RectTransform>().sizeDelta = new Vector2(MemberWindowContentParent.GetComponent<RectTransform>().sizeDelta.x, (plusMemberWindow));


        if (listedChickens.Count != 0)
        {
            for (int i = 0; i < listedChickens.Count; i++)
            {
                Chicken curChicken = listedChickens[i] as Chicken;
                GameObject newChickenPanel = Instantiate(chickenCoopChickenInformationPanelPrefab, MemberWindowContentParent.transform);
                ChickenCoopInformationPanel newChickenInformationPanel =
                    newChickenPanel.GetComponent<ChickenCoopInformationPanel>();
                chickenInfos.Add(newChickenInformationPanel);
                //---MUST--//
                newChickenInformationPanel.AnimalId = curChicken.animalId;

                newChickenInformationPanel.IsLayEgg = curChicken.isLayEgg;
                newChickenInformationPanel.Satiety = (curChicken.currentSatiety * 100) / curChicken.maxSatiety;
                newChickenInformationPanel.Attrition = (curChicken.currentAttrition * 100) / Animal.maxAttritionLimit;
                newChickenInformationPanel.Weight = (curChicken.currentWeight * 100) / AnimalMaxWeight.Chicken;


            }
        }

        if (listedRooster.Count != 0)
        {
            for (int i = 0; i < listedRooster.Count; i++)
            {
                Rooster curRooster = listedRooster[i] as Rooster;
                GameObject newRoosterPanel = Instantiate(chickenCoopRoosterInformationPanelPrefab, MemberWindowContentParent.transform);
                ChickenCoopInformationPanel newRoosterInformationPanel =
                    newRoosterPanel.GetComponent<ChickenCoopInformationPanel>();
                roosterInfos.Add(newRoosterInformationPanel);
                //---MUST--//
                newRoosterInformationPanel.AnimalId = curRooster.animalId;

                newRoosterInformationPanel.Satiety = (curRooster.currentSatiety * 100) / curRooster.maxSatiety;
                newRoosterInformationPanel.Attrition = (curRooster.currentAttrition * 100) / Animal.maxAttritionLimit;
                newRoosterInformationPanel.Weight = (curRooster.currentWeight * 100) / AnimalMaxWeight.Rooster;

            }
        }

    }


    public void updateUIData()
    {
        int allChickenCount = AnimalManager.Instance.ChickenParent.transform.childCount;
        int allRoosterCount = AnimalManager.Instance.roosterPrefab.transform.childCount;
        int chickenCoopTotalCount = allChickenCount + allRoosterCount;

        //Yeni hayvanlar gelmiş UI baştan yaratılmalı...
        // Ayrı ayrı kontrol ediliyor çünkü örneğin lambler %100 growth rate ulaştığında lamb siliniyor yerine bir sheep geliyor. Totale bakılırsa sayıda değişim olmayacaktır.
        if (allChickenCount != listedChickens.Count || allRoosterCount != listedRooster.Count)
            generateMemberUIData();

        else
        {
            if (chickenInfos.Count != 0)
            {
                for (int i = 0; i < chickenInfos.Count; i++)
                {
                    //Sheep curSheep = AnimalManager.Instance.getAnimal(sheepInfos[i].AnimalId) as Sheep;
                    Chicken curChicken = listedChickens[i] as Chicken;
                    if (curChicken != null)
                    {
                        chickenInfos[i].IsLayEgg = curChicken.isLayEgg;
                        chickenInfos[i].Satiety = (curChicken.currentSatiety * 100) / curChicken.maxSatiety;
                        chickenInfos[i].Attrition = (curChicken.currentAttrition * 100) / Animal.maxAttritionLimit;
                        chickenInfos[i].Weight = (curChicken.currentWeight * 100) / AnimalMaxWeight.Chicken;
                    }
                }
            }

            if (roosterInfos.Count != 0)
            {
                for (int i = 0; i < roosterInfos.Count; i++)
                {
                    //Rooster curRooster = AnimalManager.Instance.getAnimal(roosterInfos[i].AnimalId) as Rooster;
                    Rooster curRooster = listedRooster[i] as Rooster;
                    if (curRooster != null)
                    {
                        roosterInfos[i].Satiety = (curRooster.currentSatiety * 100) / curRooster.maxSatiety;
                        roosterInfos[i].Attrition = (curRooster.currentAttrition * 100) / Animal.maxAttritionLimit;
                        roosterInfos[i].Weight = (curRooster.currentWeight * 100) / AnimalMaxWeight.Rooster;
                    }

                }
            }

        }

    }



}
