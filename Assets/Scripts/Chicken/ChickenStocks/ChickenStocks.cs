using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChickenManager))]
[RequireComponent(typeof(Rigidbody2D))]
public partial class ChickenStocks : StateManager
{
    private ChickenManager CM;

    protected override void Awake()
    {
        base.Awake();

        InstantiateAll();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        FlipFromEnnemy();
        LookAtEnnemy();
    }

    private void InstantiateAll()
    {
        wait.InitState(onWaitEnter, onWaitUpdate, onWaitFixedUpdate, onWaitExit);
        getUp.InitState(onGetUpEnter, onGetUpUpdate, onGetUpFixedUpdate, onGetUpExit);
        basicAttack.InitState(onBasicAttackEnter, onBasicAttackUpdate, onBasicAttackFixedUpdate, onBasicAttackExit);
        skill1.InitState(onSkill1Enter, onSkill1Update, onSkill1FixedUpdate, onSkill1Exit);
        skill2.InitState(onSkill2Enter, onSkill2Update, onSkill2FixedUpdate, onSkill2Exit);
        ForcedCurrentState(wait);

        CM = GetComponent<ChickenManager>();
    }

    protected void LookAtEnnemy()
    {
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f), CM.rotationSpd * Time.deltaTime);
    }

    private void FlipFromEnnemy()
    {

        CM.eyeRotator.transform.localEulerAngles = Vector3.Lerp(CM.eyeRotator.transform.localEulerAngles, new Vector3(0f, CherchSideToFlip(), 0f), 5f*Time.deltaTime);
    }

    private float CherchSideToFlip()
    {
        return CM.target.transform.position.x < transform.position.x ? 180f : 0f;
    }
}