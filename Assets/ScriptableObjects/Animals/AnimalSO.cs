using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Animal", menuName = "ScriptableObject/Animal", order = 2)]
public class AnimalSO : ScriptableObject
{
    public string animalName = "Animal Name";

    [Tooltip("Boşluk kullanmadan hayvanın tipini yazın! CamelCase yazın!")]
    public string animalType = "Boşluk kullanmadan hayvanın tipini yazın! CamelCase yazın!";

    public GameObject animalPrefab;
    public float cost;
    //public Icon icon;
}
