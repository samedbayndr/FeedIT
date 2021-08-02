using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmAnimal : Animal
{

    public State currentStates { get; set; } = new State();
    public StateList lastState;



}
