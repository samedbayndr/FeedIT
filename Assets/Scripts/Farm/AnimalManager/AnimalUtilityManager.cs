using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimalUtilityManager : MonoBehaviour
{
    #region Singleton Things

    private static AnimalUtilityManager _instance;
    public static AnimalUtilityManager Instance
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


    #endregion
    

    void Start()
    {
    }

    public void buyAnimal(AnimalSO animalSO)
    {
        if (animalSO != null)
        {
            if (Player.Instance.getMoney() >= animalSO.cost)
            {

                if (animalSO.animalType == AnimalType.Chicken || animalSO.animalType == AnimalType.Rooster)
                {
                    if (!ChickenCoopManager.Instance.areThereCapacity())
                    {
                        MessageSentenceThrower.Instance.fireMessage("No place on the coop");
                        UnityEngine.Debug.Log(NextGenDebug.HeavyError("Kümes Yok!"));
                        return;
                    }
                }
                else if (animalSO.animalType == AnimalType.Sheep || animalSO.animalType == AnimalType.Ram || animalSO.animalType == AnimalType.Lamb)
                {
                    if (!SheepBarnManager.Instance.areThereCapacity())
                    {
                        MessageSentenceThrower.Instance.fireMessage("No place on the barn");
                        UnityEngine.Debug.Log(NextGenDebug.HeavyError("Ahır Yok!"));
                        return;
                    }
                }
                else if (animalSO.animalType == AnimalType.Kangal)
                {
                    if (!DogHouseManager.Instance.areThereCapacity())
                    {
                        MessageSentenceThrower.Instance.fireMessage("No place on the dog house");
                        UnityEngine.Debug.Log(NextGenDebug.HeavyError("Kulübe Yok!"));
                        return;
                    }
                }


                //TODO: Burada parayı kullanıcıdan düş ve güncelle
                Player.Instance.decreaseMoney(animalSO.cost);    



                // sex;
                // 0: male
                // 1: female

                switch (animalSO.animalType)
                {
                    case AnimalType.Chicken:

                        GameObject newCAnimal = Instantiate(animalSO.animalPrefab, AnimalManager.Instance.ChickenParent.transform);
                        Chicken newChicken = newCAnimal.GetComponent<Chicken>();

                        string animalCId = IdGenerator.generateGUID();
                        newChicken.animalId = animalCId;
                        AnimalEntity newCEntity = new AnimalEntity(animalCId, AnimalType.Chicken,1,0,0,
                            newChicken.currentSatiety, newChicken.maxHealth, newChicken.currentWeight,0);

                        TableManager.Instance.ownedAnimalTable.AllAnimals.Add(newCEntity);
                        TableManager.Instance.inventoryTable.chicken++;


                        break;
                    case AnimalType.Rooster:
                        GameObject newRoAnimal = Instantiate(animalSO.animalPrefab, AnimalManager.Instance.RoosterParent.transform);
                        Rooster newRooster = newRoAnimal.GetComponent<Rooster>();

                        string animalRoId = IdGenerator.generateGUID();
                        newRooster.animalId = animalRoId;
                        AnimalEntity newRoEntity = new AnimalEntity(animalRoId, AnimalType.Rooster, 0, 0, 0,
                            newRooster.currentSatiety, newRooster.maxHealth, newRooster.currentWeight,0);

                        TableManager.Instance.ownedAnimalTable.AllAnimals.Add(newRoEntity);
                        TableManager.Instance.inventoryTable.rooster++;

                        break;
                    case AnimalType.Ram:
                        GameObject newRaAnimal = Instantiate(animalSO.animalPrefab, AnimalManager.Instance.RamParent.transform);
                        Ram newRam = newRaAnimal.GetComponent<Ram>();

                        string animalRaId = IdGenerator.generateGUID();
                        newRam.animalId = animalRaId;
                        AnimalEntity newRaEntity = new AnimalEntity(animalRaId, AnimalType.Ram,0, 0, 100,
                            newRam.currentSatiety, newRam.maxHealth, newRam.currentWeight, 0);

                        TableManager.Instance.ownedAnimalTable.AllAnimals.Add(newRaEntity);
                        TableManager.Instance.inventoryTable.ram++;
                        break;
                    case AnimalType.Sheep:
                        GameObject newShAnimal = Instantiate(animalSO.animalPrefab, AnimalManager.Instance.SheepParent.transform);
                        Sheep newSheep = newShAnimal.GetComponent<Sheep>();

                        string animalShId = IdGenerator.generateGUID();
                        newSheep.animalId = animalShId;
                        AnimalEntity newShEntity = new AnimalEntity(animalShId, AnimalType.Sheep, 1,0, 100,
                            newSheep.currentSatiety, newSheep.maxHealth, newSheep.currentWeight, 0);

                        TableManager.Instance.ownedAnimalTable.AllAnimals.Add(newShEntity);
                        TableManager.Instance.inventoryTable.sheep++;
                        break;
                    case AnimalType.Lamb:
                        GameObject newLaAnimal = Instantiate(animalSO.animalPrefab, AnimalManager.Instance.LambParent.transform);
                        Lamb newLamb = newLaAnimal.GetComponent<Lamb>();

                        string animalLaId = IdGenerator.generateGUID();
                        newLamb.animalId = animalLaId;
                        AnimalEntity newLaEntity = new AnimalEntity(animalLaId, AnimalType.Lamb,0, 0, 0,
                            newLamb.currentSatiety, newLamb.maxHealth, newLamb.currentWeight, 0);

                        TableManager.Instance.ownedAnimalTable.AllAnimals.Add(newLaEntity);
                        TableManager.Instance.inventoryTable.lamb++;
                        break;
                    case AnimalType.Kangal:
                        GameObject newKaAnimal = Instantiate(animalSO.animalPrefab, AnimalManager.Instance.KangalParent.transform);
                        Kangal newKangal = newKaAnimal.GetComponent<Kangal>();

                        string animalKaId = IdGenerator.generateGUID();
                        newKangal.animalId = animalKaId;
                        AnimalEntity newKaEntity = new AnimalEntity(animalKaId, AnimalType.Kangal, Random.Range(0,2), 0, 0,
                            newKangal.currentSatiety, newKangal.maxHealth, newKangal.currentWeight, 0);

                        TableManager.Instance.ownedAnimalTable.AllAnimals.Add(newKaEntity);
                        TableManager.Instance.inventoryTable.dog++;
                        break;

                }

                AnimalManager.reCalculateAnimalInventory();



            }
            else
            {
                MessageSentenceThrower.Instance.fireMessage("You have not enough money!");
            }
        }
    }

}
