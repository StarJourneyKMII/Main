using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundPlayer : MonoBehaviour
{
    public void PlaySound(Sound sound)
    {
        SoundManager.Instance.Play(sound);
    }
}
