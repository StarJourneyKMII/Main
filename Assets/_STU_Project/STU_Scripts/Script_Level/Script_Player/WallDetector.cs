using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{
    public bool isTouching { get; private set; }
    [SerializeField] Collider2D detector = null;
    bool myDirection = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Midground1")
        {
            isTouching = true;
        }
            
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Midground1")
            isTouching = false;
    }

    public void SetDirection(bool newDirection)
    {
        if(newDirection != myDirection)
        {
            detector.offset *= -1;
            myDirection = newDirection;
        }
            
    }
}
