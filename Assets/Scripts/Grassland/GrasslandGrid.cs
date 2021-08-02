using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrasslandGrid : MonoBehaviour
{
    public List<Tussock> tussockList = new List<Tussock>();
    public Grassland parentGrassland;
    private void Start()
    {
        if (parentGrassland == null)
        {
            Debug.Log(NextGenDebug.HeavyError("Grassland NULL!!!"));
            return;
        }


        if (!GrasslandErrorHandle.IsGridMemberOfGrassland(parentGrassland.gameObject, this.gameObject))
        {
            Debug.Log(NextGenDebug.HeavyError("WRONG GRASSLAND!!!"));
            return;
        }
        else
        {

            if (!GrasslandErrorHandle.IsTussockListAndGridChildEqual(this.gameObject, ChildOperation.GetFirstLevelChildren(this.gameObject)))
            {
                Debug.Log(NextGenDebug.HeavyError("Tussock list count not equal to grid child game object."));

                return;
            }
            else
            {
                if (!GrasslandErrorHandle.IsTussocksMemberOfGrid(this.gameObject, ChildOperation.GetFirstLevelChildren(this.gameObject)))
                {
                    //Debug.Log(NextGenDebug.HeavyError("One or more tussock not element of this Grid!!!"));
                    return;
                }
                else
                {

                    // Safely Code!!!!
                    GameTimeManager.MinuteFinishedEvent.AddListener(updateTussocksHealth);
                }
            }
        }
    }


    public void updateTussocksHealth()
    {
        int tussockListCount = tussockList.Count;
        if (tussockListCount != 0)
        {
            for (int i = 0; i < tussockListCount; i++)
            {
                if (!tussockList[i].isHealthMaximum())
                {
                    tussockList[i].updateHealth(parentGrassland.getRegeneration());
                }
            }

        }
    }
}
