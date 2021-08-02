using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LogPanelUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Singleton Things
    private static LogPanelUI _instance;
    public static LogPanelUI Instance
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

    public bool isMouseOver;
    public GameObject LogPanel;
    public Image LogPanelBackground;
    public GameObject LogPanelContent;
    public GameObject LogTextPrefab;
    public CanvasGroup LogPanelCanvasGroup;

    private RectTransform LogPanelContentRectTransform;
    private GridLayoutGroup LogPanelContentGridLayoutGroup;

    public Coroutine PanelVisibilityCoroutine;
    private float panelMaxVisibility = 0.75f;
    private float panelMinVisibility = 0.15f;
    public void Start()
    {
        LogPanelContentRectTransform = LogPanelContent.GetComponent<RectTransform>();
        LogPanelContentGridLayoutGroup = LogPanelContent.GetComponent<GridLayoutGroup>();

        PanelVisibilityCoroutine = StartCoroutine(PanelVisibilityRoutine(LogPanelCanvasGroup.alpha, panelMinVisibility, 1f));
    }
    public void Update()
    {
        if (isMouseOver)
        {
            if (Input.GetMouseButtonUp(0))
            {
                //if (EventSystem.current.IsPointerOverGameObject())
                //    return;
                if (PanelVisibilityCoroutine != null )
                    StopCoroutine(PanelVisibilityCoroutine);
                
                PanelVisibilityCoroutine = StartCoroutine(PanelVisibilityRoutine(LogPanelCanvasGroup.alpha, panelMaxVisibility, 0.1f));
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        if (PanelVisibilityCoroutine != null)
            StopCoroutine(PanelVisibilityCoroutine);

        PanelVisibilityCoroutine = StartCoroutine(PanelVisibilityRoutine(LogPanelCanvasGroup.alpha, panelMinVisibility, 2.5f));

    }


    public bool addLog(string message)
    {
        if (message != String.Empty )
        {

            TextMeshProUGUI newLogText = Instantiate(LogTextPrefab, LogPanelContent.transform).GetComponent<TextMeshProUGUI>();
            LogPanelContentRectTransform.sizeDelta = new Vector2(LogPanelContentRectTransform.sizeDelta.x, (LogPanelContentRectTransform.sizeDelta.y + LogPanelContentGridLayoutGroup.cellSize.y));
            newLogText.transform.SetAsFirstSibling();
            newLogText.SetText(message);
            return true;
        }

        return false;
    }
    
    public IEnumerator PanelVisibilityRoutine(float startValue, float lastValue, float animTime)
    {
        float t = 0f;
        while (t <= animTime)
        {
            
            t += 0.25f * Time.deltaTime;
            LogPanelCanvasGroup.alpha = Mathf.Lerp(startValue, lastValue, t/animTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
