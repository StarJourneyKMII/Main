using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportDoor : MonoBehaviour
{
    [SerializeField] private Teleporter teleporter;
    [SerializeField] private GameObject focusCamera;
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
                {
                    //focusCamera.SetActive(true);
                    teleporter.GoToB(collision.gameObject);
                }
                else if(name == "DoorB")
                {
                    //focusCamera.SetActive(true);
                    teleporter.GoToA(collision.gameObject);
                }
            }
        }
    }
}
