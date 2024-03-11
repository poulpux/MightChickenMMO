using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChickenManager))]
[RequireComponent(typeof(Rigidbody2D))]
public class ChickenJump : MonoBehaviour
{
    private ChickenManager CM;
    private float timer;
    void Start()
    {
        CM = GetComponent<ChickenManager>();
        CM.leg.GetComponent<TouchGround>().touchGroundEvent.AddListener(() => Jump(GetComponent<Rigidbody2D>()));
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
    }
    private void Jump(Rigidbody2D rb)
    {
        if (timer > 0.1)
        {
            float valueX = CM.eyeRotator.transform.eulerAngles.y >= 90f ? -0.66f : 0.66f;
            rb.AddForce(new Vector2(valueX * CM.stats.jumpForce, CM.stats.jumpForce), ForceMode2D.Impulse);
            timer = 0f;
        }
    }
}
