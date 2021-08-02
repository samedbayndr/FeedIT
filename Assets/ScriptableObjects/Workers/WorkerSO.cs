using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Worker", menuName = "ScriptableObject/Worker", order = 4)]
public class WorkerSO : ScriptableObject
{
    public string workerName = "Worker Name";

    [Tooltip("Boşluk kullanmadan worker tipini yazın! CamelCase yazın!")]
    public string workerType = "Boşluk kullanmadan worker tipini yazın! CamelCase yazın!";
    public double efficiencyModifier = 1;
    public GameObject workerPrefab;
    public double cost;
    public double salary;

}
