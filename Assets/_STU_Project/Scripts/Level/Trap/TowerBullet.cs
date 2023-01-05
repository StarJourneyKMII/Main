using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector3 startPoint;
    private Vector3 dir;

    private float moveSpeed;
    private float maxDistance;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPoint = transform.position;
    }

    void Update()
    {
        transform.right = dir;
        rb.velocity = dir * moveSpeed;
        if (Vector3.Distance(transform.position, startPoint) >= maxDistance)
            Destroy(gameObject);
    }

    public void SetBullet(float speed, Vector3 dir, float maxDistance)
    {
        this.moveSpeed = speed;
        this.dir = dir;
        this.maxDistance = maxDistance;
    }
}
