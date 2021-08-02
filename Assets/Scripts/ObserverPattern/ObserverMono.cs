using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public abstract class ObserverMono : MonoBehaviour
{
    public void OnNotify([CanBeNull] Object monoObject, Notification notification)
    {

    }

    public void OnNotify([CanBeNull] object monoObject, Notification notification)
    {

    }
}
