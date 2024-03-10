using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public partial class ChickenStocks : StateManager
{
    private GameObject target;
    private Rigidbody2D rb;

    [Header("======Stats======")]
    [Space(10)]
    [SerializeField] float spdRotation, timeToFlip; 
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
    }

    private void InstantiateAll()
    {
        wait.InitState(onWaitEnter, onWaitUpdate, onWaitFixedUpdate, onWaitExit);
        getUp.InitState(onGetUpEnter, onGetUpUpdate, onGetUpFixedUpdate, onGetUpExit);
        basicAttack.InitState(onBasicAttackEnter, onBasicAttackUpdate, onBasicAttackFixedUpdate, onBasicAttackExit);
        skill1.InitState(onSkill1Enter, onSkill1Update, onSkill1FixedUpdate, onSkill1Exit);
        skill2.InitState(onSkill2Enter, onSkill2Update, onSkill2FixedUpdate, onSkill2Exit);
        ForcedCurrentState(wait);

        target = GameObject.Find(name == "Chicken1" ? "Chicken2" : "Chicken1");
        rb = target.GetComponent<Rigidbody2D>();    
    }

    protected void LookAtEnnemy()
    {
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f), 0.2f * Time.deltaTime);
    }

    private void FlipFromEnnemy()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, CherchSideToFlip(), transform.eulerAngles.z);
    }

    private float CherchSideToFlip()
    {
        return target.transform.position.x < transform.position.x ? 180f : 0f;
    }
}