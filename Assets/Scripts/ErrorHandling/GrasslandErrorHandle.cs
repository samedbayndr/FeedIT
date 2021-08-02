using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrasslandErrorHandle : MonoBehaviour
{
    //Grassland
    public static bool IsGridMemberOfGrassland(GameObject grasslandGO, GameObject gridGO)
    {
        if (gridGO.transform.parent.gameObject.GetInstanceID() == grasslandGO.GetInstanceID())
        {
            return true;
        }
        return false;
    }

    //Grid
    
    public static bool IsTussocksMemberOfGrid(GameObject gridGO, List<GameObject> gridChildTussock)
    {
        GrasslandGrid currentGrid = gridGO.GetComponent<GrasslandGrid>();
        if (currentGrid != null)
        {

            for (int i = 0; i < gridChildTussock.Count; i++)
            {
                if (currentGrid.tussockList[i].gameObject.GetInstanceID() != gridChildTussock[i].GetInstanceID())
                {
                    return false;
                }
            }

        }
        return true;
    }

    public static bool IsTussockListAndGridChildEqual(GameObject gridGO, List<GameObject> gridChildTussock)
    {
        GrasslandGrid currentGrid = gridGO.GetComponent<GrasslandGrid>();

        if (currentGrid != null)
        {
            int gridChildCount = gridChildTussock.Count;
            if (currentGrid.tussockList.Count == gridChildCount)
            {
                return true;
            }

        }
        return false;
    }
}
