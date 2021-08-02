using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum StateList
{
    neutral,

    //Player
    building,
    grazing,
    farmManaging,
    inStartMenu,
    inOptionMenu,
    inGameMenu,

    //Worker
    workerWalking,
    workingOnFarm,
    restingOnHouse,
    instrumentPlaying,
    workerIdle,

    //Pasture Animals
    breeding,
    eating,
    feedSearching,
    pastureAnimalWalking,
    shepherdFollowing,
    restingOnBarn,
    milking,
    cropping,
    pastureAnimalIdle,

    //Farm&Pet Animals
    feedingBait,
    free,
    producing,

    //Especially Dogs
    barking

}

public class State
{
    public static readonly Dictionary<StateList, int> StatePriorityDictionary = new Dictionary<StateList, int>()
    {
        {StateList.neutral, 0},
        //Player

        {StateList.building, 10},
        {StateList.grazing, 10},
        {StateList.farmManaging, 10},
        {StateList.inStartMenu, 9999},
        {StateList.inOptionMenu, 9999},
        {StateList.inGameMenu, 100},



        //Worker


        {StateList.workerWalking, 10},
        {StateList.workerIdle, 10},
        {StateList.workingOnFarm, 10},
        {StateList.restingOnHouse, 10},
        {StateList.instrumentPlaying, 15},



        //Pasture Animals

        {StateList.breeding, int.MaxValue},
        {StateList.eating, 10},
        {StateList.feedSearching, 10},
        {StateList.shepherdFollowing, 9999999},
        {StateList.milking, int.MaxValue},
        {StateList.cropping, int.MaxValue},
        {StateList.restingOnBarn, 999},
        {StateList.pastureAnimalWalking, 999999},
        {StateList.pastureAnimalIdle, 10},


        //Farm&Pet Animals

        {StateList.feedingBait, 999},
        {StateList.free, 10},
        {StateList.producing, 9999},

    };

    public State()
    {
        setState(StateList.neutral);
    }
    private StateList activeState { get; set; } = StateList.neutral;

    public StateList getState()
    {
        return activeState;
    }

    public void setState(StateList state)
    {
        if (IsStateExist(state))
        {
            activeState = state;
        }
    }
    public void deattachState()
    {
        activeState = StateList.neutral;
    }


    public static int GetPriorityOfState(StateList state)
    {
        int statePriorityValue;
        if (StatePriorityDictionary.TryGetValue(state, out statePriorityValue))
        {
            return statePriorityValue;
        }
        else
        {
            Debug.Log(NextGenDebug.HeavyError("State priority değerine ulaşılamadı!!"));
            return -1;
        }
    }

    public static bool IsStateExist(StateList state)
    {
        if (StatePriorityDictionary.ContainsKey(state))
        {
            return true;
        }
        else
        {
            Debug.Log(NextGenDebug.HeavyError("State bulunamadı!!"));
            return false;
        }
    }

}
