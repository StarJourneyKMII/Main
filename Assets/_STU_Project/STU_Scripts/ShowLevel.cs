using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLevel : MonoBehaviour
{
    [SerializeField] Text showStuff = null;

    // Start is called before the first frame update
    void Start()
    {
        // Display the information on the screen
        showStuff.text = "<size=120>" + GameManager.instance.levelName + "</size>\n\nLIFE X<color=red> " + GameManager.instance.life + "</color>";
        // 2 Sec call CloseMe
        Invoke("CloseMe", 2f);
    }
    void CloseMe()
    {
        // Close self window
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
