using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void SetVelocityX(this Rigidbody2D rb, float newX)
    {
        Vector3 temp = rb.velocity;
        temp.x = newX;
        rb.velocity = temp;
    }
    public static void SetVelocityY(this Rigidbody2D rb, float newY)
    {
        Vector3 temp = rb.velocity;
        temp.y = newY;
        rb.velocity = temp;
    }
    public static void SetLocalPositionX(this Transform t, float newX)
    {
        Vector3 temp = t.localPosition;
        temp.x= newX;
        t.localPosition = temp;
    }
    public static void SetLocalPositionY(this Transform t, float newY)
    {
        Vector3 temp = t.localPosition;
        temp.y = newY;
        t.localPosition = temp;
    }
    public static void SetLocalPositionZ(this Transform t, float newZ)
    {
        Vector3 temp = t.localPosition;
        temp.z = newZ;
        t.localPosition = temp;
    }

    public static Vector3 GetZeroZ(this Vector3 value)
    {
        Vector3 temp = value;
        temp.z = 0;
        return temp;
    }

    public static Color GetAlphaColor(this Color value, float alpha)
    {
        Color temp = value;
        temp.a = alpha;
        return temp;
    }
}
