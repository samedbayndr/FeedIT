using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<Button> tabButtons = new List<Button>();
    public List<GameObject> tabWindows = new List<GameObject>();
    public GameObject firstOpenedTabWindow;

    private GameObject selectedTabWindow;
    public void resetTab()
    {
        tabWindows.ForEach(delegate (GameObject tabGO)
        {
            tabGO.SetActive(false);
        });
    }

    public void OnEnable()
    {
        OnOpenPanel();
    }
    public void OnOpenPanel()
    {
        resetTab();
        selectedTabWindow = firstOpenedTabWindow;
        firstOpenedTabWindow.SetActive(true);
    }
    public void OnSelectTab(GameObject selectedWindow)
    {
        resetTab();
        selectedTabWindow = selectedWindow;
        selectedWindow.SetActive(true);
    }
}
