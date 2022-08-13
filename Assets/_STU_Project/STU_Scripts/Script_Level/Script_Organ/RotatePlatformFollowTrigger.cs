using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatformFollowTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform.parent);
            collision.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
            collision.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }
}
