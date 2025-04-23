using System.Collections.Generic;
using UnityEngine;

public class FSM_IdleState : StateBaseClass
{
    //The time that this enemy waits in idle before going on patrol
    public float waitingTime = 5f;
    //The time remaining before this enemy changes states naturally
    protected float waitingTimeLeft = 10f;




    //The constructor, which also executes the basic constructor for this class
    public FSM_IdleState(FSM_EnemyScript enemyScript) : base(enemyScript) { }

    //The function that tells this state to change to a different one
    public override void ChangeState(StateBaseClass newState, ref StateBaseClass currentState)
    {
        //The current state executes its exit function
        currentState.OnExitState();
        //The current state is changed
        currentState = newState;
        //The new current state executes its enter function
        currentState.OnEnterState();
    }

    //On enter the variables for this state are reset and we display a message to let the player know the enemy is now waiting
    public override void OnEnterState()
    {
        waitingTimeLeft = waitingTime;
        //Debug.Log("Waiting");
    }

    //Every frame we count down from the waitingTimeLeft variable. When the timer reaches zero we change the state to patrolling
    public override void OnEveryFrame()
    {
        waitingTimeLeft -= Time.deltaTime;

        if (waitingTimeLeft <= 0)
        {
            ChangeState(enemyScript.patrolState, ref enemyScript.currentState);
        }


        if (enemyScript.CheckIfPlayerVisible())
        {
            ChangeState(enemyScript.chaseState, ref enemyScript.currentState);
        }
    }

    //This state does nothing on every fixed frame
    public override void OnEveryPhysicsFrame()
    {

    }

    //When this enemy exits the idle state they display a message
    public override void OnExitState()
    {
        //Debug.Log("Alright time to do something.");
    }
}