using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{

    public Tussock parentTussock;

    public void setInvisible()
    {
        this.gameObject.SetActive(false);
    }

    public void setVisible()
    {
        this.gameObject.SetActive(true);
    }
}
