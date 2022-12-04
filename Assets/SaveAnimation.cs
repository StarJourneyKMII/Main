using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SaveAnimation : MonoBehaviour
{
    [SerializeField] private Image saveImage;

    public void Play()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        saveImage.gameObject.SetActive(true);
        saveImage.DOFade(1, 0.3f).From(0);
        yield return new WaitForSecondsRealtime(0.5f);
        saveImage.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear);
        yield return new WaitForSecondsRealtime(1.3f);
        saveImage.DOFade(0, 0.3f);
        yield return new WaitForSecondsRealtime(0.3f);
        saveImage.gameObject.SetActive(false);
    }
}
