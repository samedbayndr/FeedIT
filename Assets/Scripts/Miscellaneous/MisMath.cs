using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MisMath 
{
}

public static class Probability
{
    public static int Roll(int minRange, int maxRange)
    {
        return Random.Range(minRange, maxRange+1);
    }
}

