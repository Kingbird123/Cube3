using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneObjectProperty))]
public class SceneObjectPropertyDrawer : PropertyDrawer
{
    private SerializedProperty sourceRef;

    private SerializedProperty sceneObjectType;
    private SerializedProperty overrideGameObject;
    private SerializedProperty closestTag;
    private SerializedProperty nameToFind;

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

    public virtual void GetProperties()
    {
        //get property values
        sceneObjectType = sourceRef.FindPropertyRelative("sceneObjectType");
        overrideGameObject = sourceRef.FindPropertyRelative("overrideGameObject");
        closestTag = sourceRef.FindPropertyRelative("closestTag");
        nameToFind = sourceRef.FindPropertyRelative("nameToFind");
    }

    void CalculateFieldAmount()
    {
        fieldAmount = 1;
        if (sceneObjectType.enumValueIndex == (int)SceneObjectProperty.SceneObjectType.ClosestByTag || 
            sceneObjectType.enumValueIndex == (int)SceneObjectProperty.SceneObjectType.Override ||
            sceneObjectType.enumValueIndex == (int)SceneObjectProperty.SceneObjectType.FindByName)
            fieldAmount++;
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

        // Get the start indent level
        var indent = EditorGUI.indentLevel;
        // Set indent amount
        //EditorGUI.indentLevel = indent + 1;

        DisplayGUIElements(position, property, label);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public virtual void DisplayGUIElements(Rect position, SerializedProperty property, GUIContent label)
    {
            //offset position.y by field size
            //position.y += position.height;
        //first field
        sceneObjectType.enumValueIndex = EditorGUI.Popup(position,"Scene Object Type", sceneObjectType.enumValueIndex, sceneObjectType.enumDisplayNames);

        int ind = sceneObjectType.enumValueIndex;
        if (ind == (int)SceneObjectProperty.SceneObjectType.ClosestByTag)
        {
            position.y += position.height;
            closestTag.stringValue = EditorGUI.TagField(position, "Closest Tag", closestTag.stringValue);
        }
        else if (ind == (int)SceneObjectProperty.SceneObjectType.FindByName)
        {
            position.y += position.height;
            EditorGUI.PropertyField(position, nameToFind);
        }
        else if (ind == (int)SceneObjectProperty.SceneObjectType.Override)
        {
            var obj = property.serializedObject.targetObject;
            if (!obj.GetType().IsClassOrSubClass(typeof(ScriptableObject)))
            {
                position.y += position.height;
                EditorGUI.PropertyField(position, overrideGameObject);
            }
            else
            {
                Debug.Log("You can not assign a sceneobject or prefab inside this property if it is on a ScriptableObject!");
                sceneObjectType.enumValueIndex = (int)SceneObjectProperty.SceneObjectType.Receiver;
            }
        }

    }

}