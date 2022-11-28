using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneTrigger : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        SceneChangeManager.Instance.LoadSceneByName(sceneName);
    }
}
