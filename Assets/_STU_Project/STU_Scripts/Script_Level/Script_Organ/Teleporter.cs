using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeleportType { TwoWay, AToB, BToA }

[System.Serializable]
public struct TargetSetting
{
    public Transform target;
    public Vector3 offset;
    public bool isInvert;

    public Vector3 teleportPoint
    {
        get { return target.position + offset; }
    }
}

public class Teleporter : MonoBehaviour
{
    public TeleportType teleportType = TeleportType.TwoWay;
    public float teleportSec = 0;
    public float teleportCD = 1f;

    [Header("TargetAB")]
    public bool debug;
    public TargetSetting doorA;
    public TargetSetting doorB;

    public bool isCD { get; private set; }
    
    public void GoToA(GameObject target)
    {
        target.transform.position = doorA.teleportPoint;
        target.GetComponent<PlayerCtrl>().AntiGravityByProps(doorA.isInvert);
        StartCoroutine(EnterCD());
    }

    public void GoToB(GameObject target)
    {
        target.transform.position = doorB.teleportPoint;
        target.GetComponent<PlayerCtrl>().AntiGravityByProps(doorB.isInvert);
        StartCoroutine(EnterCD());
    }

    private IEnumerator EnterCD()
    {
        isCD = true;
        yield return new WaitForSeconds(teleportCD);
        isCD = false;
    }

    //畫線輔助用
    private void OnDrawGizmos()
    {
        if (debug == false) return;
        Gizmos.color = Color.red;
        Vector3 doorAPos = doorA.target.position + doorA.offset;
        Vector3 doorBPos = doorB.target.position + doorB.offset;
        bool isRight = doorBPos.x > doorAPos.x;
        ExtensionGizmo.DrawWireDisc(doorAPos, Vector3.forward, 0.3f);
        ExtensionGizmo.DrawArrow(doorAPos, (doorA.isInvert ? Vector3.down : Vector3.up) * 1.5f, 0.7f, 30);
        ExtensionGizmo.Label(doorAPos + (isRight ? Vector3.left * 3f : Vector3.right * 1.5f) + Vector3.down * 0.5f, "DoorA", Color.red);
        ExtensionGizmo.DrawWireDisc(doorBPos, Vector3.forward, 0.3f);
        ExtensionGizmo.DrawArrow(doorBPos, (doorB.isInvert ? Vector3.down : Vector3.up) * 1.5f, 0.7f, 30);
        ExtensionGizmo.Label(doorBPos + (isRight ? Vector3.right * 1.5f : Vector3.left * 3f) + Vector3.down * 0.5f, "DoorB", Color.red);

        Gizmos.color = Color.yellow;
        if(teleportType == TeleportType.TwoWay)
        {
            ExtensionGizmo.DrawArrow_Point(doorAPos, doorBPos, 0.2f, 1, true, 1.5f);
            ExtensionGizmo.DrawArrow_Point(doorBPos, doorAPos, 0.2f, 1, true, 1.5f);
        }
        else if(teleportType == TeleportType.AToB)
        {
            ExtensionGizmo.DrawArrow_Point(doorAPos, doorBPos, 0, 1, false, 1.5f);
        }
        else if(teleportType == TeleportType.BToA)
        {
            ExtensionGizmo.DrawArrow_Point(doorBPos, doorAPos, 0, 1, false, 1.5f);
        }
    }
}
