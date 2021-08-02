using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerUtilityManager : MonoBehaviour
{
    private static WorkerUtilityManager _instance;

    public static WorkerUtilityManager Instance
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

    public void buyWorker(WorkerSO workerSO)
    {
        if (workerSO != null)
        {
            if (Player.Instance.getMoney() >= workerSO.cost)
            {
                if (!WorkerHouseManager.Instance.areThereCapacity())
                {
                    MessageSentenceThrower.Instance.fireMessage("No place on the worker house");
                    return;
                }
                //TODO: Burada parayı kullanıcıdan düş ve güncelle
                Player.Instance.decreaseMoney(workerSO.cost);


             


                switch (workerSO.workerType)
                {
                    case WorkerType.NormalWorker:

                        GameObject newNWorker = Instantiate(workerSO.workerPrefab, WorkerManager.Instance.workerParent.transform);
                        Worker newWorker = newNWorker.GetComponent<Worker>();

                        string workerNId = IdGenerator.generateGUID();
                        //Animalda bu kısım sadece IDden ibaret. Bunun sebebi şudur; Worker sınıfı Animalda olduğu gibi hiyerarşik bir şekilde parçalanmadı.
                        //Yani normal worker da "Worker" sınıfını kullanacak. Diğer yaratılacak worker tipleri de "Worker" sınıfını kullanacak. 
                        //Animal sınıfında ise "Chicken", "Rooster" gibi hiyerarşi aşağı doğru inmektedir.
                        newWorker.workerId = workerNId;
                        newWorker.efficiencyMultiplier = workerSO.efficiencyModifier;
                        newWorker.salary = workerSO.salary;
                        newWorker.startDayOfWork = GameTimeManager.Instance.getDay();
                        newWorker.currentSatiety = Worker.maxSatiety;
                        newWorker.currentHealth = Worker.maxHealth;
                        WorkerEntity newNWEntity = new WorkerEntity(workerNId, WorkerType.NormalWorker, Worker.maxSatiety, Worker.maxHealth, 0, workerSO.salary, GameTimeManager.Instance.getDay(), workerSO.efficiencyModifier);

                        WorkerManager.Instance.allWorkerList.Add(newWorker);
                        Player.Instance.Economy.monthlyExpenses += newWorker.salary;
                        TableManager.Instance.ownedWorkerTable.AllWorkers.Add(newNWEntity);
                        FileOperation.SaveTextAsset(TableManager.Instance.ownedWorkerTable,
                            TableManager.Instance.ownedWorkerTable.filePath, Extension.Json);

                        break;
                }
                
            }
            else
            {
                MessageSentenceThrower.Instance.fireMessage("You have not enough money!");
            }
        }
    }
}
