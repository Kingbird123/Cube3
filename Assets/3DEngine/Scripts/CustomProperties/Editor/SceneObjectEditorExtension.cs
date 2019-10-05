using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SceneObjectEditorExtension
{

    private static SerializedProperty sceneObjectType;
    private static SerializedProperty overrideGameObject;
    private static SerializedProperty closestTag;
    private static SerializedProperty nameToFind;

    public static void SceneObjectField(this SerializedProperty _sceneObjectProperty, string _fieldName)
    {
        //get property values
        sceneObjectType = _sceneObjectProperty.FindPropertyRelative("sceneObjectType");
        var label = new GUIContent { text = _fieldName };
        EditorGUILayout.PropertyField(sceneObjectType, label);
        SceneObjectField(_sceneObjectProperty);

    }

    public static void SceneObjectField(this SerializedProperty _sceneObjectProperty)
    {
        //get property values
        sceneObjectType = _sceneObjectProperty.FindPropertyRelative("sceneObjectType");
        overrideGameObject = _sceneObjectProperty.FindPropertyRelative("overrideGameObject");
        closestTag = _sceneObjectProperty.FindPropertyRelative("closestTag");
        nameToFind = _sceneObjectProperty.FindPropertyRelative("nameToFind");

        int ind = sceneObjectType.enumValueIndex;
        if (ind == (int)SceneObjectProperty.SceneObjectType.ClosestByTag)
        {
            closestTag.stringValue = EditorGUILayout.TagField("Closest Tag", closestTag.stringValue);
        }
        else if (ind == (int)SceneObjectProperty.SceneObjectType.FindByName)
        {
            EditorGUILayout.PropertyField(nameToFind);
        }
        else if (ind == (int)SceneObjectProperty.SceneObjectType.Override)
        {
            var obj = _sceneObjectProperty.serializedObject.targetObject;
            if (!obj.GetType().IsClassOrSubClass(typeof(ScriptableObject)))
            {
                EditorGUILayout.PropertyField(overrideGameObject);
            }
            else
            {
                Debug.Log("You can not assign a sceneobject or prefab inside this property if it is on a ScriptableObject!");
                sceneObjectType.enumValueIndex = (int)SceneObjectProperty.SceneObjectType.Receiver;
            }
        }

    }

}