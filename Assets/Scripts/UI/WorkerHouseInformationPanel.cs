
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkerHouseInformationPanel : MonoBehaviour
{
    //Bu değişkenin modify ettiği gameobject daima invisible olmalı!!
    [SerializeField] private TextMeshProUGUI workerIdHiddenText;

    [SerializeField] private TextMeshProUGUI status;
    [SerializeField] private TextMeshProUGUI satietyPercentage;
    [SerializeField] private TextMeshProUGUI attritionPercentage;
    [SerializeField] private TextMeshProUGUI salary;
    [SerializeField] private TextMeshProUGUI startDayOfWork;
    [SerializeField] private Button actionButton;
    //[SerializeField] private TMP_Dropdown actionDropdown;

    private string _workerId;
    private bool _status;
    private double _satiety;
    private double _attrition;
    private double _salary;
    private int _startDayOfWork;

    public bool Status
    {
        get
        {
            return _status;
        }
        set
        {
            _status = value;
            if (_status == true)
            {
                status.SetText("Available");
            }
            else if (_status == false)
            {
                status.SetText("Busy");
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
    public double Salary
    {
        get
        {
            return _salary;
        }
        set
        {
            _salary = value;
            salary.SetText(_salary.ToString("F") + "$");
        }
    }
    public int StartDayOfWork
    {
        get
        {
            return _startDayOfWork;
        }
        set
        {
            _startDayOfWork = value;
            startDayOfWork.SetText(_startDayOfWork.ToString());
        }
    }
    public string WorkerId
    {
        get
        {
            return _workerId;
        }
        set
        {
            _workerId = value;
            workerIdHiddenText.SetText(_workerId);
        }
    }


    public void performAction()
    {
        string workerId = workerIdHiddenText.text;
        Worker curWorker = WorkerManager.Instance.getWorker(workerId);
        if (WorkerManager.Instance.quitWorker(curWorker))
        {
            WorkerHouseUI.Instance.refreshMember(workerId).updateWorkerHouseMemberUI();
            LogPanelUI.Instance.addLog("You fired worker!");
        }


    }
}
