using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string _id = Guid.NewGuid().ToString();

    [SerializeField] 
    private static SerializableDictionary<string, GameObject> idDataBase = 
        new SerializableDictionary<string, GameObject>();

    public string ID
    {
        get { return _id; }
    }

    private void OnValidate()
    {
        if (idDataBase.ContainsKey(_id))
            Generate();
        else
            idDataBase.Add(_id, this.gameObject);
    }
    private void OnDestroy()
    {
        if(idDataBase.ContainsKey(_id))
            idDataBase.Remove(_id);
    }
    private void Generate()
    {
        _id = Guid.NewGuid().ToString();
        idDataBase.Add(_id, this.gameObject);
    }
}
