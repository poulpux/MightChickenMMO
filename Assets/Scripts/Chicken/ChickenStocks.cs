using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class ChickenStocks : StateManager
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        InstantiateAll();
        AllEvent();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private void InstantiateAll()
    {
        wait.InitState(onWaitEnter, onWaitUpdate, onWaitFixedUpdate, onWaitExit);
        //jump.InitState(onJumpEnter, onJumpUpdate, onJumpFixedUpdate, onJumpExit);
        getUp.InitState(onGetUpEnter, onGetUpUpdate, onGetUpFixedUpdate, onGetUpExit);
        basicAttack.InitState(onBasicAttackEnter, onBasicAttackUpdate, onBasicAttackFixedUpdate, onBasicAttackExit);
        skill1.InitState(onSkill1Enter, onSkill1Update, onSkill1FixedUpdate, onSkill1Exit);
        skill2.InitState(onSkill2Enter, onSkill2Update, onSkill2FixedUpdate, onSkill2Exit);
        ForcedCurrentState(wait);
    }

    private void AllEvent()
    {
        
    }
}