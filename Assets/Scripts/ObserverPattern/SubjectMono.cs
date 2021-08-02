using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public abstract class SubjectMono : MonoBehaviour
{
    private List<ObserverMono> _observerMonos = new List<ObserverMono>();

    public void registerObserverMono(ObserverMono observerMono)
    {
        if (observerMono == null)
        {
            Debug.Log(NextGenDebug.HeavyError("Observer can not be null!!!!!"));
            return;
        } 
        _observerMonos.Add(observerMono);
    }
    
    public void Notify([CanBeNull] MonoBehaviour monoObject,Notification notification)
    {
        foreach (var observerMono in _observerMonos)
        {
            observerMono.OnNotify(monoObject, notification);
        }
    }

    public void Notify([CanBeNull] object monoObject, Notification notification)
    {
        foreach (var observerMono in _observerMonos)
        {
            observerMono.OnNotify(monoObject, notification);
        }
    }

    public void deleteObserverMono(ObserverMono observerMono)
    {
        if (observerMono == null)
        {
            Debug.Log(NextGenDebug.HeavyError("Observer can not be null!!!!!"));
            return;
        }
        _observerMonos.Remove(observerMono);
    }
}
