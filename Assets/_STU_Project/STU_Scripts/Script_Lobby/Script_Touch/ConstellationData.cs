using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ConstelltionData", menuName = "New Constellation")]
public class ConstellationData : BaseData
{
    [Header("互動對話")]
    public Dialogue[] dialogues;
}

[System.Serializable]
public struct Dialogue
{
    public Animation animation;
    [TextArea]
    public string content;
}
