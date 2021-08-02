using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Kangal : Dog
{


    protected float followDistance { get; set; } = 5f;
    protected Animator dogAnimator;
    protected override void Start()
    {
        base.Start();
        dogAnimator = GetComponent<Animator>();

    }

    public static float baseForceFollowModifier = 1.2f;


    // Update is called once per frame
    void Update()
    {
        if (lastState != currentStates.getState())
        {
            switch (lastState)
            {

                //StateList.breeding & StateList.restingOnBarn:
                case StateList.feedingBait:
                    exitFeedingBait();
                    break;
                case StateList.free:
                    exitFree();
                    break;
                //StateList.feedSearching & StateList.pastureAnimalWalking:
                case StateList.shepherdFollowing:
                    exitShepherdFollowing();
                    break;
                case StateList.barking:
                    exitBarking();
                    break;


            }

            //Yeni state alınıyor...
            lastState = currentStates.getState();
            switch (lastState)
            {

                case StateList.feedingBait:
                    break;
                case StateList.free:
                    break;
                case StateList.shepherdFollowing:
                    followShepherd();
                    break;
                case StateList.barking:
                    barking();
                    break;

            }

        }
        //dogAnimator.SetInteger("IdleAnim", Random.Range(0,5));
        dogAnimator.SetFloat("DogSpeed", dogAgent.desiredVelocity.magnitude);
    }


    #region Exit State Functions
    private void exitBarking()
    {
        //currentStates.setState(StateList.shepherdFollowing);
    }
    private void exitShepherdFollowing()
    {
        
    }
    private void exitFree()
    {

    }
    private void exitFeedingBait()
    {

    }

    #endregion



    #region State Functions

    public void barking()
    {
        //Burada havlama ses ve animasyon
    }
    public void followShepherd()
    {
        dogAgent.speed = walkingSpeed;
        dogAgent.stoppingDistance = followDistance;
        shepherdFollowCoroutine = StartCoroutine(shepherdFollowRoutine());
    }

    #endregion
    
    
    private Coroutine shepherdFollowCoroutine;
    private Coroutine agentArriveCheckCoroutine;
    #region State Coroutine
    public IEnumerator shepherdFollowRoutine()
    {
        while (true)
        {
            if (currentStates.getState() == StateList.shepherdFollowing)
            {
                try
                {
                    if (WorkerManager.Instance.currentShepherd != null)
                    {
                        Vector3 shepherdPosition = WorkerManager.Instance.currentShepherd.transform.position;

                        //Güvenlik amaçlı yazılmış bir if state
                        if (isOnTheWay == true && !dogAgent.hasPath)
                            isOnTheWay = false;


                        if (Vector3.Distance(shepherdPosition, this.transform.position) > this.followDistance && (isOnTheWay == false))
                        {
                            this.dogAgent.destination = shepherdPosition + (Vector3.forward*followDistance);
                            this.isOnTheWay = true;
                            agentArriveCheckCoroutine = StartCoroutine(agentArriveCheckRoutine());
                            // Yürüme animasyonuna geçildi

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
    }

    public IEnumerator agentArriveCheckRoutine()
    {
        while (isOnTheWay)
        {
            if (dogAgent.hasPath)
            {
                if (!dogAgent.pathPending)
                {
                    if (dogAgent.remainingDistance <= dogAgent.stoppingDistance)
                    {
                        if (!dogAgent.hasPath || dogAgent.velocity.sqrMagnitude == 0f)
                        {
                            this.isOnTheWay = false;

                            Debug.Log(NextGenDebug.HeavyError("Hayvan Idle animasyonuna geçti!"));
                            // Burada Idle animasyonuna geçiş yapılacak.
                        }
                    }
                }
            }
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
    }

    #endregion
}
