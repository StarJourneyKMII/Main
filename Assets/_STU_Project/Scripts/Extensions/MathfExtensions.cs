using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfExtensions
{
    public static T GetRandom<T>(T[] array)
    {
        int r = Random.Range(0, array.Length);
        return array[r];
    }
    public static T GetRandom<T>(List<T> list)
    {
        int r = Random.Range(0, list.Count);
        return list[r];
    }
}
