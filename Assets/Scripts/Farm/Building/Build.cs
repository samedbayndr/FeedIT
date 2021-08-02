using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Bu sınıf binaların üzerine attachlenecek bir component olacak kullanılacaktır.
//Binaya iki kere sağ tıklandığında bina silinecektir.
public class Build : MonoBehaviour
{
    public string id;
    public double maintenanceCost;
    public double buildCost;
}
