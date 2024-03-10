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
    [SerializeField] float spdRotation; 
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

        target = GameObject.Find(name == "Chicken1" ? "Chicken2" : "Chicken1");
        rb = target.GetComponent<Rigidbody2D>();    
    }
}