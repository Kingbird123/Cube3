using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RigidbodyConstraintsProperty))]
public class RigidbodyConstraintsPropertyDrawer : PropertyDrawer
{
    private SerializedProperty sourceRef;

    private SerializedProperty freezePositionX;
    private SerializedProperty freezePositionY;
    private SerializedProperty freezePositionZ;
    private SerializedProperty freezeRotationX;
    private SerializedProperty freezeRotationY;
    private SerializedProperty freezeRotationZ;
    private SerializedProperty constraints;

    //need to set field amount manually if you add more fields
    private int fieldAmount = 2;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        sourceRef = property;
        GetProperties();
        CalculateFieldAmount();
        //set the height of the drawer by the field size and padding
        return (fieldAmount * EditorGUIUtility.singleLineHeight) + (fieldAmount * EditorGUIUtility.standardVerticalSpacing);
    }

    void CalculateFieldAmount()
    {
        fieldAmount = 1;
        if (sourceRef.isExpanded)
        {
            fieldAmount = 3;
        }
        
    }

    public virtual void GetProperties()
    {
        //get property values
        freezePositionX = sourceRef.FindPropertyRelative("freezePositionX");
        freezePositionY = sourceRef.FindPropertyRelative("freezePositionY");
        freezePositionZ = sourceRef.FindPropertyRelative("freezePositionZ");
        freezeRotationX = sourceRef.FindPropertyRelative("freezeRotationX");
        freezeRotationY = sourceRef.FindPropertyRelative("freezeRotationY");
        freezeRotationZ = sourceRef.FindPropertyRelative("freezeRotationZ");
        constraints = sourceRef.FindPropertyRelative("constraints");
    }


    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        //divide all field heights by the field amount..then minus the padding
        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Draw Prefix label...this will push all other content to the right
        //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Draw non-indented label instead
        //EditorGUI.LabelField(position, property.displayName);

        //draw dropdown
        sourceRef.isExpanded = EditorGUI.Foldout(position, sourceRef.isExpanded, "Constraints");

        // Get the start indent level
        var indent = EditorGUI.indentLevel;
        // Set indent amount
        EditorGUI.indentLevel = indent + 1;

        DisplayGUIElements(position, property, label);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public virtual void DisplayGUIElements(Rect position, SerializedProperty property, GUIContent label)
    {
        if (!sourceRef.isExpanded)
            return;

        //draw position stuff
            //offset position.y by field size
            position.y += position.height;
        DrawConstraintsField(position, "Freeze Position", freezePositionX, freezePositionY, freezePositionZ);

        //draw rotation stuff
        //offset position.y by field size
        position.y += position.height;
        DrawConstraintsField(position, "Freeze Rotation", freezeRotationX, freezeRotationY, freezeRotationZ);

        //Do constraints
        constraints.intValue = (int)RigidbodyConstraints.None;

        //position values
        if (freezePositionX.boolValue)
            constraints.intValue = constraints.intValue | (int)RigidbodyConstraints.FreezePositionX;
        if (freezePositionY.boolValue)
            constraints.intValue = constraints.intValue | (int)RigidbodyConstraints.FreezePositionY;
        if (freezePositionZ.boolValue)
            constraints.intValue = constraints.intValue | (int)RigidbodyConstraints.FreezePositionZ;

        //rotation Values
        if (freezeRotationX.boolValue)
            constraints.intValue = constraints.intValue | (int)RigidbodyConstraints.FreezeRotationX;
        if (freezeRotationY.boolValue)
            constraints.intValue = constraints.intValue | (int)RigidbodyConstraints.FreezeRotationY;
        if (freezeRotationZ.boolValue)
            constraints.intValue = constraints.intValue | (int)RigidbodyConstraints.FreezeRotationZ;
    }

    void DrawConstraintsField(Rect position, string label, SerializedProperty xProp, SerializedProperty yProp, SerializedProperty zProp)
    {
        EditorGUI.LabelField(position, label);

        // Calculate rects
        Rect xRect = new Rect(position.x + 110, position.y, 30, position.height);
        Rect xRectLabel = new Rect(position.x + 125, position.y, 30, position.height);
        Rect yRect = new Rect(position.x + 140, position.y, 50, position.height);
        Rect yRectLabel = new Rect(position.x + 155, position.y, 50, position.height);
        Rect zRect = new Rect(position.x + 170, position.y, position.width - 90, position.height);
        Rect zRectLabel = new Rect(position.x + 185, position.y, position.width - 90, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(xRect, xProp, GUIContent.none);
        EditorGUI.LabelField(xRectLabel, "X");
        EditorGUI.PropertyField(yRect, yProp, GUIContent.none);
        EditorGUI.LabelField(yRectLabel, "Y");
        EditorGUI.PropertyField(zRect, zProp, GUIContent.none);
        EditorGUI.LabelField(zRectLabel, "Z");
    }

}