using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDown : MonoBehaviour
{
    Rigidbody2D rigiType;
    [SerializeField] float freeFallT = 1f;
    [SerializeField] float disappearT = 0.5f;
    [SerializeField] float regenerationT = 2f;

    void Start()
    {
        rigiType = GetComponent<Rigidbody2D>();
        rigiType.bodyType = RigidbodyType2D.Kinematic;
        //Simulate Physics2D.gravity = new Vector2(0f, 0f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Invoke("FreeFall", freeFallT);
        }
    }

    private void FreeFall()
    {
        rigiType.bodyType = RigidbodyType2D.Dynamic;
        // Simulate Physics2D.gravity = new Vector2(0f, -40f);
        Invoke("Disappear", disappearT);
    }

    
    private void Disappear()
    {
        gameObject.SetActive(false);

        if (isRecover) 
        {
            Invoke("Disappear", regenerationT);
        }
    }

    [SerializeField] bool isRecover = true;
    private void Recovery() 
    {
        if (regenerationT != 0)
        {
            gameObject.SetActive(true);
            rigiType.bodyType = RigidbodyType2D.Kinematic;
        }
        else
        {
            Destroy(this.gameObject);
            // Destroy(gameObject, 1.0f);
        }
    }

    
}
