using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bu sınıf transform sınıfını deep copy yapamadığımız için yazıldı.
public class EasyTransform
{
    public Vector3 position;
    public Vector3 rotationEuler;

    public EasyTransform(Vector3 pos, Vector3 rot)
    {
        this.position = pos;
        this.rotationEuler = rot;

    }

}
