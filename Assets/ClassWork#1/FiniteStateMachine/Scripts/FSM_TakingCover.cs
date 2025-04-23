using UnityEngine;

public class FSM_TakingCover : StateBaseClass
{
    //How close the enemy has to be to their destination to consider to have "arrived"
    public float destinationTolerance = 1f;
    //The destination of this enemy
    public Vector3 targetPos;



    public FSM_TakingCover(FSM_EnemyScript enemyScript) : base(enemyScript) { }

    public override void ChangeState(StateBaseClass newState, ref StateBaseClass currentState)
    {
        //The current state executes its exit function
        currentState.OnExitState();
        //The current state is changed
        currentState = newState;
        //The new current state executes its enter function
        currentState.OnEnterState();
    }

    //Allows us to set the target position of the investigation
    public void SetTargetPos(Vector3 targetPos)
    {
        this.targetPos = targetPos;
    }

    public override void OnEnterState()
    {
        //The enemyscript is told that their new destination is the targetPos
        enemyScript.navMeshAgent.SetDestination(targetPos);
        //We refresh the target position so it matches the one calculated by the navmesh
        targetPos = enemyScript.navMeshAgent.pathEndPosition;
        //
        //Debug.Log("Moving to " + targetPos.ToString());
    }

    public override void OnEveryFrame()
    {



        //If the distance between the enemy and its destination is less than the maximum accepted or the path cannot be reached
        if (Vector3.Distance(enemyScript.transform.position, targetPos) <= destinationTolerance
            || enemyScript.navMeshAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            //Change back to the idle state
            ChangeState(enemyScript.idleState, ref enemyScript.currentState);
        }


        if (enemyScript.CheckIfPlayerVisible())
        {
            ChangeState(enemyScript.chaseState, ref enemyScript.currentState);
        }
    }

    public override void OnEveryPhysicsFrame()
    {

    }

    public override void OnExitState()
    {
        //Debug.Log("Leaving patrol state");
    }
}