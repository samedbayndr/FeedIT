using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForSale : MonoBehaviour
{
    public GameObject OfferPanel;
    public TextMeshProUGUI OfferText;
    public Button acceptButton;
    public Button declineButton;
    public double price;
    public string areaName;


    public void openOfferPanel()
    {
        prepareOffer();
        acceptButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();
        acceptButton.onClick.AddListener(this.accept);
        declineButton.onClick.AddListener(this.decline);
        OfferPanel.SetActive(true);
    }

    public void closeOfferPanel()
    {
        OfferPanel.SetActive(false);

    }

    public void prepareOffer()
    {
        OfferText.SetText("Do you want to buy this huge piece of land for only "+price.ToString("F1")+" dollars?");
    }
    public void accept()
    {

        if (EstateManager.Instance.buyEstate(areaName, price))
            closeOfferPanel();
        else
            ErrorMessageUI.Instance.openErrorPanel("You have not enough money! You need to " +
                                                   Math.Abs(Player.Instance.getMoney() - price).ToString("F1") + " dollar!");


    }

    public void decline()
    {
        closeOfferPanel();
    }
}
