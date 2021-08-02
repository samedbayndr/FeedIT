using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class FarmJobType
{
    public const string Cropping = "Cropping";
    public const string Cutting = "Cutting";
    public const string Birth = "Birthing";
    public const string Milking = "Milking";
    
}

[CreateAssetMenu(fileName = "Job", menuName = "ScriptableObject/Job", order = 3)]
public class JobSO : ScriptableObject
{
    [Tooltip("Boşluk kullanmadan Job tipini yazın! CamelCase yazın!")]
    public string jobType = "Job Name";
    public double perProcessTimeCost;
}