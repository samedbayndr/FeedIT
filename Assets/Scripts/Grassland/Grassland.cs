using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrasslandGuestEvent : UnityEvent<Grassland>{

}

public class Grassland : MonoBehaviour
{
    public double yield;
    public double regenerationRate;
    private double amountOfRegeneration;
    public string grasslandName;

    //Çoban mera alanına girdi eventi
    public static GrasslandGuestEvent GrasslandGuestEvent = new GrasslandGuestEvent();
    //Çoban mera alanından ayrıldı eventi
    public static  UnityEvent GrasslandGuestLeaveEvent = new UnityEvent();
    public void Start()
    {
        calculateRegeneration();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Shepherd")
        {
            GrasslandGuestEvent.Invoke(this);
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Shepherd")
        {
            GrasslandGuestLeaveEvent.Invoke();
        }
       
    }

    public double getRegeneration()
    {
        if (amountOfRegeneration == 0.0d)
        {
            if (calculateRegeneration() == 0.0d)
            {
                Debug.Log(NextGenDebug.NormalError("Problem! amount of regeneration is still zero!!!"));
            }
            
        }

        return amountOfRegeneration;
    }

    public double calculateRegeneration()
    {
        this.amountOfRegeneration = (regenerationRate * yield) / 100;
        return this.amountOfRegeneration;
    }


}
