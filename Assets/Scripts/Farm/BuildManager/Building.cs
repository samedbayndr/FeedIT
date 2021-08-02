using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour
{
    private static Building _instance;

    public static Building Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
    }


    public void buyBuild(BuildingSO buildingSo)
    {
        if (buildingSo != null)
        {
            //Para kontrolü yap!
            if (Player.Instance.getMoney() >= buildingSo.cost)
            {
                FarmUI.Instance.closeMenu(BuildUI.Instance.BuildBuyPanel);
                BuildingPlacementHelper.Instance.spawnBuild(buildingSo);
                BuildUI.Instance.buyBuildMenuBtn.interactable = false;
            }
            else
            {
                MessageSentenceThrower.Instance.fireMessage("You have not enough money!");
                //ErrorMessageUI.Instance.openErrorPanel("You have not enough money! You need to " +
                //                                       Math.Abs(Player.Instance.Economy.money - buildingSo.cost) + " dollar!");
                Debug.Log(NextGenDebug.NormalError("Money is money"));
            }
        }
    }


}

