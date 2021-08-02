using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CreateBuildingPlacementDUMMY : MonoBehaviour
{
    
    public GameObject parentGrid;
    public GameObject planePrefab;
    private int gridSize = 9;
    private Vector3 origin;

    void Start()
    {
        origin = Vector3.zero;
        createBuildingPlacement();
    }




    void createBuildingPlacement()
    {
        for (int j = 0; j < gridSize + 1; j++)
        {

            for (int k = 0; k < gridSize + 1; k++)
            {
                GameObject newPlanePart1 = Instantiate(planePrefab, parentGrid.transform);
                GameObject newPlanePart2 = Instantiate(planePrefab, parentGrid.transform);
                GameObject newPlanePart3 = Instantiate(planePrefab, parentGrid.transform);
                GameObject newPlanePart4 = Instantiate(planePrefab, parentGrid.transform);

                



                newPlanePart1.transform.localPosition = new Vector3(j, 0, k);
                newPlanePart2.transform.localPosition = new Vector3(-j, 0, -k);
                newPlanePart3.transform.localPosition = new Vector3(j, 0, -k);
                newPlanePart4.transform.localPosition = new Vector3(-j, 0, k);



            }
        }
    }




    // Update is called once per frame
    void Update()
    {

    }
}
