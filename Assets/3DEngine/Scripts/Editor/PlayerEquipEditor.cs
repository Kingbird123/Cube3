using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(PlayerEquip))]
public class PlayerEquipEditor : UnitEquipEditor
{

    private SerializedProperty loadInventory;
    private SerializedProperty userDataManager;
    private SerializedProperty user;
    private SerializedProperty quickMenuButtons;
    private SerializedProperty interactButton;
    private SerializedProperty enableToggleSwitch;
    private SerializedProperty toggleForwardsButton;
    private SerializedProperty toggleBackwardsButton;
    private SerializedProperty equipButton;
    private SerializedProperty dropButton;


    public override void GetProperties()
    {
        base.GetProperties();
        loadInventory = sourceRef.FindProperty("loadInventory");
        userDataManager = sourceRef.FindProperty("userDataManager");
        user = sourceRef.FindProperty("user");
        quickMenuButtons = sourceRef.FindProperty("quickMenuButtons");
        interactButton = sourceRef.FindProperty("interactButton");
        enableToggleSwitch = sourceRef.FindProperty("enableToggleSwitch");
        toggleForwardsButton = sourceRef.FindProperty("toggleForwardsButton");
        toggleBackwardsButton = sourceRef.FindProperty("toggleBackwardsButton");
        equipButton = sourceRef.FindProperty("equipButton");
        dropButton = sourceRef.FindProperty("dropButton");
    }

    public override void SetProperties()
    {
        base.SetProperties();

        if (loadInventory.enumValueIndex == 1)//override items
            quickMenuButtons.arraySize = itemsToAdd.arraySize;

        quickMenuButtons.ArrayFieldCustom(false, true, "Select Item");

        EditorGUILayout.PropertyField(enableToggleSwitch);
        if (enableToggleSwitch.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(toggleForwardsButton);
            EditorGUILayout.PropertyField(toggleBackwardsButton);
            EditorGUI.indentLevel--;
        }


        EditorGUILayout.PropertyField(interactButton);
        if (!autoEquipItems.boolValue)
            EditorGUILayout.PropertyField(equipButton);
        EditorGUILayout.PropertyField(dropButton);


    }

    protected override void DisplayInputProperties()
    {
        //override input property to user
        inputType.enumValueIndex = 1;
    }

    protected override void DisplayItemDatasProperties()
    {
        EditorGUILayout.PropertyField(loadInventory);
        if (loadInventory.enumValueIndex == 1)//override items
            base.DisplayItemDatasProperties();
        else
        {
            //load from userdata
            EditorGUILayout.PropertyField(userDataManager);
            var userData = userDataManager.GetRootValue<UserDataManager>();
            if (userData != null)
            {
                EditorGUILayout.PropertyField(user);
                var userSource = user.GetRootValue<IndexStringProperty>();
                if (userSource != null)
                {
                    userSource.stringValues = userData.GetUserNames();
                    var user = userData.GetUser(userSource.indexValue);
                    if (user != null)
                    {
                        quickMenuButtons.arraySize = user.inventoryItems.Length;
                    }

                }

            }
            else
                EditorExtensions.LabelFieldCustom("Need " + userDataManager.displayName + " to get user information!", FontStyle.Bold, Color.red);

        }
    }



}
