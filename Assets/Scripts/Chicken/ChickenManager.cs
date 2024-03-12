using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(CircleCollider2D))]
public class ChickenManager : MonoBehaviour
{
    public ChickenObject stats;
    [Header("======Stats======")]
    [Space(10)]
    public float rotationSpd = 0.2f;

    [Header("======Part======")]
    [Space(10)]
    public GameObject eyeRotator;
    public GameObject leg;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public GameObject target;
    void Awake()
    {
        AssignVariablesAndValues();
    }

    private void AssignVariablesAndValues()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = stats.weight;
        rb.gravityScale = stats.gravType == GRAVITYFEEL.HOVERS ? 0.5f : 1.0f;

        transform.localScale = Vector3.one * stats.radius;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = stats.sprite!= null ? stats.sprite : spriteRenderer.sprite;

        target = GameObject.Find(name == "Chicken1" ? "Chicken2" : "Chicken1");
    }
}
