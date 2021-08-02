using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    #region Singleton Things

    


    private static WorkerManager _instance;

    public static WorkerManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }
    #endregion

    public GameObject workerParent;
    public List<Worker> allWorkerList = new List<Worker>();
    [HideInInspector]
    public Worker currentShepherd;

    public bool shepherdFlag = false;
    public List<JobSO> farmJobs = new List<JobSO>();


    public GameObject normalWorkerPrefab;

    public void Start()
    {
        generateWorkers();
    }



    public void generateWorkers()
    {
        OwnedWorker ownedWorker = TableManager.Instance.ownedWorkerTable;

        foreach (var worker in ownedWorker.AllWorkers)
        {
            switch (worker.workerType)
            {
                case WorkerType.NormalWorker:
                    GameObject normalWorkerGen = Instantiate(normalWorkerPrefab, workerParent.transform);
                    Worker curWorker = normalWorkerGen.GetComponent<Worker>();
                    curWorker.workerId = worker.workerId;
                    curWorker.currentHealth = worker.health;
                    curWorker.currentSatiety = worker.satiety;
                    curWorker.currentAttrition = worker.attrition;
                    curWorker.efficiencyMultiplier = worker.efficiencyModifier;
                    curWorker.salary = worker.salary;
                    curWorker.startDayOfWork = worker.startDayOfWork;

                    allWorkerList.Add(curWorker);
                    Player.Instance.Economy.monthlyExpenses += curWorker.salary;
                    break;

            }
        }
    } 


    public JobSO getJob(string jobType)
    {
        foreach (var job in farmJobs)
        {
            if (job.jobType == jobType)
            {
                return job;
            }
        }

        return null;
    }


    public Worker getAvailableWorkerForShepherd()
    {
        //Zaten bir çoban var 
        if (shepherdFlag)
            return null;

        
        for (int i = 0; i < allWorkerList.Count; i++)
        {
            if (allWorkerList[i].currentStates.getState() == StateList.restingOnHouse)
            {
                attachShepherdToWorker(allWorkerList[i]);

                currentShepherd = allWorkerList[i];

                currentShepherd.workerLocation = WorkerLocation.onPasture;
                currentShepherd.currentStates.setState(StateList.workerWalking);
                shepherdFlag = true;
                return allWorkerList[i];
            }
        }
        Debug.Log(NextGenDebug.NormalError("There are no suitable employees!"));
        //ErrorMessageUI.Instance.openErrorPanel("There are no suitable employees!");
        return null;
    }


    public bool areThereAvailableWorker()
    {
        foreach (Worker worker in allWorkerList)
        {
            if (worker.currentStates.getState() == StateList.restingOnHouse)
            {
                return true;
            }
        }
        Debug.Log(NextGenDebug.HeavyError("İşçi yok!"));
        return false;
    }

    public int getAvailableWorkersCount()
    {
        int count= 0;
        foreach (Worker worker in allWorkerList)
        {
            if (worker.currentStates.getState() == StateList.restingOnHouse)
            {
                count++;
            }
        }
        return count;
    }

    public Worker getWorker(string workerId)
    {
        return allWorkerList.Find(a => a.workerId == workerId);
    }
    public void freeWorker(Worker workingWorker)
    {
        workingWorker.currentStates.setState(StateList.restingOnHouse);
        workingWorker.workPassingTime = 0;
    }
    public void freeCurrentShepherd()
    {
        CameraManager.Instance.changeCameraToFree();
        currentShepherd.tag = "NormalWorker";
        currentShepherd.workerLocation = WorkerLocation.onFarm;

        freeWorker(currentShepherd);
        deattachShepherdFromWorker(currentShepherd);

        ShepherdUI.Instance.closeShepherdUI();
        SheepBarnUI.Instance.GoGrazeBtn.SetActive(true);
        WorkerHouseUI.Instance.GoPastureBtn.SetActive(true);

        setAIWorkerPosition(currentShepherd,Farm.Instance.initSpawn.position);

        shepherdFlag = false;
        //Shepherd skilleri kapatılıyor.
        SkillManager.Instance.makeSkillsUnusable();

    }
    
    //Navmesh agentlarının pozisyonlarını doğrudan transform.position diyerek editlemek mümkün değil.
    //Bu yüzden önce üzerlerindeki navmeshagent componentini disable ettin. Sonra pozisyon editleyip tekrar aktif hale getirdin.
    public void setAIWorkerPosition(Worker worker, Vector3 newPos)
    {
        worker.workerNavAgent.enabled = false;
        worker.transform.position = newPos;
        worker.workerNavAgent.enabled = true;
    }

    public void attachShepherdToWorker(Worker worker)
    {
        worker.gameObject.AddComponent<ShepherdTestScript>();
    }

    public void deattachShepherdFromWorker(Worker worker)
    {
        Destroy(worker.gameObject.GetComponent<ShepherdTestScript>());
    }

    public Worker getAvailableWorkerForFarmJobs(JobSO jobSO, int processCount)
    {
        for (int i = 0; i < allWorkerList.Count; i++)
        {
            if (allWorkerList[i].currentStates.getState() == StateList.restingOnHouse)
            {
                allWorkerList[i].currentJob.currentJobSO = jobSO;
                allWorkerList[i].currentJob.processCount = processCount;
                allWorkerList[i].currentJob.calculatedTime = jobSO.perProcessTimeCost * processCount;

                allWorkerList[i].currentStates.setState(StateList.workingOnFarm);

                return allWorkerList[i];
            }
        }
        ErrorMessageUI.Instance.openErrorPanel("There are no suitable employees!");
        Debug.Log(NextGenDebug.NormalError("There are no suitable employees!"));
        return null;
    }


    public bool quitWorker(Worker worker)
    {
        WorkerEntity workerEntity = TableManager.Instance.ownedWorkerTable.AllWorkers.Find(a => a.workerId == worker.workerId);
        if (workerEntity != null)
        {
            allWorkerList.Remove(worker);
            TableManager.Instance.ownedWorkerTable.AllWorkers.Remove(workerEntity);
            FileOperation.SaveTextAsset(TableManager.Instance.ownedWorkerTable,
                TableManager.Instance.ownedWorkerTable.filePath, Extension.Json);
            
            //Eğer işten ayrılırken otlaktaysa...
            if (worker.workerLocation == WorkerLocation.onPasture)
            {
                freeCurrentShepherd();
            }

            Destroy(worker.gameObject);
            return true;
        }

        return false;
    }


}
