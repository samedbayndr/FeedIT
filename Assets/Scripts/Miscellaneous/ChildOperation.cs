using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildOperation : MonoBehaviour
{
    public static List<GameObject> GetFirstLevelChildren(GameObject parentGO)
    {
        List<GameObject> childGameObjects = new List<GameObject>();
        int childCount = parentGO.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parentGO.transform.GetChild(i);
            if (child.transform.parent.gameObject.GetInstanceID() == parentGO.GetInstanceID())
            {
                childGameObjects.Add(child.gameObject);
            }
        }

        return childGameObjects;
    }
}
