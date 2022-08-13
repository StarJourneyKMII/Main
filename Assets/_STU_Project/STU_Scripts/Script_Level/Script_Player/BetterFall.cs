using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterFall : MonoBehaviour
{
    public float fallForce = 1f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.velocity.y <= 0 && Physics2D.gravity.y < 0)
        {
            rb.AddForce(Vector2.down * fallForce);
        }
        else if (rb.velocity.y >= 0 && Physics2D.gravity.y > 0)
        {
            rb.AddForce(Vector2.up * fallForce);
        }
    }
}
