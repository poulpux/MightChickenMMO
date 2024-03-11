using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChickenManager))]
[RequireComponent(typeof(Rigidbody2D))]
public class ChickenJump : MonoBehaviour
{
    private ChickenManager CM;  
    void Start()
    {
        CM = GetComponent<ChickenManager>();
        CM.leg.GetComponent<TouchGround>().touchGroundEvent.AddListener(() => Jump(GetComponent<Rigidbody2D>()));
    }

    private void Jump(Rigidbody2D rb)
    {
        float valueX = CM.eyeRotator.transform.eulerAngles.y >=90f ? -0.66f : 0.66f;
        rb.AddForce(new Vector2(valueX * CM.manager.jumpForce, CM.manager.jumpForce), ForceMode2D.Impulse);
    }
}
