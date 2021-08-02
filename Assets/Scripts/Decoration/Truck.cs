using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    private static Truck _instance;
    public static Truck Instance
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

    public GameObject truckPrefab;
    public GameObject decorationPa;
    private double _income;
    private double _outcome;
    public double Income
    {
        get
        {
            return _income;
        }
        set
        {
            _income = value;
            if (_income >= 2500)
            {
                _income = 0;
                runTruckFromFarm();
            }
        }
    }

    public double Outcome
    {
        get
        {
            return _outcome;
        }
        set
        {
            _outcome = value;
            if (_outcome >= 2500)
            {
                _outcome = 0;
                runTruckFromTown();
            }
        }
    }

    public void runTruckFromTown()
    {
        GameObject decorativeTruck = Instantiate(truckPrefab, decorationPa.transform);
        Animator truckAnimator = decorativeTruck.GetComponent<Animator>();
        truckAnimator.Play("Base Layer.CarGoToFarm", 0);
        truckAnimationRoutine(decorativeTruck, truckAnimator.GetCurrentAnimatorStateInfo(0).length);
        StartCoroutine(truckAnimationRoutine(decorativeTruck, truckAnimator.GetCurrentAnimatorStateInfo(0).length));
    }



    public void runTruckFromFarm()
    {
        GameObject decorativeTruck = Instantiate(truckPrefab, decorationPa.transform);
        Animator truckAnimator = decorativeTruck.GetComponent<Animator>();
        truckAnimator.Play("Base Layer.CarGoToTown", 0);
        StartCoroutine(truckAnimationRoutine(decorativeTruck, truckAnimator.GetCurrentAnimatorStateInfo(0).length));
    }

    public IEnumerator truckAnimationRoutine(GameObject truck, float animationLength)
    {
        yield return new WaitForSeconds(animationLength);
        Destroy(truck);
    }
}
