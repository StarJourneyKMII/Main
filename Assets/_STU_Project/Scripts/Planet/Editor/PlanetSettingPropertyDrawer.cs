using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PlanetSetting))]
public class PlanetSettingPropertyDrawer : PropertyDrawer, IEditorDrawer
{
    public float SingleLineSpace => EditorGUIUtility.singleLineHeight;
    public int DrawLineCount { get; set; }
    public Rect GetRectAndIterateLine(Rect position)
    {
        return EditorDrawingUtility.GetRectAndIterateLine(this, position);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        DrawLineCount = 0;
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty scriptableProperty = property.FindPropertyRelative("scriptable");
        SerializedProperty unLockProperty = property.FindPropertyRelative("unLock");
        SerializedProperty planetIndexProperty = property.FindPropertyRelative("planetIndex");
        SerializedProperty areaProperty = property.FindPropertyRelative("area");

        property.isExpanded = EditorGUI.Foldout(GetRectAndIterateLine(position), property.isExpanded, new GUIContent(GetSceneName(scriptableProperty.stringValue)));

        if(property.isExpanded)
        {
            EditorGUI.PropertyField(GetRectAndIterateLine(position), planetIndexProperty, new GUIContent("PlanetIndex"));
            EditorGUI.PropertyField(GetRectAndIterateLine(position), scriptableProperty, new GUIContent("Data"));
            EditorGUI.PropertyField(GetRectAndIterateLine(position), unLockProperty, new GUIContent("UnLock"));
            EditorGUI.PropertyField(GetRectAndIterateLine(position), areaProperty, new GUIContent("Areas"));
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;
        if (property.isExpanded)
        {
            SerializedProperty scriptableProperty = property.FindPropertyRelative("scriptable");
            SerializedProperty unLockProperty = property.FindPropertyRelative("unLock");
            SerializedProperty planetIndexProperty = property.FindPropertyRelative("planetIndex");
            SerializedProperty areaProperty = property.FindPropertyRelative("area");

            height += EditorGUI.GetPropertyHeight(areaProperty) + EditorGUI.GetPropertyHeight(scriptableProperty) + EditorGUI.GetPropertyHeight(unLockProperty) + EditorGUI.GetPropertyHeight(planetIndexProperty);
        }
        return height;
    }

    private string GetSceneName(string scenePath)
    {
        return scenePath.Substring(scenePath.LastIndexOf("/") + 1).Replace(".asset", "");
    }
}
