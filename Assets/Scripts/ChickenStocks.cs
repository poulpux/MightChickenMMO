using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class ChickenStocks : StateManager
{

    protected override void Start()
    {
        base.Start();

        wait.InitState(onWaitEnter, onWaitUpdate, onWaitExit); 
        jump.InitState(onJumpEnter, onJumpUpdate, onJumpExit); 
        getUp.InitState(onGetUpEnter, onGetUpUpdate, onGetUpExit); 
        basicAttack.InitState(onBasicAttackEnter, onBasicAttackUpdate, onBasicAttackExit); 
        skill1.InitState(onSkill1Enter, onSkill1Update, onSkill1Exit); 
        skill2.InitState(onSkill2Enter, onSkill2Update, onSkill2Exit); 
        ForcedCurrentState(wait);
    }

    protected override void Update()
    {
        base.Update();
    }
}