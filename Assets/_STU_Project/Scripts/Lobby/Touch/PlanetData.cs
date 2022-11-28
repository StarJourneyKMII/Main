using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Planet", menuName = "Data/Level Data/New Planet")]
public class PlanetData : ScriptableObject
{
    [Header("����")]
    public bool unlock;
    public Sprite lockSprite;
    public Sprite unlockSprite;

    [Header("�ԲӸ��")]
    public string planetName;
    [TextArea(7, 13)]
    public string description;

    [Header("�ϰ�")]
    public int planetIndex = -1;
    public List<PlanetAreaData> planetArea;
    
    public int ContinueAreaIndex
    {
        get
        {
            for(int i = 0; i < planetArea.Count; i++)
            {
                if(planetArea[i].unLock == false)
                    return Mathf.Clamp(i - 1, 0, planetArea.Count);
            }
            return 0;
        }
    }
    public bool IsAllClear
    {
        get
        {
            foreach(PlanetAreaData area in planetArea)
            {
                if (area.unLock)
                    continue;
                else
                    return false;
            }
            return true;

        }
    }
}
