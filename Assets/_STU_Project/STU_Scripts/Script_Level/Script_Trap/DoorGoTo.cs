using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGoTo: MonoBehaviour
{

    public Transform target;
    public bool toInvert;
    public bool toOpposite;
    
    GameObject playerD;

    int index = 0;
    int s_index = 0;
    float time = 0;

    private void Start()
    {
        playerD = GameObject.Find("Player");
        index = 0;
        s_index = 0;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        time += Time.deltaTime;
        index = index + 1;

        if (index == 15)
        {
            if (other.gameObject.name == "Player")
            {
                if (toOpposite)
                {
                    playerD.GetComponent<SpriteRenderer>().flipX = false;
                    other.gameObject.transform.position = new Vector3(target.transform.position.x - 1f, target.transform.position.y, target.transform.position.z);
                    target.gameObject.SetActive(false);
                    Invoke("OneSecGone", 1f);
                }
                else
                {
                    playerD.GetComponent<SpriteRenderer>().flipX = true;
                    other.gameObject.transform.position = new Vector3(target.transform.position.x + 1f, target.transform.position.y, target.transform.position.z);
                    target.gameObject.SetActive(false);
                    Invoke("OneSecGone", 1f);
                }
                
                GameObject.Find("Player").GetComponent<PlayerCtrl>().AntiGravityByProps(toInvert);
            }

            s_index = s_index + 1;
            Debug.Log("---index: " + index + "---s_index: " + s_index + "---Time: " + time);
            index = 0;
        }
    }

    private void OneSecGone()
    {
        //target.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        target.gameObject.SetActive(true);
    }
}


