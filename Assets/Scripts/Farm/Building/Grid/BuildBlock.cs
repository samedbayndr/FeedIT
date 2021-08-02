using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBlock : MonoBehaviour
{
    public Material availableMat;
    public Material outOfPlaceMat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider collider)
    {
        this.GetComponent<MeshRenderer>().material = outOfPlaceMat;
    }

    public void OnTriggerExit(Collider collider)
    {
        this.GetComponent<MeshRenderer>().material = availableMat;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
