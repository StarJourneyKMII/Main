using MiProduction.Scene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHintLoader : MonoBehaviour
{
    [SerializeField] SceneConfig_MultipleSprites _sceneSpritesData = null;
    [SerializeField] Image _image = null;
    void OnEnable()
    {
        if(_sceneSpritesData.TryGetSceneData(out Sprite[] sprites) && sprites.Length > 0)
        {
            _image.sprite = sprites[Random.Range(0, sprites.Length)];
        }
        
    }
}
