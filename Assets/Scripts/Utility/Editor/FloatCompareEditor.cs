using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(FloatCompare))]
public class FloatCompareEditor : PropertyDrawer
{
    const int valueWidth = 50;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        SerializedProperty value = property.FindPropertyRelative("Value");
        SerializedProperty comparator = property.FindPropertyRelative("Comparator");
        SerializedProperty name = property.FindPropertyRelative("Name");

        Rect pos = new Rect(position.x, position.y, position.width - valueWidth - 3, position.height);
        comparator.enumValueIndex = EditorGUI.Popup(pos, name.stringValue, comparator.enumValueIndex, comparator.enumDisplayNames);

        
        pos = new Rect(position.width - valueWidth + 14, position.y, valueWidth, position.height);
        value.floatValue = EditorGUI.FloatField(pos, value.floatValue);

        EditorGUI.indentLevel = indent;
    }
}
