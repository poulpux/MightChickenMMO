using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChickenBasic
{
    State wait = new State();

    protected override void onWaitEnter()
    {
        base.onWaitEnter();
    }
    protected override void onWaitUpdate()
    {
        base.onWaitUpdate();
    }
    protected override void onWaitFixedUpdate()
    {
        base.onWaitFixedUpdate();

        LookAtEnnemy();
    }

    protected override void onWaitExit()
    {
        base .onWaitExit();
    }
}