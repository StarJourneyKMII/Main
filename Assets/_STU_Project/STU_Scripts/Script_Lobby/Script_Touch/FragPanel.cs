using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FragPanel : MonoBehaviourSingleton<FragPanel>
{
    public GameObject showPanel;
    public Image frag01Image;
    public Image frag02Image;
    public Image frag03Image;
    public Image frag04Image;

    public void ShowDetail(SpacePartsData data)
    {
        showPanel.SetActive(true);
        frag01Image.sprite = data.frags[0].showSprite;
        frag02Image.sprite = data.frags[1].showSprite;
        frag03Image .sprite = data.frags[2].showSprite;
        frag04Image.sprite = data.frags[3].showSprite;
    }
}
