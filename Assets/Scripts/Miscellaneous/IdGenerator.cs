using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdGenerator : MonoBehaviour
{
    public static string generateGUID()
    {
        string newId = "";
        newId = Guid.NewGuid().ToString().ToLower();
        return newId;
    }
}
