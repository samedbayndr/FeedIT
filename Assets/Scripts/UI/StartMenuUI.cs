using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartMenuUI : MonoBehaviour
{
    #region Singleton Things
    private static StartMenuUI _instance;
    public static StartMenuUI Instance
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
        //Time.timeScale = 0;

    }

    public UnityEvent PlayGameEvent = new UnityEvent();

    public GameObject GeneralGameUI;
    public GameObject FarmUI;
    public GameObject GeneralFarmUI;
    
    public GameObject StartMenuUIParentGO;
    public CanvasGroup StartMenuCanvasGroup;
    public void playGame()
    {


        StartCoroutine(TransparentPassing());
        PlayGameEvent.Invoke();
    }

    IEnumerator TransparentPassing()
    {
        float t = 0.0f;

        GameTimeManager.Instance.startTime();
        GeneralGameUI.SetActive(true);
        FarmUI.SetActive(true);
        GeneralFarmUI.SetActive(true);

        while (StartMenuCanvasGroup.alpha > 0)
        {
            StartMenuCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
            t += (0.75f * Time.deltaTime);
            //Debug.Log(StartMenuCanvasGroup.alpha);
            yield return new WaitForEndOfFrame();
        }
        StartMenuUIParentGO.SetActive(false);
    }
}
