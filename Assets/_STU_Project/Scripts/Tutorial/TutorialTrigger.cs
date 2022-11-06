using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _tutorialObject = null;

    private bool _isPlaying = false;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !_isPlaying)
        {
            _tutorialObject.gameObject.SetActive(true);
            _isPlaying = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isPlaying)
        {
            _tutorialObject.gameObject.SetActive(false);
            _isPlaying = false;
        }
    }
}
