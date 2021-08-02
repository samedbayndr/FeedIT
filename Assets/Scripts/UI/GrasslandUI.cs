using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GrasslandUI : MonoBehaviour
{


    #region Singleton Things
    private static GrasslandUI _instance;
    public static GrasslandUI Instance
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

    private Grassland currentGrassland;
    private List<GrasslandGrid> allGrids = new List<GrasslandGrid>();
    private List<Tussock> allTussocks = new List<Tussock>();
    private double grasslandBenefitabilityPercentage = 0.0;
    
    //UI Elements
    public GameObject grasslandUIGo;
    public TextMeshProUGUI grasslandWelcomePanelText;

    public GameObject grasslandInfoPanel;
    public TextMeshProUGUI grasslandInfoGrasslandNameText;
    public TextMeshProUGUI grasslandInfoYieldText;
    public TextMeshProUGUI grasslandInfoBenefitabilityPercentageText;
    public Animator welcomePanelAnimator;


    //Events
    public UnityEvent updateGrasslandInfoEvent = new UnityEvent();

    public void Start()
    {
        //welcomePanelAnimator = grasslandWelcomePanelText.GetComponent<Animator>();

        Tussock.UpdateGrasslandEvent.AddListener(calculateGrasslandBenefitability);
        Grassland.GrasslandGuestEvent.AddListener(helloVisitor);
        Grassland.GrasslandGuestLeaveEvent.AddListener(goodByeVisitor);


    }

    public void helloVisitor(Grassland visitedGrassland)
    {
        currentGrassland = visitedGrassland;
        fetchTussocksData();
        enterGrasslandUI();
        //enterGrasslandUIdan sonra calculate çalışacak... !!ÖNEMLİ!!
        calculateGrasslandBenefitability();
    }
    public void enterGrasslandUI()
    {
        grasslandUIGo.SetActive(true);
        grasslandInfoPanel.SetActive(true);

        grasslandInfoGrasslandNameText.SetText(currentGrassland.grasslandName);
        grasslandInfoYieldText.SetText("Yield %" +currentGrassland.yield);

        fireWelcomeGrasslandText(currentGrassland.grasslandName);
        prepareGrasslandInfoPanel();
    }


    public void fireWelcomeGrasslandText(string grasslandName)
    {
        if (!grasslandWelcomePanelText.gameObject.activeSelf)
            grasslandWelcomePanelText.gameObject.SetActive(true);

        grasslandWelcomePanelText.SetText("Welcome " + grasslandName);
        welcomePanelAnimator?.Play("Base Layer.WelcomeGrassland");
        StartCoroutine(welcomeAnimationRoutine());

    }

    public IEnumerator welcomeAnimationRoutine()
    {
        yield return new WaitForSeconds(welcomePanelAnimator.GetCurrentAnimatorStateInfo(0).length);
        welcomePanelAnimator.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void prepareGrasslandInfoPanel()
    {
        updateGrasslandInfoEvent.AddListener(updateGrasslandInfoPanel);
    }

    public void updateGrasslandInfoPanel()
    {
        grasslandInfoBenefitabilityPercentageText.SetText("Mera Verimi: %"+ grasslandBenefitabilityPercentage.ToString("F1"));
    }



    public void goodByeVisitor()
    {
        exitGrassland();
        currentGrassland = null;
        allGrids.Clear();
        allTussocks.Clear();
        grasslandBenefitabilityPercentage = 0.0;

    }


    public void exitGrassland()
    {
        grasslandUIGo.SetActive(false);
        grasslandInfoPanel.gameObject.SetActive(false);

        updateGrasslandInfoEvent.RemoveListener(updateGrasslandInfoPanel);
    }

    public void calculateGrasslandBenefitability()
    {

        if (currentGrassland != null)
        {
            double grasslandMaxHealth = 0;
            double grasslandCurrentHealth = 0;
            for (int i = 0; i < allTussocks.Count; i++)
            {
                grasslandCurrentHealth += allTussocks[i].getHealth();
                grasslandMaxHealth += allTussocks[i].maxHealth;
            }

            grasslandBenefitabilityPercentage = (grasslandCurrentHealth * 100) / grasslandMaxHealth;
            updateGrasslandInfoEvent.Invoke();
        }
        else
        {
            //Debug.Log(NextGenDebug.HeavyError("!!!! Mera alanında değilsin !!!!"));
        }
    }

    public void fetchTussocksData()
    {
        if (currentGrassland != null)
        {
            for (int i = 0; i < currentGrassland.transform.childCount; i++)
            {
                allGrids.Add(currentGrassland.transform.GetChild(i).GetComponent<GrasslandGrid>());
            }

            if (allGrids.Count != 0)
            {
                for (int i = 0; i < allGrids.Count; i++)
                {
                    for (int j = 0; j < allGrids[i].tussockList.Count; j++)
                    {
                        allTussocks.Add(allGrids[i].tussockList[j]);
                    }
                }
            }
            else
                Debug.Log(NextGenDebug.HeavyError("!!!! Bu gridler nereye kayboldu !!!!"));
        }
        else
        {
            Debug.Log(NextGenDebug.HeavyError("!!!! Mera alanında değilsin !!!!"));
        }
    }
}
