using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Farm : MonoBehaviour
{
    private static Farm _instance;

    public static Farm Instance
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    public Transform initSpawn;
    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Worker"))
        {
            GrazingMode.Instance.updateHerdAnimalsOnPastureList();
            GrazingMode.Instance.updateDogsOnPastureList();

            ShepherdTestScript shepherd = collider.gameObject.GetComponent<ShepherdTestScript>();
            Debug.Log(GrazingMode.Instance.herdAnimalsOnPasture.Count);
            for (var i = 0; i < GrazingMode.Instance.herdAnimalsOnPasture.Count; i++)
            {
                var pastureAnimal = GrazingMode.Instance.herdAnimalsOnPasture[i];
                if (Vector3.Distance(shepherd.transform.position, pastureAnimal.transform.position) <= GrazingMode.Instance.shepherdVisiblityDistance)
                {
                    pastureAnimal.animalLocation = AnimalLocation.onBarn;
                    pastureAnimal.goToRest(initSpawn.position);
                    GrazingMode.Instance.herdAnimalsOnPasture.Remove(pastureAnimal);
                    i--;
                }
            }

            for (var i = 0; i < GrazingMode.Instance.dogsOnPasture.Count; i++)
            {
                var dog = GrazingMode.Instance.dogsOnPasture[i];
                if (Vector3.Distance(shepherd.transform.position, dog.transform.position) <= GrazingMode.Instance.shepherdVisiblityDistance)
                {
                    dog.animalLocation = AnimalLocation.onDogHouse;
                    dog.goToRest(initSpawn.position);
                    GrazingMode.Instance.dogsOnPasture.Remove(dog);
                    i--;
                }
            }
        }    
        
    }
    

}
