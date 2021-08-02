using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShepherdTestScript : MonoBehaviour
{
    private NavMeshAgent shepherdAgent;
    private UnityEvent moveHerdEvent = new UnityEvent();

    public GameObject ShepherdUIParentGO;
    public Button playInstrumentButton;
    public bool shepherdForceFollow;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        ShepherdUIParentGO = GameObject.Find("ShepherdUI");
        shepherdAgent = GetComponent<NavMeshAgent>();
        setInitPosition();
        this.gameObject.tag = "Shepherd";

        //herdAnimalList = GrazingMode.Instance.getHerdAvailableAnimal();


        //Go Grazing butonu deaktif oluyor..
        //GameObject.Find("GoGrazingBtn").SetActive(false);

        //Shepherd Play Instrument butonu aktif oluyor
        //GameObject playInstrumentGO = ShepherdUIParentGO.transform.GetComponentsInChildren<Transform>(true).ToList().Find(a => a.name == "PlayInstrumentBtn").gameObject;
        //playInstrumentGO.SetActive(true);
        //playInstrumentButton = playInstrumentGO.GetComponent<Button>();
        //playInstrumentButton.onClick.AddListener(callHerdAnimals);



    }

    public void setInitPosition()
    {
        shepherdAgent.enabled = false;
        this.transform.position = GrazingMode.Instance.ShepherdSpawnPos.transform.position;
        shepherdAgent.enabled = true;
    }

    //public float whistleHeardDistance = 20f;
    //public void whistle()
    //{
    //    for (var i = 0; i < GrazingMode.Instance.herdAnimalsOnPasture.Count; i++)
    //    {
    //        PastureAnimal pastureAnimal = GrazingMode.Instance.herdAnimalsOnPasture[i];
    //        if (pastureAnimal.currentStates.getState() != StateList.shepherdFollowing && Probability.Roll(0, 100) <= (baseFollowProbability * GrazingMode.Instance.forceFollowModifier))
    //        {
    //            AnimalEmotionUI.Instance.fireUnwillingEmotion(pastureAnimal.transform.position);
    //            continue;
    //        }

            
    //        if (Vector3.Distance(transform.position, pastureAnimal.transform.position) <= whistleHeardDistance)
    //        {
    //            pastureAnimal.currentStates.setState(StateList.shepherdFollowing);
    //        }
    //    }
    //}


    //public void forcePastureAnimalForShepherdFollow()
    //{
    //    GeneralGameUIManager.Instance.shepherdForcedFollowIcon.SetActive(true);
    //    for (var i = 0; i < GrazingMode.Instance.herdAnimalsOnPasture.Count; i++)
    //    {
    //        PastureAnimal pastureAnimal = GrazingMode.Instance.herdAnimalsOnPasture[i];
    //        if (Vector3.Distance(transform.position, pastureAnimal.transform.position) <= whistleHeardDistance * 3)
    //        {
    //            pastureAnimal.isForcedFollow = true;
    //        }
    //    }
    //}

    //public void unforcePastureAnimalForShepherdFollow()
    //{
    //    if (shepherdForceFollow != true)
    //        return;
    //    GeneralGameUIManager.Instance.shepherdForcedFollowIcon.SetActive(false);
    //    for (var i = 0; i < GrazingMode.Instance.herdAnimalsOnPasture.Count; i++)
    //    {
    //        PastureAnimal pastureAnimal = GrazingMode.Instance.herdAnimalsOnPasture[i];
    //        pastureAnimal.isForcedFollow = false;

    //    }
    //}


    public void callHerdAnimals()
    {
        foreach (PastureAnimal pastureAnimal in GrazingMode.Instance.herdAnimalsOnPasture)
        {
            pastureAnimal.currentStates.setState(StateList.shepherdFollowing);
        }

        //playInstrumentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Stop Instrument");
        playInstrumentButton.onClick.RemoveAllListeners();
        playInstrumentButton.onClick.AddListener(freeHerdAnimals);
    }

    public void freeHerdAnimals()
    {
        foreach (var pastureAnimal in GrazingMode.Instance.herdAnimalsOnPasture)
        {
            if (pastureAnimal.collidedGOs.Find(a=>a.name.Contains("Grassland")))
            {
                pastureAnimal.currentStates.setState(StateList.feedSearching);
            }
            else
            {
                pastureAnimal.currentStates.setState(StateList.pastureAnimalIdle);
            }
        }


        playInstrumentButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Play Instrument");
        playInstrumentButton.onClick.RemoveAllListeners();
        playInstrumentButton.onClick.AddListener(callHerdAnimals);
    }

    
    //private float passedTime;
    //private bool canLongPressActive;
    //private float neededLongPressTime = 3f;
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        passedTime = Time.time;
    //        canLongPressActive = true;
    //    }

    //    if (shepherdForceFollow == false && canLongPressActive == true && Time.time - passedTime > neededLongPressTime)
    //    {
    //        shepherdForceFollow = true;
    //        canLongPressActive = false;
    //        forcePastureAnimalForShepherdFollow();
    //    }

    //    if (Input.GetKeyUp(KeyCode.C))
    //    {
    //        if (shepherdForceFollow == true && Time.time - passedTime < neededLongPressTime)
    //        {
    //            unforcePastureAnimalForShepherdFollow();
    //            canLongPressActive = false;
    //            shepherdForceFollow = false;
    //            //passedTime = 0f;
    //            whistle();
    //        }
    //        else
    //        {
    //            canLongPressActive = false;
    //            whistle();
    //        }
                   
    //    }
    //}

    //void OnDestroy()
    //{
    //    unforcePastureAnimalForShepherdFollow();
    //}
}
