using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PlanetDataSelector))]
public class ScriptableSelectorPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);

        if (property.propertyType == SerializedPropertyType.String)
        {
            var oldObject = AssetDatabase.LoadAssetAtPath<PlanetData>(property.stringValue);
            EditorGUI.BeginChangeCheck();
            var newScene = EditorGUI.ObjectField(position, "Data", oldObject, typeof(PlanetData), true) as PlanetData;

            if (EditorGUI.EndChangeCheck())
            {
                var newPath = AssetDatabase.GetAssetPath(newScene);
                property.stringValue = newPath;
            }
        }
    }
}
