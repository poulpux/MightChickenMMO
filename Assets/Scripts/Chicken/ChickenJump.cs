using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChickenJump : MonoBehaviour
{
    [SerializeField] private GameObject leg;
    [SerializeField] private float jumpForce = 6f;
    private GameObject target;
    void Start()
    {
        target = GameObject.Find(name == "Chicken1" ? "Chicken2" : "Chicken1");
        leg.GetComponent<TouchGround>().touchGroundEvent.AddListener(() => Jump(GetComponent<Rigidbody2D>()));
    }

    private void Jump(Rigidbody2D rb)
    {
        //Calculate the x direction
        float x = transform.eulerAngles.z > 180f ? (180f - transform.eulerAngles.z) / 180f : transform.eulerAngles.z / 180f;
        Debug.Log(transform.eulerAngles.z+" "+ x);
        rb.AddForce(new Vector2(-x * jumpForce, jumpForce), ForceMode2D.Impulse);
    }
}
