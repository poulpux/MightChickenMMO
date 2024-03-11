using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChickenStocks
{
    State wait = new State();
    private float waitTimer;
    protected virtual void onWaitEnter()
    {
       
    }
    protected virtual void onWaitUpdate()
    {

    }
    protected virtual void onWaitFixedUpdate()
    {
        if(distanceOk())
            waitTimer += Time.deltaTime;
        else
            waitTimer = 0;  

        if (waitTimer > CM.stats.basicAttackCldwn)
            BasicAttack();

    }

    protected virtual void onWaitExit()
    {

    }

    private bool distanceOk()
    {
        return Vector3.Distance(CM.target.transform.position, transform.position) < CM.stats.basicAttackDist ? true : false;
    }

    private void BasicAttack()
    {
        Vector3 targetDirection = CM.target.transform.position - transform.position;
        CM.rb.AddForce(Vector2.ClampMagnitude(targetDirection, 1f) * CM.stats.basicAttackForce,ForceMode2D.Impulse);
        waitTimer = 0f;
    }
}