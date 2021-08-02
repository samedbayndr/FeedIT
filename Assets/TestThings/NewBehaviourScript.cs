using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float minimum = 1.0F;
    public float maximum = 10.0F;
    static float t = 0.0f;
    void Update()
    {
        transform.localScale = new Vector3(Mathf.Lerp(minimum, maximum, t), Mathf.Lerp(minimum, maximum, t), 1);
        
        // .. and increase the t interpolater
        t += 0.1f * Time.deltaTime;

        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (t > 500.0f)
        {
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
    }
}
