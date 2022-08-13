using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetChildWithObject : MonoBehaviour
{
    public Transform parent;

    [ContextMenu("ResetChildPos")]
    public void ResetChildPos()
    {
        int referenceChildCount = parent.childCount;
        int ownChildCount = transform.childCount;
        if(ownChildCount != referenceChildCount)
        {
            Debug.Log("¸É»ô©Î§R°£¨ì : " + referenceChildCount);
        }
        else
        {
            for(int i = 0; i < ownChildCount; i++)
            {
                transform.GetChild(i).transform.position = parent.GetChild(i).transform.position;
            }
        }
    }
}
