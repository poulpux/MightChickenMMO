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
        Vector3 targetDirection = target.transform.position - transform.position;

        if (targetDirection != Vector3.zero)
        {
            Vector3 targetRotation = Quaternion.LookRotation(targetDirection).eulerAngles;
            //Vector3 newEUler = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime).eulerAngles;
            //Debug.Log(newEUler);
            transform.eulerAngles = new Vector3(0f, 0f, -(180f-targetRotation.y)  *0.1f);
            
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
        ////Calculate the x direction
        //float x = transform.eulerAngles.z > 180f ? (180f - rb.transform.eulerAngles.z) : rb.transform.eulerAngles.z ;
        //Vector3.Lerp(transform.eulerAngles, new Vector3(0f,0f,x))
        //transform.Rotate(new Vector3(0f, 0f, 1), x);
    }
}