using UnityEngine;
//
using System.Collections.Generic;

public abstract class StateBaseClass
{
    //A reference to the enemyscript that controls this class
    public FSM_EnemyScript enemyScript;


    //Whenever a state of this type is created we need to give it a reference to an enemy script
    public StateBaseClass(FSM_EnemyScript enemyScript)
    {
        this.enemyScript = enemyScript;
    }

    //A function that should be called when entering this state
    public abstract void OnEnterState();
    //A function that will be called when exiting this state
    public abstract void OnExitState();
    //A function called when exiting out of this state
    public abstract void ChangeState(StateBaseClass newState, ref StateBaseClass currentState);
    //A function called every frame while this state is the one active
    public abstract void OnEveryFrame();
    //A function called every fixed frame while this state is the one active
    public abstract void OnEveryPhysicsFrame();

}