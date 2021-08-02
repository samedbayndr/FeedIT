using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class PastureAnimal : Animal
{
    protected bool isOnTheWay { get; set; }
    protected float followDistance { get; set; } = 10.0f;
    protected float eatingDistance { get; set; } = 0.25f;

    public float eatingSpeed = 0f;
    public float walkingSpeed = 5f;
    public float searchingSpeed = 2.5f;
    public float sightDistance = 50f;

    public double oneBiteNutrition = 5;
    


    public bool isForcedFollow;

    public List<GameObject> collidedGOs = new List<GameObject>();
    public State currentStates { get; set; } = new State();


    //Inspector panelden debug yapabilmek için eklendi
    public string stateText;

    private StateList dummyLastState;
    private StateList lastState
    {
        get
        {
            return dummyLastState;
        }
        set
        {
            dummyLastState = value;
        }
    }


    public List<Tussock> visitableTussocks = new List<Tussock>();

    public Tussock fedTussock { get; set; }

    public NavMeshAgent animalAgent;
    private GameObject visitedGrassland;

    public AudioSource animalAudioSource;
    protected virtual void Start()
    {
        animalAgent = GetComponent<NavMeshAgent>();
        animalAgent.updatePosition = true;
        animalAgent.updateRotation = true;
        animalLocation = AnimalLocation.onBarn;

        lastState = StateList.restingOnBarn;
        currentStates.setState(StateList.restingOnBarn);

        GameTimeManager.TenMinuteFinishedEvent.AddListener(digestAte);

        animalAudioSource = this.GetComponent<AudioSource>();
    }

    public void setPosition(Vector3 newPos)
    {
        animalAgent.enabled = false;
        this.transform.position = newPos;
        animalAgent.enabled = true;

    }

    void OnTriggerEnter(Collider collider)
    {
        collidedGOs.Add(collider.gameObject);
        // if grasslande girerse 
        if (collider.tag == "Grassland")
        {
            visitedGrassland = collider.gameObject;
            updateTussocksInformation(visitedGrassland);
            currentStates.setState(StateList.feedSearching);

        }
        // if ot öbeğine girerse
        else if (collider.tag == "Tussock")
        {

        }
    }

    void OnTriggerExit(Collider collider)
    {
        collidedGOs.Remove(collider.gameObject);
        // if grasslanden çıkarsa
        if (collider.tag == "Grassland")
        {
            visitableTussocks.Clear();
            visitedGrassland = null;
        }
        // if ot öbeğine girerse
        else if (collider.tag == "Tussock")
        {
            fedTussock = null;
        }
    }


    //Preview 
    //{StateList.breeding, int.MaxValue},
    //{ StateList.eating, 10},
    //{ StateList.feedSearching, 10},
    //{ StateList.shepherdFollowing, 9999999},
    //{ StateList.milking, int.MaxValue},
    //{ StateList.cropping, int.MaxValue},
    //{ StateList.restingOnBarn, 999},
    //{ StateList.pastureAnimalWalking, 999999},
    //{ StateList.pastureAnimalIdle, 10},
    protected virtual void Update()
    {

        if (isForcedFollow)
        {
            currentStates.setState(StateList.shepherdFollowing);
        }
        if (lastState != currentStates.getState())
        {
            switch (lastState)
            {

                //StateList.breeding & StateList.restingOnBarn:
                case StateList.breeding:
                    exitBreedingState();
                    break;
                case StateList.eating:
                    exitEatingState();
                    break;

                //StateList.feedSearching & StateList.pastureAnimalWalking:
                case StateList.feedSearching:
                    exitFeedSearchingState();
                    break;

                //StateList.shepherdFollowing & StateList.pastureAnimalWalking:
                case StateList.shepherdFollowing:
                    exitShepherdFollowingState();
                    break;

                //StateList.cropping | StateList.restingOnBarn:
                case StateList.cropping:
                    exitCroppingState();
                    break;


                case StateList.restingOnBarn:
                    exitRestingOnBarnState();
                    break;

                case StateList.pastureAnimalIdle:
                    StopCoroutine(pastureAnimalIdleCoroutine);
                    break;

                
            }

            //Yeni state alınıyor...
            lastState = currentStates.getState();
            stateText = lastState.ToString();
            switch (lastState)
            {

                //StateList.breeding & StateList.restingOnBarn:
                case StateList.breeding:

                    break;
                case StateList.eating:

                    break;

                //StateList.feedSearching & StateList.pastureAnimalWalking:
                case StateList.feedSearching:
                    // BURADA STOP DISTANCE TEKRAR AYARLA!!!!
                    searchFeed();
                    break;

                //StateList.shepherdFollowing & StateList.pastureAnimalWalking:
                case StateList.shepherdFollowing:

                    followShepherd();

                    break;

                //StateList.cropping | StateList.restingOnBarn:
                case StateList.cropping:
                    if (animalLocation == AnimalLocation.onBarn)
                    {

                    }
                    break;


                case StateList.restingOnBarn:
                    animalLocation = AnimalLocation.onBarn;

                    break;

                case StateList.pastureAnimalIdle:
                    animalLocation = AnimalLocation.onPasture;
                    
                    pastureAnimalIdleCoroutine = StartCoroutine(pastureAnimalIdleRoutine()); 
                    break;
            }

        }


    }




    #region State Exit Functions
    private void exitRestingOnBarnState()
    {
        Debug.Log("EXIT RestingOnBarn State");
    }

    private void exitCroppingState()
    {
        throw new System.NotImplementedException();
    }

    private void exitShepherdFollowingState()
    {
        //Burada hayvanlar eve döndüklerinde following state çalışıyorsa takibi bırakmaları için..
        try
        {
            isOnTheWay = false;
            //animalAgent.ResetPath();
            //animalAgent.isStopped = true;
        }
        catch (Exception e)
        {

        }
        Debug.Log("EXIT RestingOnBarn State");
    }

    private void exitEatingState()
    {
        Debug.Log("EXIT Eating State");
        //   throw new System.NotImplementedException();
    }

    private void exitBreedingState()
    {
        //throw new System.NotImplementedException();
    }

    private void exitFeedSearchingState()
    {
        Debug.Log("EXIT feed searching state");

        //   throw new System.NotImplementedException();
    }


    #endregion

    #region State Functions

    public void followShepherd()
    {
        animalAgent.speed = walkingSpeed;
        //animalAgent.stoppingDistance = followDistance;
        shepherdFollowCoroutine = StartCoroutine(shepherdFollowRoutine());
    }

    public void searchFeed()
    {

        if (visitableTussocks.Count == 0)
        {
            Debug.Log(NextGenDebug.HeavyError("There aren't grass! Updating..."));

            updateTussocksInformation(visitedGrassland);
            //If still zero
            if (visitableTussocks.Count == 0)
                return;

        }
        animalAgent.speed = searchingSpeed;
        animalAgent.stoppingDistance = eatingDistance;
        if (!isSatietyMaximum())
        {
            goNutritionCoroutine = StartCoroutine(goNutritionRoutine());
        }
        else
        {
            currentStates.setState(StateList.shepherdFollowing);
        }
    }

    public void eat(double amountOfBite)
    {
        if (currentStates.getState() == StateList.eating)
        {

            if (this.updateSatiety(amountOfBite))
            {
                if (sex == AnimalSex.female)
                {
                    gainWeight((oneBiteNutrition * 5) / 100);
                }
                else if (sex == AnimalSex.male)
                {
                    gainWeight((oneBiteNutrition * 3) / 100);
                }

            }


        }

    }

    // Bu fonksiyon SheepGroup gibi bir altta veya her hayvan sınıfına özel işletilmelidir!!
    public void gainWeight(double gainedWeight)
    {
        if (sex == AnimalSex.female)
        {


            if (currentWeight + gainedWeight > AnimalMaxWeight.Sheep)
            {
                currentWeight = AnimalMaxWeight.Sheep;
            }
            else
            {
                currentWeight += gainedWeight;
            }
        }
        else if (sex == AnimalSex.male)
        {
            if (currentWeight + gainedWeight > AnimalMaxWeight.Ram)
            {
                currentWeight = AnimalMaxWeight.Ram;
            }
            else
            {
                currentWeight += gainedWeight;
            }
        }
    }

    public void digestAte()
    {
        double currentSatiety = this.getSatiety();
        double satietyPercentage = (100 * currentSatiety) / this.maxHealth;



        if (satietyPercentage > 5)
        {
            //Otlakta daha yavaş acıkıyor...
            double amountOfDigestAte = 0d;
            if (this.animalLocation == AnimalLocation.onPasture)
            {
                amountOfDigestAte = (-oneBiteNutrition) / 8;
            }
            else if (this.animalLocation == AnimalLocation.onBarn)
            {
                amountOfDigestAte = -oneBiteNutrition / 4;
            }
            if (this.updateSatiety(amountOfDigestAte))
            {
                //Debug.Log(this.gameObject.name + " Sindirdi!!!");
            }

            this.decreaseHungryAttrition();
        }
        else if (satietyPercentage <= 5)
        {
            this.increaseHungryAttrition();
        }


        //Bu if bloklarının amacı otlakta hayvanlar doyduktan sonra yapacağı eylemleri belirlemektir.
        if (Math.Round(satietyPercentage, 2, MidpointRounding.ToEven) == 100 && animalLocation == AnimalLocation.onPasture && visitedGrassland != null)
        {
            if (Vector3.Distance(WorkerManager.Instance.currentShepherd.transform.position, this.transform.position) < GrazingMode.Instance.shepherdVisiblityDistance)
            {
                AnimalEmotionUI.Instance.fireLoveEmotion(this.transform.position);
                currentStates.setState(StateList.shepherdFollowing);
            }
            else
            {
                currentStates.setState(StateList.pastureAnimalIdle);
            }
        }
        else if (satietyPercentage <= 50 && visitedGrassland != null && fedTussock == null)
        {
                
            currentStates.setState(StateList.feedSearching);
        }

    }

    //Fonksiyonun amacı hayvan otlakta karnını doyurduktan
    //sonra eğer çobanı görebilecek mesafede değilse random bir yere gidiyor
    public void goRandomAnywhere()
    {
        if (animalAgent.hasPath)
            animalAgent.ResetPath();

        NavMeshPath randomPath = new NavMeshPath();
        Vector3 randomPosition;

        int failCounter = 0;
        int maxFail = 3;
        while (failCounter < maxFail)
        {
            float randomX = Random.Range(-10, 10);
            float randomZ = Random.Range(-10, 10);
            //Random direction seçiyorsun.Ya mersine gidiyor ya da tersine.
            randomPosition = transform.position + (Vector3.right * randomX) + (Vector3.forward * randomZ);

            //randomPosition += transform.position;

            //Random positionda yeni bir tussock olmaması için ray fırlatıyoruz.
            Ray ray = Camera.main.ScreenPointToRay(randomPosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (animalAgent.CalculatePath(randomPosition, randomPath) && raycastHit.collider.tag != "Tussock" && raycastHit.collider.tag != "Farm")
                {
                    Debug.Log(randomPath.status);
                    animalAgent.SetPath(randomPath);
                    break;
                }
            }

            failCounter++;
        }



    }


    public void goToRest(Vector3 restPosition)
    {
        animalAgent.ResetPath();
        animalAgent.isStopped = true;
        setPosition(restPosition);
        currentStates.setState(StateList.restingOnBarn);
        stopAnimalVoice();
    }

    public double getOneBiteNutrition()
    {
        return oneBiteNutrition;
    }


    public void playAnimalVoice()
    {
        playAnimalVoiceCoroutine = StartCoroutine(playAnimalVoiceRoutine());
    }

    public void stopAnimalVoice()
    {
        if (playAnimalVoiceCoroutine != null)
            StopCoroutine(playAnimalVoiceCoroutine);
    }
    #endregion

    private Coroutine agentArriveCheckCoroutine;
    private Coroutine shepherdFollowCoroutine;
    private Coroutine goNutritionCoroutine;
    private Coroutine pastureAnimalIdleCoroutine;
    private Coroutine playAnimalVoiceCoroutine;
    #region State Coroutine

    public IEnumerator pastureAnimalIdleRoutine()
    {
        while (true)
        {
            goRandomAnywhere();
            float randomDuration = Random.Range(15, 60);
            yield return new WaitForSeconds(randomDuration);

        }
    }
    
    public IEnumerator shepherdFollowRoutine()
    {
        //Eğer hayvan 30 saniyeden daha uzun bir süre çobanı takip ederken hareketsiz kalırsa dikkati dağılıyor.

        float boringTime = 0;
        float maxBoringDuration = Random.Range(15, 60);
        while (true)
        {
            if (currentStates.getState() == StateList.shepherdFollowing)
            {
                try
                {

                    if (WorkerManager.Instance.shepherdFlag == true)
                    {
                        Vector3 shepherdPosition = WorkerManager.Instance.currentShepherd.transform.position;

                        //Güvenlik amaçlı yazılmış bir if state
                        if (isOnTheWay == true && !animalAgent.hasPath)
                            isOnTheWay = false;

                        float shepherdDistance = Vector3.Distance(shepherdPosition, this.transform.position);

                        if (shepherdDistance > GrazingMode.Instance.shepherdVisiblityDistance)
                        {
                            currentStates.setState(StateList.pastureAnimalIdle);
                        }
                        else if (shepherdDistance > this.followDistance && isOnTheWay == false)
                        {
                            //Debug.Log(shepherdPosition); 
                            this.animalAgent.destination = shepherdPosition;
                            this.isOnTheWay = true;
                            agentArriveCheckCoroutine = StartCoroutine(agentArriveCheckRoutine());
                            boringTime = 0f;

                        }
                        //Boring...
                        else if (boringTime < maxBoringDuration)
                        {
                            boringTime += Time.deltaTime;
                            if (boringTime > maxBoringDuration)
                            {
                                currentStates.setState(StateList.pastureAnimalIdle);
                                AnimalEmotionUI.Instance.fireUnwillingEmotion(this.transform.position);
                            }
                        }

                    }
                    else
                        currentStates.setState(StateList.pastureAnimalIdle);
                }
                catch (Exception e)
                {
                    Debug.Log(NextGenDebug.HeavyError(e.Message));
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator agentArriveCheckRoutine()
    {
        while (isOnTheWay)
        {
            if (animalAgent.hasPath)
            {
                if (!animalAgent.pathPending)
                {
                    if (animalAgent.remainingDistance <= animalAgent.stoppingDistance + followDistance)
                    {
                        //if (!animalAgent.hasPath || animalAgent.velocity.sqrMagnitude == 0f)
                        //{
                            this.isOnTheWay = false;
                            Debug.Log(NextGenDebug.HeavyError("Hayvan Idle animasyonuna geçti!"));
                            animalAgent.isStopped = true;
                            animalAgent.ResetPath();

                            // Burada Idle animasyonuna geçiş yapılacak.
                            //}

                    }
                }
            }

            yield return new WaitForSeconds(2f);
        }

        yield return new WaitForEndOfFrame();
    }

    public IEnumerator goNutritionRoutine()
    {
        while (fedTussock == null && isForcedFollow != true)
        {

            if (currentStates.getState() == StateList.feedSearching)
            {
                Transform nearestTussockTransform = findNearestTussock();
                if (nearestTussockTransform != null)
                {
                    //Debug.Log("Grid: " + nearestTussockTransform.parent.name + " /// Tussock: " + nearestTussockTransform.name);
                    if (Vector3.Distance(animalAgent.destination, nearestTussockTransform.position) > 1)
                    {
                        animalAgent.ResetPath();
                        NavMeshPath tussockPath = new NavMeshPath();
                        animalAgent.CalculatePath(nearestTussockTransform.position, tussockPath);
                        if (animalAgent.SetPath(tussockPath))
                            Debug.Log(animalAgent.pathStatus);
                        
                    }
                }
                else
                {
                    updateTussocksInformation(this.visitedGrassland);
                }

            }

            yield return new WaitForEndOfFrame();
        }

    }

    public IEnumerator playAnimalVoiceRoutine()
    {
        while (animalLocation == AnimalLocation.onPasture )
        {
            float randomTime = Random.Range(10, 45);
            yield return new WaitForSeconds(randomTime);

            animalAudioSource.volume = TableManager.Instance.audioSettingTable.environmentLevel;
            animalAudioSource.Play();
        }

        Debug.Log("Animal Pasture lokasyonunda değil");
    }

    #endregion




    public void updateTussocksInformation(GameObject visitedGrassland)
    {

        visitableTussocks.Clear();
        for (int i = 0; i < visitedGrassland.transform.childCount; i++)
        {
            if (visitedGrassland.transform.GetChild(i).name.Contains("Grid"))
            {
                List<Tussock> gridTussocks =
                    visitedGrassland.transform.GetChild(i).GetComponent<GrasslandGrid>().tussockList;
                foreach (var tussock in gridTussocks)
                {
                    visitableTussocks.Add(tussock);
                }
            }
        }
    }

    public void dropTussock(Tussock tussock)
    {
        if (visitableTussocks.Count != 0)
        {
            visitableTussocks.Remove(tussock);
        }
    }

    public void exitFromTussock(Tussock tussock)
    {

        Debug.Log("Tussock bitti veya çok kalabalık! Çıkılıyor...");
        fedTussock = null;
        dropTussock(tussock);
        addAI();

        if (this.currentStates.getState() != StateList.shepherdFollowing)
            this.currentStates.setState(StateList.feedSearching);


    }

    public void addAI()
    {
        if (this.GetComponent<NavMeshAgent>() == null)
        {
            this.gameObject.AddComponent<NavMeshAgent>();
        }
    }
    public Transform findNearestTussock()
    {
        Vector3 animalPosition = this.transform.position;

        Transform nearestTussock = null;
        float minDistance = float.MaxValue;
        if (visitableTussocks.Count != 0)
        {
            foreach (Tussock visitableTussock in visitableTussocks)
            {
                float distance = Vector3.Distance(animalPosition, visitableTussock.transform.position);
                if (distance < minDistance && visitableTussock.isTussockAvailable())
                {
                    nearestTussock = visitableTussock.transform;
                    minDistance = distance;
                }
            }

            return nearestTussock;
        }

        Debug.Log(NextGenDebug.HeavyError("visitableTussocks listesi boş!"));
        return nearestTussock;
    }






}
