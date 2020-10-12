using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BlackboardConditionalInt))]
public class BlackboardConditionalIntDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(property.FindPropertyRelative(BlackboardConditionalString.ComparatorVarName), GUIContent.none);
        SerializedProperty boolProp = property.FindPropertyRelative(BlackboardConditionalString.ComparedValVarName);
        EditorGUILayout.PropertyField(boolProp, GUIContent.none);
        EditorGUILayout.EndHorizontal();
        EditorGUI.EndProperty();
    }
}
