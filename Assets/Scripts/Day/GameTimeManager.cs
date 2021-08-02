using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class GameTimeManager : MonoBehaviour
{
    #region Singleton
    private static GameTimeManager _instance;
    public static GameTimeManager Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
        
    }

    #endregion

    Lifetime lifetime;

    private Coroutine moveTimeCoroutine;
    private Coroutine saveTimeCoroutine;

    public bool saveTimeFlag;
    public bool moveTimeFlag;

    public static UnityEvent DayFinishedEvent = new UnityEvent();
    public static UnityEvent HourFinishedEvent = new UnityEvent();
    public static UnityEvent HalfHourFinishedEvent = new UnityEvent();
    public static UnityEvent TenMinuteFinishedEvent = new UnityEvent();
    public static UnityEvent MinuteFinishedEvent = new UnityEvent();

    //Special Time Events
    public static UnityEvent MorningEightEvent = new UnityEvent();
    public static UnityEvent MorningSixEvent = new UnityEvent();
    public static UnityEvent MonthFinished = new UnityEvent();

    //UI Update Event
    public static UnityEvent TimeSpeedChangeEvent = new UnityEvent();

    public readonly int oneDayOneMinute = 1440;
    public readonly int oneDayThreeMinute = 480;
    public readonly int oneDayFiveMinute = 288;
    public readonly int oneDayTenMinute = 144;
    public readonly int oneDayFifteenMinute = 96;
    public readonly int oneDayThirtyMinute = 48;

    public int timeSpeedStep = 0;
    public int[] timeSpeedLevels; 
    
    void Start()
    {

        timeSpeedLevels = new int[] {oneDayFifteenMinute, oneDayTenMinute, oneDayOneMinute };

        lifetime = TableManager.Instance.lifetimeTable; 






        MinuteFinishedEvent.AddListener(customTenMinuteEventCheck);
        MinuteFinishedEvent.AddListener(customHalfHourEventCheck);
        HourFinishedEvent.AddListener(customMorningEightEventCheck);
        HourFinishedEvent.AddListener(customMorningSixEventCheck);

        DayFinishedEvent.AddListener(customMonthEventCheck);
    }

    public void startTime()
    {
        saveTimeFlag = true;
        saveTimeCoroutine = StartCoroutine(saveTimeRoutine());

        moveTimeFlag = true;
        moveTimeCoroutine = StartCoroutine(moveTimeRoutine());
    }

    public void stopTime()
    {
        moveTimeFlag = false;
        StopCoroutine(moveTimeCoroutine);
    }

    public void changeTimeSpeed()
    {
        timeSpeedStep = (timeSpeedStep + 1) % timeSpeedLevels.Length;
        TimeSpeedChangeEvent.Invoke();
    }

    public int getDay()
    {
        return lifetime.day;
    }

    public int getHour()
    {
        return lifetime.hour;
    }

    //Debugging için...
    public void setCheatHour()
    {
        lifetime.hour = 3;
    }

    public int getMinute()
    {
        return lifetime.minute;
    }


    public IEnumerator moveTimeRoutine()
    {
        float second = 0;
        while (moveTimeFlag)
        {
            
            float frameDeltaTime = Time.deltaTime* timeSpeedLevels[timeSpeedStep];
            //float frameDeltaTime = Time.deltaTime* 2880;
            //float frameDeltaTime = Time.deltaTime* oneDayFifteenMinute;
            //float frameDeltaTime = Time.deltaTime* oneDayFiveMinute;
            
            second += frameDeltaTime;
            if (second >= 60)
            {
                second = 0;
                lifetime.minute++;

                MinuteFinishedEvent.Invoke();
            }
            if (lifetime.minute == 60)
            {
                lifetime.minute = 0;
                lifetime.hour++;
                //Debug.Log((lifetime.hour.ToString("D2") + ":" + lifetime.minute.ToString("D2")));
                HourFinishedEvent.Invoke();
            }
            if (lifetime.hour == 24)
            {
                lifetime.hour = 0;
                lifetime.day++;
                DayFinishedEvent.Invoke();
                Debug.Log(NextGenDebug.OneShotUsage("Gün Bitti!!"));
            }
            
           
            yield return new WaitForEndOfFrame();
        }
    }


    private int tenMinuteStack;
    public void customTenMinuteEventCheck()
    {
        tenMinuteStack++;
        if (tenMinuteStack == 10)
        {
            //Debug.Log("10 Dakika");
            TenMinuteFinishedEvent.Invoke();
            tenMinuteStack = 0;
        }
    }

    private int halfHourStack;
    public void customHalfHourEventCheck()
    {
        halfHourStack++;
        if (halfHourStack == 30)
        {
            HalfHourFinishedEvent.Invoke();
            halfHourStack = 0;
        }
    }

    public void customMorningEightEventCheck()
    {
        if (lifetime.hour == 8)
        {
            MorningEightEvent.Invoke();
        }
    }

    public void customMorningSixEventCheck()
    {
        if (lifetime.hour == 6)
        {
            MorningSixEvent.Invoke();
        }
    }

    public void customMonthEventCheck()
    {
        if (lifetime.day != 0 && lifetime.day % 30 == 0)
        {
            MonthFinished.Invoke();
        }
    }


    public IEnumerator saveTimeRoutine()
    {
        while(saveTimeFlag)
        {
            FileOperation.SaveTextAsset(lifetime, lifetime.filePath, Extension.Json);
            
            yield return new WaitForSeconds(60);
        }
    }
}
