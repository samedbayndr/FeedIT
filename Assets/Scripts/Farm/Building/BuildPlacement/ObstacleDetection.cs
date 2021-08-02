using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetection : MonoBehaviour
{

    public Material availableHighlightMaterial;
    public Material outOfPlaceHighlightMaterial;

    public GameObject HighlightGameObject;

    public bool isBuildable;
    public List<Collider> collidedObjects = new List<Collider>();
    public void Start()
    {

    }





    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Build") || collider.transform.tag == "Fence")
        {
            if (HighlightGameObject !=null)
            {
                collidedObjects.Add(collider);
                if (HighlightGameObject.activeSelf !=false && collidedObjects.Count > 0)
                    HighlightGameObject.GetComponent<MeshRenderer>().material = outOfPlaceHighlightMaterial;
            }

            

        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Build") || collider.transform.tag == "Fence")
        {

            if (HighlightGameObject != null)
            {
                collidedObjects.Remove(collider);
                if (HighlightGameObject.activeSelf != false && collidedObjects.Count <= 0)
                    HighlightGameObject.GetComponent<MeshRenderer>().material = availableHighlightMaterial;
            }

            
        }
    }

}
