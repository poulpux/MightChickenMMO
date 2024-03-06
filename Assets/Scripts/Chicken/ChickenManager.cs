using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(CircleCollider2D))]
public class ChickenManager : MonoBehaviour
{
    [SerializeField] private ChickenObject manager;
    void Start()
    {
        AssignVariablesAndValues();
    }

    private void AssignVariablesAndValues()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.mass = manager.weight;
        rb.gravityScale = manager.gravType == GRAVITYFEEL.HOVERS ? 0.5f : 1.0f;

        CircleCollider2D Ccollider = GetComponent<CircleCollider2D>();
        Ccollider.radius = manager.radius;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = manager.sprite!= null ? manager.sprite : spriteRenderer.sprite;
    }
}
