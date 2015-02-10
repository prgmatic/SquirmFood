using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(UseReorderableList))]
public class UseReorderableListDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
    }
}
