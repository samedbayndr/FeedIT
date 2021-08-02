using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationManager : MonoBehaviour
{
    #region Singleton Things
    private static ApplicationManager _instance;
    public static ApplicationManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }

        _instance = this;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        GameTimeManager.DayFinishedEvent.AddListener(fullSave);
    }

    public void fullSave()
    {
        List<Animal> allAnimals = new List<Animal>();
        allAnimals.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Chicken));
        allAnimals.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Rooster));
        allAnimals.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Sheep));
        allAnimals.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Ram));
        allAnimals.AddRange(AnimalManager.Instance.getAnimalsWithAnimalType(AnimalType.Lamb));
        foreach (var animal in allAnimals)
        {
            animal.saveAnimalEntity();
        }
        AnimalManager.reCalculateAnimalInventory();
        TableManager.Instance.saveAllTable();
    }

    public void quitGame()
    {
        Application.Quit();
    }


    private void OnApplicationQuit()
    {
        fullSave();
        //TableManager.Instance.saveAllTable();

    }
}
