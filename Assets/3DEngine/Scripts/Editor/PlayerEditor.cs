using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Player))]
public class PlayerEditor : UnitEditor
{

    //health
    private SerializedProperty setData;
    private SerializedProperty userMode;
    private SerializedProperty userDataManager;
    private SerializedProperty user;

    protected override void GetProperties()
    {
        base.GetProperties();
        setData = sourceRef.FindProperty("setData");
        userMode = sourceRef.FindProperty("userMode");
        userDataManager = sourceRef.FindProperty("userDataManager");
        user = sourceRef.FindProperty("user");
    }

    protected override void DisplayDataProperties<T>()
    {
        EditorGUILayout.PropertyField(setData);
        if (setData.boolValue)
        {
            base.DisplayDataProperties<UnitData>();
        }
        else
        {
            EditorExtensions.LabelFieldCustom("User Options", FontStyle.Bold);
            EditorGUILayout.PropertyField(userMode);
            if (userMode.enumValueIndex == (int)Player.UserMode.Override)
            {
                EditorGUILayout.PropertyField(userDataManager);

                var man = userDataManager.GetRootValue<UserDataManager>();
                if (man != null)
                {
                    user.IndexStringPropertyField(man.GetUserNames());
                }
                else
                    EditorExtensions.LabelFieldCustom("Need User Data Manager!", FontStyle.Bold, Color.red);
            }

        }

    }

}
