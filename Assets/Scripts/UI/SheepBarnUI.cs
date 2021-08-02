using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SheepBarnUI : MonoBehaviour
{
    #region Singleton Things
    private static SheepBarnUI _instance;
    public static SheepBarnUI Instance
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

    public GameObject SheepBarnPanel;
    public GameObject MemberWindowContentParent;
    public GameObject GoGrazeBtn;
    public void openSheepBarnPanel()
    {
        generateGeneralUIData();
        //generateUIData();
        //GameTimeManager.MinuteFinishedEvent.AddListener(updateSheepBarnUI);
        if (FarmUI.Instance.openMenu(SheepBarnPanel))
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

                if (raycastHit.collider.tag == "SheepBarn" && raycastHit.collider.transform.childCount == 0)
                {
                    openSheepBarnPanel();
                }
            }
        }

    }

    public void updateSheepBarnUI()
    {
        if (MemberWindowContentParent.activeSelf == true)
        {
            updateSheepBarnMemberUIData();
        }
        else
        {
            //GameTimeManager.MinuteFinishedEvent.RemoveListener(updateUIData);
        }
    }

    public TextMeshProUGUI totalMember;
    public Slider barnCapacitySlider;
    public TextMeshProUGUI totalSheepMember;
    public TextMeshProUGUI totalRamMember;
    public TextMeshProUGUI totalLambMember;

    public void generateGeneralUIData()
    {
        int totalSheep;
        int totalRam;
        int totalLamb;
        totalSheep = AnimalManager.Instance.SheepParent.transform.childCount;
        totalRam = AnimalManager.Instance.RamParent.transform.childCount;
        totalLamb = AnimalManager.Instance.LambParent.transform.childCount;
        int totalSheepBarn = totalLamb+ totalSheep + totalRam;

        totalMember.SetText(totalSheepBarn.ToString());
        barnCapacitySlider.value = (totalSheepBarn * 100) / SheepBarnManager.Instance.currentCapacity;
        totalSheepMember.SetText(totalSheep.ToString());
        totalRamMember.SetText(totalRam.ToString());
        totalLambMember.SetText(totalLamb.ToString());
    }



    //Bu prefabler büyük oranda aynılar ama lamb, ram ve sheepde gösterime sunulan bilgiler değişiklik gösteriyor.
    //Action farklılıkları var.
    public GameObject sheepBarnSheepInformationPanelPrefab;
    public GameObject sheepBarnRamInformationPanelPrefab;
    public GameObject sheepBarnLambInformationPanelPrefab;


    //Hayvanlar..
    private List<Animal> listedSheeps = new List<Animal>();
    private List<Animal> listedRams = new List<Animal>();
    private List<Animal> listedLambs = new List<Animal>();

    //Hayvanların bilgi panelleri
    private List<SheepBarnInformationPanel> sheepInfos = new List<SheepBarnInformationPanel>();
    private List<SheepBarnInformationPanel> ramInfos = new List<SheepBarnInformationPanel>();
    private List<SheepBarnInformationPanel> lambInfos = new List<SheepBarnInformationPanel>();


    private int totalSheepBarnAnimalCount;
    public void generateSheepBarnMemberUIData()
    {
        for (int i = 0; i < MemberWindowContentParent.transform.childCount; i++)
        {
            Destroy(MemberWindowContentParent.transform.GetChild(i).gameObject);
        }

        listedSheeps = AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Sheep);
        listedRams = AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Ram);
        listedLambs = AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Lamb);
        sheepInfos.Clear();
        ramInfos.Clear();
        lambInfos.Clear();

        float onePanelHeight = 45;
        totalSheepBarnAnimalCount = (listedSheeps.Count + listedRams.Count + listedLambs.Count);
        float plusMemberWindow = onePanelHeight * totalSheepBarnAnimalCount;
        MemberWindowContentParent.GetComponent<RectTransform>().sizeDelta = new Vector2(MemberWindowContentParent.GetComponent<RectTransform>().sizeDelta.x, (plusMemberWindow));


        if (listedSheeps.Count != 0)
        {
            for (int i = 0; i < listedSheeps.Count; i++)
            {
                Sheep curSheep = listedSheeps[i] as Sheep;
                if (curSheep.currentStates.getState() != StateList.restingOnBarn)
                    continue;
                GameObject newSheepPanel = Instantiate(sheepBarnSheepInformationPanelPrefab, MemberWindowContentParent.transform);
                SheepBarnInformationPanel newSheepInformationPanel =
                    newSheepPanel.GetComponent<SheepBarnInformationPanel>();
                sheepInfos.Add(newSheepInformationPanel);
                //---MUST--//
                newSheepInformationPanel.AnimalId = curSheep.animalId;


                newSheepInformationPanel.BreedingRate = (curSheep.currentBreedingRate * 100) / Animal.maxBreedingRate;
                newSheepInformationPanel.Satiety = (curSheep.currentSatiety * 100) / curSheep.maxSatiety;
                newSheepInformationPanel.Attrition = (curSheep.currentAttrition * 100) / Animal.maxAttritionLimit;
                newSheepInformationPanel.Weight = (curSheep.currentWeight * 100) / AnimalMaxWeight.Sheep;
                newSheepInformationPanel.Wool = (curSheep.wool * 100) / curSheep.maxWool;

            }
        }

        if (listedRams.Count != 0)
        {
            for (int i = 0; i < listedRams.Count; i++)
            {
                Ram curRam = listedRams[i] as Ram;
                if (curRam.currentStates.getState() != StateList.restingOnBarn)
                    continue;
                GameObject newRamPanel = Instantiate(sheepBarnRamInformationPanelPrefab, MemberWindowContentParent.transform);
                SheepBarnInformationPanel newRamInformationPanel =
                    newRamPanel.GetComponent<SheepBarnInformationPanel>();
                ramInfos.Add(newRamInformationPanel);
                //---MUST--//
                newRamInformationPanel.AnimalId = curRam.animalId;

                newRamInformationPanel.Satiety = (curRam.currentSatiety * 100) / curRam.maxSatiety;
                newRamInformationPanel.Attrition = (curRam.currentAttrition * 100) / Animal.maxAttritionLimit;
                newRamInformationPanel.Weight = (curRam.currentWeight * 100) / AnimalMaxWeight.Ram;
                newRamInformationPanel.Wool = (curRam.wool * 100) / curRam.maxWool;

            }
        }

        if (listedLambs.Count != 0)
        {
            for (int i = 0; i < listedLambs.Count; i++)
            {
                Lamb curLamb = listedLambs[i] as Lamb;
                if (curLamb.currentStates.getState() != StateList.restingOnBarn)
                    continue;
                GameObject newLambPanel = Instantiate(sheepBarnLambInformationPanelPrefab, MemberWindowContentParent.transform);
                SheepBarnInformationPanel newLambInformationPanel =
                    newLambPanel.GetComponent<SheepBarnInformationPanel>();
                lambInfos.Add(newLambInformationPanel);

                //---MUST--//
                newLambInformationPanel.AnimalId = curLamb.animalId;

                newLambInformationPanel.Satiety = (curLamb.currentSatiety * 100) / curLamb.maxSatiety;
                newLambInformationPanel.GrowthRate = (curLamb.currentGrowthRate * 100) / Animal.maxGrowthRate;
                newLambInformationPanel.Attrition = (curLamb.currentAttrition * 100) / Animal.maxAttritionLimit;
                //newLambInformationPanel.Weight = (curLamb.currentWeight * 100) / AnimalMaxWeight.Lamb;

            }
        }
    }


    public SheepBarnUI refreshMember(string member)
    {
        List<SheepBarnInformationPanel> allInfos = new List<SheepBarnInformationPanel>();
        allInfos.AddRange(sheepInfos);
        allInfos.AddRange(ramInfos);
        allInfos.AddRange(lambInfos);

        for (var i = 0; i < sheepInfos.Count; i++)
        {
            var sbip = sheepInfos[i];
            if (sbip.AnimalId == member)
            {
                DestroyImmediate(sbip.gameObject);
                sheepInfos.Remove(sbip);
                return this;
            }
        }

        for (var i = 0; i < ramInfos.Count; i++)
        {
            var sbip = ramInfos[i];
            if (sbip.AnimalId == member)
            {
                DestroyImmediate(sbip.gameObject);
                ramInfos.Remove(sbip);
                return this;
            }
        }

        for (var i = 0; i < lambInfos.Count; i++)
        {
            var sbip = lambInfos[i];
            if (sbip.AnimalId == member)
            {
                DestroyImmediate(sbip.gameObject);
                lambInfos.Remove(sbip);
                return this;
            }
        }
        //All
        for (var i = 0; i < allInfos.Count; i++)
        {
            var sbip = allInfos[i];
            if (sbip.AnimalId == member)
            {
                DestroyImmediate(sbip.gameObject);
                allInfos.Remove(sbip);
            }
        }

        return null;
    }

    public void updateSheepBarnMemberUIData()
    {
        int allSheepCount = AnimalManager.Instance.SheepParent.transform.childCount;
        int allRamCount = AnimalManager.Instance.RamParent.transform.childCount;
        int allLambCount = AnimalManager.Instance.LambParent.transform.childCount;
        int sheepBarnTotalCount = allLambCount + allRamCount + allSheepCount;

        //Yeni hayvanlar gelmiş UI baştan yaratılmalı...
        // Ayrı ayrı kontrol ediliyor çünkü örneğin lambler %100 growth rate ulaştığında lamb siliniyor yerine bir sheep geliyor. Totale bakılırsa sayıda değişim olmayacaktır.
        if (allSheepCount != listedSheeps.Count || allRamCount != listedRams.Count || allLambCount != listedLambs.Count) 
            generateSheepBarnMemberUIData();

        else
        {
            if (sheepInfos.Count != 0)
            {
                for (int i = 0; i < sheepInfos.Count; i++)
                {
                    //Sheep curSheep = AnimalManager.Instance.getAnimal(sheepInfos[i].AnimalId) as Sheep;
                    Sheep curSheep = listedSheeps[i] as Sheep;
                    if (curSheep != null)
                    {
                        sheepInfos[i].BreedingRate = (curSheep.currentBreedingRate * 100) / Animal.maxBreedingRate;
                        sheepInfos[i].Satiety = (curSheep.currentSatiety * 100) / curSheep.maxSatiety;
                        sheepInfos[i].Attrition = (curSheep.currentAttrition * 100) / Animal.maxAttritionLimit;
                        sheepInfos[i].Weight = (curSheep.currentWeight * 100) / AnimalMaxWeight.Sheep;
                        sheepInfos[i].Wool = (curSheep.wool * 100) / curSheep.maxWool;
                    }
                }
            }

            if (ramInfos.Count != 0)
            {
                for (int i = 0; i < ramInfos.Count; i++)
                {
                    Ram curRam = AnimalManager.Instance.getAnimal(ramInfos[i].AnimalId) as Ram;
                    if (curRam != null)
                    {
                        ramInfos[i].Satiety = (curRam.currentSatiety * 100) / curRam.maxSatiety;
                        ramInfos[i].Attrition = (curRam.currentAttrition * 100) / Animal.maxAttritionLimit;
                        ramInfos[i].Weight = (curRam.currentWeight * 100) / AnimalMaxWeight.Ram;
                        ramInfos[i].Wool = (curRam.wool * 100) / curRam.maxWool;
                    }

                }
            }

            if (lambInfos.Count != 0)
            {
                for (int i = 0; i < lambInfos.Count; i++)
                {
                    Lamb curLamb = AnimalManager.Instance.getAnimal(lambInfos[i].AnimalId) as Lamb;
                    if (curLamb != null)
                    {
                        lambInfos[i].Satiety = (curLamb.currentSatiety * 100) / curLamb.maxSatiety;
                        lambInfos[i].GrowthRate = (curLamb.currentGrowthRate * 100) / Animal.maxGrowthRate;
                        lambInfos[i].Attrition = (curLamb.currentAttrition * 100) / Animal.maxAttritionLimit;
                    }
                }
            }
        }

    }



}
