using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCtrl : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.rotation *= Quaternion.Euler(180, 0, 0);
            //GameObject.Find("Player").transform.rotation = Quaternion.Euler(180f, 0f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Camera.main.transform.position = new Vector3(7.5f, 0, 10f);
            Camera.main.transform.rotation *= Quaternion.Euler(180, 0, 0);
        }
    }
}
