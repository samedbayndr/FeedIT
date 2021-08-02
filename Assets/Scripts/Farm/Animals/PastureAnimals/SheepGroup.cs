using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepGroup : PastureAnimal
{
    [Header("Sheep, Ram Special")]
    public double maxWool = 5;
    public double wool;
    public int timeForMaxWool = 15;
    public bool isWoolMaximum()
    {
        if (wool >= maxWool)
        {
            wool = maxWool;
            return true;
        }
        else
            return false;
    }

    public double shearing()
    {
        if (isWoolMaximum())
        {
            double tempWool = wool;
            wool = 0;

            TableManager.Instance.inventoryTable.wool += tempWool;
            FileOperation.SaveTextAsset(TableManager.Instance.inventoryTable,
                TableManager.Instance.inventoryTable.filePath, Extension.Json);

            return tempWool;
            
        }

        return 0;
    }

    public void gainWool()
    {

        if (!isWoolMaximum())
        {
            this.wool += (maxWool / timeForMaxWool);
        }
    }
}
