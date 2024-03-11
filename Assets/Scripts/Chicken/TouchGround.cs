using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class TouchGround : MonoBehaviour
{
    [HideInInspector] public UnityEvent touchGroundEvent = new UnityEvent();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.layer != LayerMask.NameToLayer("Chicken"))
            touchGroundEvent.Invoke();
    }
}
