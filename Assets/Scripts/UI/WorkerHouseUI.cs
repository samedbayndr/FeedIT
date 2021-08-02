using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class WorkerHouseUI : MonoBehaviour
{
    #region Singleton Things
    private static WorkerHouseUI _instance;
    public static WorkerHouseUI Instance
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

    public GameObject WorkerHousePanel;
    public GameObject MemberWindowContentParent;
    public GameObject GoPastureBtn;
    public void openWorkerHousePanel()
    {
        generateGeneralUIData();

        //GameTimeManager.MinuteFinishedEvent.AddListener(updateChickenCoopUI);
        if (FarmUI.Instance.openMenu(WorkerHousePanel))
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

                if (raycastHit.collider.tag == "WorkerHouse" && raycastHit.collider.transform.childCount == 0)
                {
                    openWorkerHousePanel();
                }
            }
        }

    }


    //General Window

    public TextMeshProUGUI totalMember;
    public Slider workerCapacitySlider;
    public TextMeshProUGUI availableMember;
    public TextMeshProUGUI totalMeal;
    public TextMeshProUGUI monthlyMealConsumption;

    public void generateGeneralUIData()
    {

        int totalMemberCount = WorkerManager.Instance.allWorkerList.Count;
        int availableWorkerCount = WorkerManager.Instance.getAvailableWorkersCount();

        totalMember.SetText(totalMemberCount.ToString());
        try
        {
            workerCapacitySlider.value = (availableWorkerCount * 100) / totalMemberCount;
        }
        catch (DivideByZeroException e)
        {
            workerCapacitySlider.value = 0;
        }
        availableMember.SetText(availableWorkerCount.ToString());
        totalMeal.SetText(TableManager.Instance.farmStockTable.workerMeal.ToString("F1"));
        monthlyMealConsumption.SetText(calculateMealConsumption(30).ToString("F1"));
    }


    public double calculateMealConsumption(int day)
    {
        int totalWorker = WorkerManager.Instance.allWorkerList.Count;
        double workerMaxSatiety = Worker.maxSatiety;
        double totalOneBiteNutrition = workerMaxSatiety * totalWorker;

        double calculatedConsumption = totalOneBiteNutrition * day;
        if (calculatedConsumption < 0)
        {
            return 0;
        }

        return calculatedConsumption;
    }

    public void buyOneMonthChickenFeed()
    {
        double consumption = calculateMealConsumption(30);
        FarmStockManager.Instance.buyWorkerMeal((int)consumption);
    }




    //----------------------------------------------------------------
    //Member Window
    //Member Window
    public void updateWorkerHouseMemberUI()
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

    public GameObject workerHouseWorkerInformationPanelPrefab;


    private List<Worker> listedWorkers = new List<Worker>();

    private List<WorkerHouseInformationPanel> workerInfos = new List<WorkerHouseInformationPanel>();


    public void generateMemberUIData()
    {

        for (int i = 0; i < MemberWindowContentParent.transform.childCount; i++)
        {
            Destroy(MemberWindowContentParent.transform.GetChild(i).gameObject);
        }

        listedWorkers = WorkerManager.Instance.allWorkerList;
        int totalWorker = WorkerManager.Instance.allWorkerList.Count;
        workerInfos.Clear();

        float onePanelHeight = 45;
        float plusMemberWindow = onePanelHeight * totalWorker;
        MemberWindowContentParent.GetComponent<RectTransform>().sizeDelta = new Vector2(MemberWindowContentParent.GetComponent<RectTransform>().sizeDelta.x, (plusMemberWindow));


        if (listedWorkers.Count != 0)
        {
            for (int i = 0; i < listedWorkers.Count; i++)
            {
                Worker curWorker = listedWorkers[i];
                GameObject newWorkerPanel = Instantiate(workerHouseWorkerInformationPanelPrefab, MemberWindowContentParent.transform);
                WorkerHouseInformationPanel newWorkerInformationPanel =
                    newWorkerPanel.GetComponent<WorkerHouseInformationPanel>();
                workerInfos.Add(newWorkerInformationPanel);
                //---MUST--//
                newWorkerInformationPanel.WorkerId = curWorker.workerId;

                newWorkerInformationPanel.Status = curWorker.isAvailable();
                newWorkerInformationPanel.Satiety = (curWorker.currentSatiety * 100) / Worker.maxSatiety;
                newWorkerInformationPanel.Attrition = (curWorker.currentAttrition * 100) / Worker.maxAttritionLimit;
                newWorkerInformationPanel.Salary = curWorker.salary;
                newWorkerInformationPanel.StartDayOfWork = curWorker.startDayOfWork;


            }
        }



    }


    //WorkerHouseUI dönüş tipinin sebebi chain function olarak kullanabilmek.
    public WorkerHouseUI refreshMember(string member)
    {
        for (var i = 0; i < workerInfos.Count; i++)
        {
            var whip = workerInfos[i];
            if (whip.WorkerId == member)
            {
                DestroyImmediate(whip.gameObject);
                workerInfos.Remove(whip);
            }
        }

        return this;
    }

    public void updateUIData()
    {


        if (workerInfos.Count != 0)
        {
            //Worker listesi yenileniyor.
            listedWorkers = WorkerManager.Instance.allWorkerList;

            for (int i = 0; i < workerInfos.Count; i++)
            {
                Worker curWorker = listedWorkers[i] as Worker;
                if (curWorker != null)
                {
                    workerInfos[i].Status = curWorker.isAvailable();
                    workerInfos[i].Satiety = (curWorker.currentSatiety * 100) / Worker.maxSatiety;
                    workerInfos[i].Attrition = (curWorker.currentAttrition * 100) / Worker.maxAttritionLimit;
                    workerInfos[i].Salary = curWorker.salary;
                    workerInfos[i].StartDayOfWork = curWorker.startDayOfWork;
                }
            }
        }

    }



}
