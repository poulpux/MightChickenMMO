using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ChickenBasic
{
    State getUp = new State();

    protected override void onGetUpEnter()
    {
       base.onGetUpEnter();
    }
    protected override void onGetUpUpdate()
    {
        base.onGetUpUpdate();
    }
    protected override void onGetUpFixedUpdate()
    {
        base.onGetUpFixedUpdate();
    }

    protected override void onGetUpExit()
    {
        base.onGetUpExit();
    }
}