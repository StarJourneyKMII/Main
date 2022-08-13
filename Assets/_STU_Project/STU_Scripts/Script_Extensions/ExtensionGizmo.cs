using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionGizmo
{
    public static void DrawArrow_Point(Vector3 posA, Vector3 posB, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Vector3 direction = posB - posA;
        Gizmos.DrawLine(posA, posB);
        Vector3 right = Quaternion.Euler(0, 0, arrowHeadAngle) * -direction;
        Vector3 left = Quaternion.Euler(0, 0, -arrowHeadAngle) * -direction;
        Gizmos.DrawLine(posB, posB + right.normalized * arrowHeadLength);
        Gizmos.DrawLine(posB, posB + left.normalized * arrowHeadLength);
    }
    public static void DrawArrow_Point(Vector3 posA, Vector3 posB, float XOffset, float sacleReduce, bool onlyRight, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Vector3 direction = (posB - posA).normalized;
        Vector3 offset = Vector3.Cross(direction, Vector3.back) * XOffset;
        posA += direction * sacleReduce / 2;
        posB -= direction * sacleReduce / 2;
        posA -= offset;
        posB -= offset;
        Gizmos.DrawLine(posA, posB);
        Vector3 right = Quaternion.Euler(0, 0, arrowHeadAngle) * -direction;
        Vector3 left = Quaternion.Euler(0, 0, -arrowHeadAngle) * -direction;
        Gizmos.DrawLine(posB, posB + right.normalized * arrowHeadLength);
        if (onlyRight) return;
        Gizmos.DrawLine(posB, posB + left.normalized * arrowHeadLength);
    }
    public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.DrawRay(pos, direction);
        
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }

    public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
    {
        Gizmos.color = color;
        Gizmos.DrawRay(pos, direction);

        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
        Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
        Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
    }

    public static void DrawWireArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Gizmos.color;
        UnityEditor.Handles.DrawWireArc(center, normal, from, angle, radius);
#endif
    }

    public static void DrawSolidArc(Vector3 center, Vector3 normal, Vector3 from, float angle, float radius)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Gizmos.color;
        UnityEditor.Handles.DrawSolidArc(center, normal, from, angle, radius);
#endif
    }

    public static void DrawWireDisc(Vector3 center, Vector3 normal, float radius)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Gizmos.color;
        UnityEditor.Handles.DrawWireDisc(center, normal, radius);
#endif
    }

    public static void DrawSolidDisc(Vector3 center, Vector3 normal, float radius)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Gizmos.color;
        UnityEditor.Handles.DrawSolidDisc(center, normal, radius);
#endif
    }

    public static void Label(Vector3 position, string text)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Gizmos.color;
        UnityEditor.Handles.Label(position, text);
#endif
    }
    public static void Label(Vector3 position, string text, Color textColor)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Gizmos.color;
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = textColor;
        UnityEditor.Handles.Label(position, text, style);
#endif
    }

    public static void Label(Vector3 position, GUIContent content, GUIStyle style)
    {
#if UNITY_EDITOR
        UnityEditor.Handles.color = Gizmos.color;
        UnityEditor.Handles.Label(position, content, style);
#endif
    }

}
