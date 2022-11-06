using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum RotateType
{
    Clockwise, Anticlockwise, PingPongClockwise, PingPongAnticlockwise
}
public class RotatePlatform : MonoBehaviour
{
    public Transform platform;
    public RotateType rotateMode;
    public float rotateSec = 1.5f;
    public float waitSec = 2f;
    public AnimationCurve rotateCurve;

    private void Start()
    {
        Rotate();
    }

    private void Rotate()
    {
        int rotateValue = 0;
        if (rotateMode == RotateType.Clockwise)
            rotateValue = -180;
        else if (rotateMode == RotateType.Anticlockwise)
            rotateValue = 180;
        else if (rotateMode == RotateType.PingPongClockwise)
        {
            if (platform.localEulerAngles.z == 180)
                rotateValue = 180;
            else
                rotateValue = -180;
        }
        else if (rotateMode == RotateType.PingPongAnticlockwise)
        {
            if (platform.localEulerAngles.z == 180)
                rotateValue = -180;
            else
                rotateValue = 180;
        }

        platform.DORotate(new Vector3(0, 0, rotateValue), rotateSec,  RotateMode.WorldAxisAdd).SetEase(rotateCurve).OnComplete(() =>
        {
            Invoke("Rotate", waitSec);
        });
    }
}
