using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [SerializeField] private Teleporter teleporter;
    private float timer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (teleporter.isCD) return;

        if (collision.CompareTag("Player"))
        {
            timer = teleporter.teleportSec;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (teleporter.isCD) return;

        if(collision.CompareTag("Player"))
        {
            timer -= teleporter.teleportSec;
            if(timer <= 0)
            {
                if (name == "DoorA")
                    teleporter.GoToB(collision.gameObject);
                else if(name == "DoorB")
                    teleporter.GoToA(collision.gameObject);
            }
        }
    }
}
