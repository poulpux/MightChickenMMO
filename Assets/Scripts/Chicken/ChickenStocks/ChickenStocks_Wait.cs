using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChickenStocks
{
    State wait = new State();

    protected virtual void onWaitEnter()
    {
       
    }
    protected virtual void onWaitUpdate()
    {

    }
    protected virtual void onWaitFixedUpdate()
    {

    }

    protected virtual void onWaitExit()
    {

    }

    protected void LookAtEnnemy()
    {
        Vector3 direction = target.transform.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float multiplyAngle = -0.1f;
        float modifEulerY = targetRotation.eulerAngles.y > 180f ? 180f - targetRotation.eulerAngles.y : targetRotation.eulerAngles.y;  
        Vector3 futurEuler = new Vector3(0f, 0f, modifEulerY * multiplyAngle);
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, futurEuler, spdRotation);
    }
}