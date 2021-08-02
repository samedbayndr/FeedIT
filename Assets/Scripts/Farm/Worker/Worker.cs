using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum WorkerLocation
{
    onFarm,
    onPasture
}

public class CurrentJob
{
    public JobSO currentJobSO;
    public int processCount;
    public double calculatedTime;
}
public class Worker : MonoBehaviour
{

    private const double hungryAttrition = 0.10;
    private const double homelessAttrition = 1;
    private const double salaryPaymentAttrition = 7.5;
    public const double maxAttritionLimit = 10;

    public const double maxSatiety = 100;
    public const double oneBiteNutrition = 10;
    public const double maxHealth = 100;

    public string workerId;
    public string workerType;
    public double efficiencyMultiplier;
    public double currentAttrition;
    public double currentSatiety;
    public double currentHealth;
    public double salary;

    public int startDayOfWork;

    public bool isEnterFarm;



    public State currentStates { get; set; }
    private StateList lastState;
    public WorkerLocation workerLocation { get; set; }

    public NavMeshAgent workerNavAgent;
    public CurrentJob currentJob { get; set; } = new CurrentJob();

    public Animator workerAnimator;

    void Start()
    {
        currentStates = new State();
        workerLocation = WorkerLocation.onFarm;
        lastState = StateList.restingOnHouse;
        currentStates.setState(StateList.restingOnHouse);

        workerNavAgent = GetComponent<NavMeshAgent>();
        workerAnimator = GetComponent<Animator>();


        GameTimeManager.MinuteFinishedEvent.AddListener(eat);
        GameTimeManager.HourFinishedEvent.AddListener(digestAte);
        GameTimeManager.MorningEightEvent.AddListener(wantSalary);

    }

    void OnTriggerEnter(Collider collider)
    {
        // if grasslande girerse 
        if (collider.tag == "Grassland")
        {

        }

        if (collider.tag == "Farm")
        {

            isEnterFarm = true;
            ShepherdUI.Instance.goRestBtn.interactable = true;
            var btnColors = ShepherdUI.Instance.goRestBtn.GetComponent<Button>().colors;
            btnColors.normalColor = new Color(ShepherdUI.Instance.goRestBtn.colors.normalColor.r, 
                ShepherdUI.Instance.goRestBtn.colors.normalColor.g, 
                ShepherdUI.Instance.goRestBtn.colors.normalColor.b, 1f);
            ShepherdUI.Instance.goRestBtn.GetComponent<Button>().colors = btnColors;
        }


    }

    void OnTriggerExit(Collider collider)
    {
        // if grasslanden çıkarsa
        if (collider.tag == "Grassland")
        {

        }

        if (collider.tag == "Farm")
        {
            isEnterFarm = false;
            ShepherdUI.Instance.goRestBtn.interactable = false;
            var btnColors = ShepherdUI.Instance.goRestBtn.GetComponent<Button>().colors;
            btnColors.normalColor = new Color(ShepherdUI.Instance.goRestBtn.colors.normalColor.r, 
                ShepherdUI.Instance.goRestBtn.colors.normalColor.g, 
                ShepherdUI.Instance.goRestBtn.colors.normalColor.b, 0.5f);
            ShepherdUI.Instance.goRestBtn.GetComponent<Button>().colors = btnColors;
        } 
        
    }

    // Update is called once per frame


    //Worker


    //{StateList.workerWalking, 10},
    //{ StateList.workerIdle, 10},
    //{ StateList.workingOnFarm, 10},
    //{ StateList.restingOnHouse, 10},
    //{ StateList.animalGrazing, 10},
    //{ StateList.instrumentPlaying, 15},
    void Update()
    {
        #region State Exit
        if (lastState != currentStates.getState())
        {
            // last state için exit çalışıyor...
            if (lastState != StateList.neutral)
            {
                //Exit Functions calling
                switch (lastState)
                {
                    case StateList.workerIdle:
                        exitWorkIdleState();
                        break;
                    case StateList.workerWalking:
                        exitWorkerWalkingState();
                        break;

                    case StateList.workingOnFarm:
                        exitWorkingOnFarmState();
                        break;

                    case StateList.restingOnHouse:
                        exitRestingOnHouseState();
                        break;
                    //StateList.workerIdle || StateList.workerWalking && StateList.instrumentPlaying
                    case StateList.instrumentPlaying:
                        exitInstrumentPlayingState();
                        break;
                }
            }

            //Yeni state alınıyor...
            lastState = currentStates.getState();
            switch (lastState)
            {

                case StateList.workerIdle:
                    idle();
                    break;
                case StateList.workerWalking:
                    walk();
                    break;

                case StateList.workingOnFarm:
                    workingOnFarm();
                    break;

                case StateList.restingOnHouse:

                    break;
                //StateList.workerIdle || StateList.workerWalking && StateList.instrumentPlaying
                case StateList.instrumentPlaying:

                    break;


            }

        }

        #endregion

        //TEST ANIMATION 
        workerAnimator.SetFloat("ShepherdSpeed", workerNavAgent.desiredVelocity.magnitude);

    }

    #region Time based functions

    public void eat()
    {
        if (currentStates.getState() == StateList.restingOnHouse)
        {
            double satietyPercentage = (100 * this.currentSatiety) / Worker.maxSatiety;
            if (satietyPercentage <= 20 || !(satietyPercentage > 80))
            {

                // oneBiteNutrition/10 yapmamızın sebebi henüz tam denge oturtulmadı ancak
                // insanlar, hayvanlar kadar yemek odaklı canlılar olmadıkları için
                // hesaplamalar oturtulana kadar oneBiteNutrition/10 oranında stock azalacaktır.
                if (WorkerHouseManager.Instance.decreaseMealStock(oneBiteNutrition / 10))
                {
                    this.currentSatiety += oneBiteNutrition;
                }
            }

        }
    }

    public void digestAte()
    {
        double satietyPercentage = (100 * this.currentSatiety) / Worker.maxSatiety;
        if (satietyPercentage > 5)
        {

            double amountOfDigestAte = -oneBiteNutrition;
            this.currentSatiety += amountOfDigestAte;
            //Eğer tokluk oranı %5 üstündeyse ve zaten sindirim yaptıysa açlık attritionu bir miktar düşüyor.
            this.decreaseHungryAttrition();
        }
        else if (satietyPercentage <= 5)
        {
            this.increaseHungryAttrition();
        }

    }

    public void wantSalary()
    {
        int progressPayment = GameTimeManager.Instance.getDay() - startDayOfWork;
        if (progressPayment != 0 && progressPayment % 30 == 0)
        {
            Debug.Log("Mayış günü!!");


            if (Player.Instance.getMoney() > this.salary)
            {
                Player.Instance.decreaseMoney(this.salary);
            }
            else
            {
                increaseSalaryPaymentAttrition();
            }
        }
    }

    #region Attrition Things

    public void increaseHungryAttrition()
    {
        currentAttrition += Worker.hungryAttrition;
        checkAttritionLimit();

    }

    public void increaseHomelessAttrition()
    {
        currentAttrition += Worker.homelessAttrition;
        checkAttritionLimit();

    }

    public void increaseSalaryPaymentAttrition()
    {
        currentAttrition += Worker.salaryPaymentAttrition;
        checkAttritionLimit();

    }

    public void decreaseHungryAttrition()
    {
        if (currentAttrition == 0)
        {
            return;
        }
        this.currentAttrition -= (Worker.hungryAttrition * 2);
        if (this.currentAttrition < 0)
        {
            this.currentAttrition = 0;
        }
    }

    public void checkAttritionLimit()
    {
        if (currentAttrition >= Worker.maxAttritionLimit)
        {
            WorkerManager.Instance.quitWorker(this);
        }
    }

    #endregion


    #endregion


    #region State Functions
    private void idle()
    {

    }
    private void walk()
    {
        walkCoroutine = StartCoroutine(walkRoutine());
    }

    private void workingOnFarm()
    {
        if (currentStates.getState() == StateList.workingOnFarm && this.currentJob != null)
        {
            GameTimeManager.MinuteFinishedEvent.AddListener(work);
        }
    }

    public int workPassingTime = 0;
    public void work()
    {
        workPassingTime++;
        if (Math.Round(currentJob.calculatedTime) == workPassingTime)
        {
            GameTimeManager.MinuteFinishedEvent.RemoveListener(work);

            //TODO: Burada WorkerManager.freeWorker() fonksiyonunu kullanman gerekiyor!!!
            WorkerManager.Instance.freeWorker(this);
            //currentJob = null;

        }
    }

    #endregion

    #region State Exit Functions
    private void exitWorkIdleState()
    {
    }

    private void exitWorkerWalkingState()
    {
        StopCoroutine(walkCoroutine);
    }

    private void exitWorkingOnFarmState()
    {
    }

    private void exitRestingOnHouseState()
    {
    }

    private void exitInstrumentPlayingState()
    {
    }
    #endregion

    public void goToRest()
    {
        if (!isEnterFarm)
            return;
        workerNavAgent.isStopped = true;
        workerNavAgent.ResetPath();
        WorkerManager.Instance.freeCurrentShepherd();

    }

    public bool isAvailable()
    {
        if (this.currentStates.getState() == StateList.restingOnHouse)
            return true;
        else
            return false;
    }


    private Coroutine walkCoroutine;
    private Coroutine agentArriveCheckCoroutine;
    #region State Coroutine


    // Walk Routine
    private IEnumerator walkRoutine()
    {
        while (true)
        {
            while (!Input.GetMouseButtonDown(0) || EventSystem.current.IsPointerOverGameObject())
                yield return null;

            //Sol click yapıldığında çalışan aralık

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                Debug.Log(raycastHit.collider.name);
                workerNavAgent.ResetPath();
                NavMeshPath path = new NavMeshPath();

                workerNavAgent.CalculatePath(raycastHit.point, path);
                //workerNavAgent.destination = raycastHit.point;
                workerNavAgent.SetPath(path);
                agentArriveCheckCoroutine = StartCoroutine(agentArriveCheckRoutine());
            }


            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator agentArriveCheckRoutine()
    {
        //Burada yürüme animasyonun geçiş yapılacak.
        Debug.Log(NextGenDebug.HeavyError("Yürüme animasyonuna geçti!"));
        bool isReached = false;
        while (!isReached)
        {

            if (workerNavAgent.hasPath)
            {
                try
                {
                    if (!workerNavAgent.pathPending )
                    {

                        if (workerNavAgent.hasPath && workerNavAgent.remainingDistance <= workerNavAgent.stoppingDistance)
                        {
                            if (!workerNavAgent.hasPath || workerNavAgent.velocity.sqrMagnitude == 0f)
                            {
                                isReached = true;
                                
                                Debug.Log(NextGenDebug.HeavyError("Idle animasyonuna geçti!"));
                                // Burada Idle animasyonuna geçiş yapılacak.
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(NextGenDebug.HeavyError(e.Message));
                }
            }
            yield return new WaitForEndOfFrame();

        }
        yield return new WaitForEndOfFrame();
    }

    //--Walk Routine --//

    #endregion


}
