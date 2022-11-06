using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditName : MonoBehaviour
{
    public InputField inputField;
    public Text playerNameText;
    public Text playerNameText2;

    public void Ok()
    {
        playerNameText.text = inputField.text;
        playerNameText2.text = inputField.text;
    }
}
