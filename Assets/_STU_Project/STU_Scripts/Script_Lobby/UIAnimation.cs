using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    public void ChangeScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }
}
