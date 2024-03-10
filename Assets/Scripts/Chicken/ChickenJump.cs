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
        //float x = transform.eulerAngles.y >= 180f ? (180f - (transform.eulerAngles.z - 180f)) / 180f : transform.eulerAngles.z / 180f;
        //float posNeg = transform.eulerAngles.y >= 180f ? -1 : 1;
        //Debug.Log(transform.eulerAngles.z +" euler : "+ x);
        float valueX = transform.eulerAngles.y >=90f ? -0.66f : 0.66f;
        rb.AddForce(new Vector2(valueX * jumpForce, jumpForce), ForceMode2D.Impulse);
    }
}
