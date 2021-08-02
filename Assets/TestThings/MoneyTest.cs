using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadPlus))
        {
            Player.Instance.addMoney(1000000);
        }
        if (Input.GetKeyUp(KeyCode.KeypadMinus))
        {

            if (!Player.Instance.decreaseMoney(1000000))
            {
                Player.Instance.decreaseMoney(Player.Instance.getMoney());
            }

           
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            GameTimeManager.Instance.setCheatHour();
        }
    }
}
