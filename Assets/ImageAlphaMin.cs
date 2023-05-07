using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAlphaMin : MonoBehaviour
{
    private Image m_image;

    void Start()
    {
        m_image = GetComponent<Image>();

        m_image.alphaHitTestMinimumThreshold = 0.05f;
    }

}
