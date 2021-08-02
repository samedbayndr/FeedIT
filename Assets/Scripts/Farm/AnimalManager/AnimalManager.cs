using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class AnimalManager : MonoBehaviour
{
    #region Singleton Things

    private static AnimalManager _instance;
    public static AnimalManager Instance
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


    // Sahne içi kontrol hayvanların tutulacağı parent game objeleri
    public GameObject HerdAnimalParent;
    public GameObject SheepParent;
    public GameObject RamParent;
    public GameObject LambParent;
    public GameObject FarmAnimalParent;
    public GameObject ChickenParent;
    public GameObject RoosterParent;
    public GameObject PetParent;
    public GameObject KangalParent;

    //Animal prefabs
    public GameObject chickenPrefab;
    public GameObject roosterPrefab;
    public GameObject sheepPrefab;
    public GameObject ramPrefab;
    public GameObject lambPrefab;
    public GameObject kangalPrefab;



    public UnityEvent DieAnimalEvent = new UnityEvent();
    void Start()
    {
        generateAnimals();
    }


    public void generateAnimals()
    {
        OwnedAnimal ownedAnimal = TableManager.Instance.ownedAnimalTable;

        foreach (var animal in ownedAnimal.AllAnimals)
        {
            switch (animal.animalType)
            {
                case AnimalType.Chicken:
                    GameObject chickenGen = Instantiate(chickenPrefab, ChickenParent.transform);
                    Chicken curChicken = chickenGen.GetComponent<Chicken>();
                    curChicken.animalId = animal.animalId;
                    curChicken.sex = (AnimalSex)animal.sex;
                    curChicken.currentWeight = animal.weight;
                    curChicken.currentHealth = animal.health;
                    curChicken.currentSatiety = animal.satiety;
                    curChicken.currentAttrition = animal.attrition;
                    break;
                case AnimalType.Rooster:
                    GameObject roosterGen = Instantiate(roosterPrefab, RoosterParent.transform);
                    Rooster curRooster = roosterGen.GetComponent<Rooster>();
                    curRooster.animalId = animal.animalId;
                    curRooster.sex = (AnimalSex)animal.sex;
                    curRooster.currentWeight = animal.weight;
                    curRooster.currentHealth = animal.health;
                    curRooster.currentSatiety = animal.satiety;
                    curRooster.currentAttrition = animal.attrition;
                    break;
                case AnimalType.Ram:
                    GameObject ramGen = Instantiate(ramPrefab, RamParent.transform);
                    Ram curRam = ramGen.GetComponent<Ram>();
                    curRam.animalId = animal.animalId;
                    curRam.sex = (AnimalSex)animal.sex;
                    curRam.currentWeight = animal.weight;
                    curRam.currentHealth = animal.health;
                    curRam.currentSatiety = animal.satiety;
                    curRam.currentAttrition = animal.attrition;
                    break;
                case AnimalType.Sheep:
                    GameObject sheepGen = Instantiate(sheepPrefab, SheepParent.transform);
                    Sheep curSheep = sheepGen.GetComponent<Sheep>();
                    curSheep.animalId = animal.animalId;
                    curSheep.sex = (AnimalSex)animal.sex;
                    curSheep.currentWeight = animal.weight;
                    curSheep.currentHealth = animal.health;
                    curSheep.currentSatiety = animal.satiety;
                    curSheep.currentAttrition = animal.attrition;
                    curSheep.currentBreedingRate = animal.breedingRate;
                    break;
                case AnimalType.Lamb:
                    GameObject lambGen = Instantiate(lambPrefab, LambParent.transform);
                    Lamb curLamb = lambGen.GetComponent<Lamb>();
                    curLamb.animalId = animal.animalId;
                    curLamb.sex = (AnimalSex)animal.sex;
                    curLamb.currentWeight = animal.weight;
                    curLamb.currentHealth = animal.health;
                    curLamb.currentSatiety = animal.satiety;
                    curLamb.currentAttrition = animal.attrition;
                    curLamb.currentGrowthRate = animal.growthRate;
                    break;

                case AnimalType.Kangal:
                    GameObject kangalGen = Instantiate(kangalPrefab, KangalParent.transform);
                    Kangal curKangal = kangalGen.GetComponent<Kangal>();
                    curKangal.animalId = animal.animalId;
                    curKangal.sex = (AnimalSex)animal.sex;
                    curKangal.currentWeight = animal.weight;
                    curKangal.currentHealth = animal.health;
                    curKangal.currentSatiety = animal.satiety;
                    curKangal.currentAttrition = animal.attrition;
                    curKangal.currentGrowthRate = animal.growthRate;
                    break;
            }
        }
    }





    //Chickens
    public List<Chicken> getFertilizableChickens()
    {
        List<Chicken> allChickens = ChickenParent.GetComponentsInChildren<Chicken>().ToList();

        List<Chicken> fertilizableChickens = new List<Chicken>();
        for (int i = 0; i < allChickens.Count; i++)
        {
            Chicken curChicken = allChickens[i];
            if (curChicken.isReadyForEggProduction)
            {
                fertilizableChickens.Add(allChickens[i]);
            }
        }
        return fertilizableChickens;
    }





    // Sheeps
    public List<PastureAnimal> getCroppableSheepBarnMembers()
    {
        List<PastureAnimal> allSheeps = SheepParent.GetComponentsInChildren<PastureAnimal>().ToList();

        List<PastureAnimal> croppableSheeps = new List<PastureAnimal>();
        for (int i = 0; i < allSheeps.Count; i++)
        {
            PastureAnimal curSheep = allSheeps[i];
            if (curSheep)
            {
                croppableSheeps.Add(allSheeps[i]);
            }
        }
        return croppableSheeps;
    }
    public List<Sheep> getFertilizableSheeps()
    {
        List<Sheep> allSheeps = SheepParent.GetComponentsInChildren<Sheep>().ToList();

        List<Sheep> fertilizableSheeps = new List<Sheep>();
        for (int i = 0; i < allSheeps.Count; i++)
        {
            Sheep curSheep = allSheeps[i];
            if (curSheep.reProductionStatus == SheepReProductionStatus.ReadyForPregnant)
            {
                fertilizableSheeps.Add(allSheeps[i]);
            }
        }
        return fertilizableSheeps;
    }



    //JOBS
    public bool cutAnimal(string animalId)
    {
        //Uygun işçi var mı?
        if (!WorkerManager.Instance.areThereAvailableWorker())
            return false;

        AnimalEntity animalEntity = getAnimalEntity(animalId);
        Animal animal = getAnimalWithEntity(animalEntity);
        double rewardMeatKilogram;
        switch (animalEntity.animalType)
        {
            case AnimalType.Chicken:
                if (animal.currentWeight < AnimalMaxWeight.Chicken)
                {
                    ErrorMessageUI.Instance.openErrorPanel("You should continue to feed this animal!");
                    return false;
                }
                else
                {
                    rewardMeatKilogram = AnimalMeatEfficiency.calculateAnimalMeatReward(animal.currentWeight, AnimalMeatEfficiency.Chicken);
                    TableManager.Instance.inventoryTable.chickenMeat += rewardMeatKilogram;
                }


                break;
            case AnimalType.Rooster:
                if (animal.currentWeight < AnimalMaxWeight.Rooster)
                {
                    ErrorMessageUI.Instance.openErrorPanel("You should continue to feed this animal!");
                    return false;
                }
                else
                {
                    rewardMeatKilogram = AnimalMeatEfficiency.calculateAnimalMeatReward(animal.currentWeight, AnimalMeatEfficiency.Rooster);
                    TableManager.Instance.inventoryTable.roosterMeat += rewardMeatKilogram;
                }
                break;
            case AnimalType.Ram:
                if (animal.currentWeight < AnimalMaxWeight.Ram)
                {
                    ErrorMessageUI.Instance.openErrorPanel("You should continue to feed this animal!");
                    return false;
                }
                else
                {
                    rewardMeatKilogram = AnimalMeatEfficiency.calculateAnimalMeatReward(animal.currentWeight, AnimalMeatEfficiency.Ram);
                    TableManager.Instance.inventoryTable.ramMeat += rewardMeatKilogram;

                }
                break;
            case AnimalType.Sheep:
                if (animal.currentWeight < AnimalMaxWeight.Sheep)
                {
                    ErrorMessageUI.Instance.openErrorPanel("You should continue to feed this animal!");
                    return false;
                }
                else
                {
                    rewardMeatKilogram = AnimalMeatEfficiency.calculateAnimalMeatReward(animal.currentWeight, AnimalMeatEfficiency.Sheep);
                    TableManager.Instance.inventoryTable.sheepMeat += rewardMeatKilogram;
                }

                break;
        }

        animal.killAnimal();
        FileOperation.SaveTextAsset(TableManager.Instance.inventoryTable,
            TableManager.Instance.inventoryTable.filePath, Extension.Json);

        WorkerManager.Instance.getAvailableWorkerForFarmJobs(WorkerManager.Instance.getJob(FarmJobType.Cutting), 1);
        return true;
    }
    //Doğum.. 
    public bool breeding(string animalId)
    {
        //Uygun işçi var mı?
        if (!WorkerManager.Instance.areThereAvailableWorker())
            return false;

        AnimalEntity animalEntity = getAnimalEntity(animalId);
        Animal animal = getAnimalWithEntity(animalEntity);
        if (animal.animalLocation == AnimalLocation.onBarn && animal.isBreedingRateMaximum())
        {
            GameObject newLaAnimal = Instantiate(AnimalManager.Instance.lambPrefab, AnimalManager.Instance.LambParent.transform);
            Lamb newLamb = newLaAnimal.GetComponent<Lamb>();

            string animalLaId = IdGenerator.generateGUID();
            int randomSex = UnityEngine.Random.Range(0, 2);
            newLamb.animalId = animalLaId;
            newLamb.sex = (AnimalSex)randomSex;

            AnimalEntity newLaEntity = new AnimalEntity(animalLaId, AnimalType.Lamb, randomSex, 0, 0,
                newLamb.currentSatiety, newLamb.maxHealth, newLamb.currentWeight, 0);

            TableManager.Instance.ownedAnimalTable.AllAnimals.Add(newLaEntity);
            TableManager.Instance.inventoryTable.lamb++;
            FileOperation.SaveTextAsset(TableManager.Instance.inventoryTable,
                TableManager.Instance.inventoryTable.filePath, Extension.Json);
            FileOperation.SaveTextAsset(TableManager.Instance.ownedAnimalTable,
                TableManager.Instance.ownedAnimalTable.filePath, Extension.Json);
            WorkerManager.Instance.getAvailableWorkerForFarmJobs(WorkerManager.Instance.getJob(FarmJobType.Birth), 1);

            animal.currentBreedingRate = 0;
            return true;
        }

        return false;
    }

    public bool shearingWool(string animalId)
    {
        //Uygun işçi var mı?
        if (!WorkerManager.Instance.areThereAvailableWorker())
            return false;

        AnimalEntity animalEntity = getAnimalEntity(animalId);
        Animal animal = getAnimalWithEntity(animalEntity);
        SheepGroup sheepGroup = new SheepGroup();
        if (animalEntity.animalType == AnimalType.Sheep || animalEntity.animalType == AnimalType.Ram)
        {
            sheepGroup = animal as SheepGroup;

        }
        else
            return false;

        if (animal.animalLocation == AnimalLocation.onBarn && sheepGroup.isWoolMaximum())
        {
            if (sheepGroup.isWoolMaximum())
            {
                TableManager.Instance.inventoryTable.wool += sheepGroup.shearing();
                FileOperation.SaveTextAsset(TableManager.Instance.inventoryTable,
                    TableManager.Instance.inventoryTable.filePath, Extension.Json);

                WorkerManager.Instance.getAvailableWorkerForFarmJobs(WorkerManager.Instance.getJob(FarmJobType.Cropping), 1);
                return true;
            }

        }

        return false;
    }
    //--JOBS BİTİŞ--//



    public static void reCalculateAnimalInventory()
    {
        int sheepCount = 0;
        int ramCount = 0;
        int lambCount = 0;
        int chickenCount = 0;
        int roosterCount = 0;
        int kangal = 0;

        foreach (var animalEntity in TableManager.Instance.ownedAnimalTable.AllAnimals)
        {
            switch (animalEntity.animalType)
            {
                case AnimalType.Chicken:
                    chickenCount++;
                    break;
                case AnimalType.Rooster:
                    roosterCount++;
                    break;
                case AnimalType.Ram:
                    ramCount++;
                    break;
                case AnimalType.Sheep:
                    sheepCount++;
                    break;
                case AnimalType.Lamb:
                    lambCount++;
                    break;
                case AnimalType.Kangal:
                    kangal++;
                    break;
            }
        }

        TableManager.Instance.inventoryTable.chicken = chickenCount;
        TableManager.Instance.inventoryTable.rooster = roosterCount;
        TableManager.Instance.inventoryTable.sheep = sheepCount;
        TableManager.Instance.inventoryTable.ram = ramCount;
        TableManager.Instance.inventoryTable.lamb = lambCount;
        TableManager.Instance.inventoryTable.dog = kangal;

        FileOperation.SaveTextAsset(TableManager.Instance.inventoryTable,
    TableManager.Instance.inventoryTable.filePath, Extension.Json);


    }

    public AnimalEntity getAnimalEntity(string animalId)
    {
        AnimalEntity animalEntity = TableManager.Instance.ownedAnimalTable.AllAnimals.Find(a => a.animalId == animalId);
        return animalEntity;
    }

    public Animal getAnimalWithEntity(AnimalEntity entity)
    {
        Animal curAnimal = new Animal();
        switch (entity.animalType)
        {
            case AnimalType.Chicken:
                curAnimal = ChickenParent.GetComponentsInChildren<Chicken>().ToList()
                    .Find(a => a.animalId == entity.animalId);

                break;
            case AnimalType.Rooster:
                curAnimal = RoosterParent.GetComponentsInChildren<Rooster>().ToList()
                    .Find(a => a.animalId == entity.animalId);
                break;
            case AnimalType.Ram:
                curAnimal = RamParent.GetComponentsInChildren<Ram>().ToList()
                    .Find(a => a.animalId == entity.animalId);
                break;
            case AnimalType.Sheep:
                curAnimal = SheepParent.GetComponentsInChildren<Sheep>().ToList()
                    .Find(a => a.animalId == entity.animalId);

                break;
            case AnimalType.Lamb:
                curAnimal = LambParent.GetComponentsInChildren<Lamb>().ToList()
                    .Find(a => a.animalId == entity.animalId);
                break;
            case AnimalType.Kangal:
                curAnimal = KangalParent.GetComponentsInChildren<Kangal>().ToList()
                    .Find(a => a.animalId == entity.animalId);
                break;
            default:
                curAnimal = null;
                break;
        }
        return curAnimal;
    }

    public List<Animal> getAnimalsWithAnimalType(string animalType)
    {
        List<Animal> relatedAnimals = new List<Animal>();
        for (int i = 0; i < TableManager.Instance.ownedAnimalTable.AllAnimals.Count; i++)
        {
            if (TableManager.Instance.ownedAnimalTable.AllAnimals[i].animalType == animalType)
            {
                relatedAnimals.Add(getAnimalWithEntity(TableManager.Instance.ownedAnimalTable.AllAnimals[i]));
            }
        }

        return relatedAnimals;
    }

    public Animal getAnimal(string animalId)
    {
        AnimalEntity animalEntity = TableManager.Instance.ownedAnimalTable.AllAnimals.Find(a => a.animalId == animalId);
        if (animalEntity == null)
            return null;

        Animal curAnimal = new Animal();
        switch (animalEntity.animalType)
        {
            case AnimalType.Chicken:
                curAnimal = ChickenParent.GetComponentsInChildren<FarmAnimal>().ToList()
                    .Find(a => a.animalId == animalEntity.animalId);

                break;
            case AnimalType.Rooster:
                curAnimal = RoosterParent.GetComponentsInChildren<FarmAnimal>().ToList()
                    .Find(a => a.animalId == animalEntity.animalId);
                break;
            case AnimalType.Ram:
                curAnimal = RamParent.GetComponentsInChildren<PastureAnimal>().ToList()
                    .Find(a => a.animalId == animalEntity.animalId);
                break;
            case AnimalType.Sheep:
                curAnimal = SheepParent.GetComponentsInChildren<PastureAnimal>().ToList()
                    .Find(a => a.animalId == animalEntity.animalId);

                break;
            case AnimalType.Lamb:
                curAnimal = LambParent.GetComponentsInChildren<PastureAnimal>().ToList()
                    .Find(a => a.animalId == animalEntity.animalId);
                break;
            case AnimalType.Kangal:
                curAnimal = KangalParent.GetComponentsInChildren<Kangal>().ToList()
                    .Find(a => a.animalId == animalEntity.animalId);
                break;

        }

        return curAnimal;
    }




}
