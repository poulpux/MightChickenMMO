using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(CircleCollider2D))]
public class ChickenManager : MonoBehaviour
{
    public ChickenObject manager;
    [Header("======Stats======")]
    [Space(10)]
    public float rotationSpd = 0.2f;

    [Header("======Part======")]
    [Space(10)]
    public GameObject eyeRotator;
    public GameObject leg;


    [HideInInspector] public GameObject target;
    void Awake()
    {
        AssignVariablesAndValues();
    }

    private void AssignVariablesAndValues()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = manager.weight;
        rb.gravityScale = manager.gravType == GRAVITYFEEL.HOVERS ? 0.5f : 1.0f;

        transform.localScale = Vector3.one * manager.radius;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = manager.sprite!= null ? manager.sprite : spriteRenderer.sprite;

        target = GameObject.Find(name == "Chicken1" ? "Chicken2" : "Chicken1");
    }
}
