using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GeneralGameUIManager : MonoBehaviour
{
    private static GeneralGameUIManager _instance;
    public static GeneralGameUIManager Instance
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

    public TextMeshProUGUI money;
    public TextMeshProUGUI date;
    public GameObject timeSpeedBtn;
    public GameObject shepherdForcedFollowIcon;
    public GameObject moneyNotificationPA;
    public GameObject moneyNotificationPrefab;

    public List<Sprite> timeSpeedSprites;
    void Start()
    {
       GameTimeManager.MinuteFinishedEvent.AddListener(updateDateUI);
       GameTimeManager.TimeSpeedChangeEvent.AddListener(updateTimeSpeedUI);
    }

    ///ÖNEMLİ UYARI! Bu fonksiyon Player classının Start fonksiyonunda Execution Order problemi yaşamamak için listener olarak moneyNotification Unity Eventine bağlanmıştır!!!!!!!
    ///ÖNEMLİ UYARI! Bu fonksiyon Player classının Start fonksiyonunda Execution Order problemi yaşamamak için listener olarak moneyNotification Unity Eventine bağlanmıştır!!!!!!!
    public void updateMoneyUI()
    {
        money.SetText(Player.Instance.getMoney().ToString("F1"));
    }

    public void updateDateUI()
    {
        date.SetText(("Day: "+GameTimeManager.Instance.getDay().ToString("D2")+ "      " +GameTimeManager.Instance.getHour().ToString("D2") + ":" + GameTimeManager.Instance.getMinute().ToString("D2")));
    }

    public void updateTimeSpeedUI()
    {
        timeSpeedBtn.GetComponent<UnityEngine.UI.Image>().sprite = timeSpeedSprites[GameTimeManager.Instance.timeSpeedStep];
    }

    public void fireMoneyNotification(double pMoney)
    {
        GameObject newMoneyNotification = Instantiate(moneyNotificationPrefab, moneyNotificationPA.transform);
        TextMeshProUGUI moneyNotificationText = newMoneyNotification.GetComponent<TextMeshProUGUI>();

        if (pMoney >= 0)
        {
            moneyNotificationText.SetText("+" + pMoney);
            moneyNotificationText.color = new Color(0, 255, 0);
        }
        else if (pMoney < 0)
        {
            moneyNotificationText.SetText(pMoney.ToString());
            moneyNotificationText.color = new Color(255, 0, 0);
        }

        StartCoroutine(closeMoneyNotificationRoutine(newMoneyNotification));
    }

    public IEnumerator closeMoneyNotificationRoutine(GameObject newMoneyNotification)
    {
        newMoneyNotification.SetActive(true);
        Animator moneyNotificationAnimator = newMoneyNotification.GetComponent<Animator>();
        moneyNotificationAnimator.Play("Base Layer.MoneyNotification", 0);
        yield return new WaitForSeconds(moneyNotificationAnimator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(newMoneyNotification);
    }
}
