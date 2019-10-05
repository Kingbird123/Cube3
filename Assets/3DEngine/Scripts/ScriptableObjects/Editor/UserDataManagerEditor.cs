using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(UserDataManager))]
public class UserDataManagerEditor : Editor
{

    protected SerializedObject sourceRef;
    protected UserDataManager source;

    protected SerializedProperty maxUsers;
    protected SerializedProperty maxItems;
    protected SerializedProperty setDefaultItems;
    protected SerializedProperty itemManager;
    protected SerializedProperty items;
    protected SerializedProperty userContainer;
    protected SerializedProperty users;


    private void OnEnable()
    {
        source = (UserDataManager)target;
        sourceRef = serializedObject;

        source.InitializeData();

        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();
        sourceRef.ApplyModifiedProperties();
    }

    private void GetProperties()
    {
        maxUsers = sourceRef.FindProperty("maxUsers");
        maxItems = sourceRef.FindProperty("maxItems");
        setDefaultItems = sourceRef.FindProperty("setDefaultItems");
        itemManager = sourceRef.FindProperty("itemManager");
        items = sourceRef.FindProperty("items");
        userContainer = sourceRef.FindProperty("userContainer");
        users = userContainer.FindPropertyRelative("users");
    }

    private void SetProperties()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(maxUsers);
        maxItems.IntFieldClamp(0, int.MaxValue);
        EditorGUILayout.PropertyField(setDefaultItems);
        if (setDefaultItems.boolValue)
        {
            EditorGUILayout.PropertyField(itemManager);
            var manager = itemManager.GetRootValue<ItemDataManager>();
            if (manager != null)
            {
                var itemArray = items.GetRootValue<ItemProperty[]>();
                for (int i = 0; i < itemArray.Length; i++)
                {
                    itemArray[i].itemNames = manager.GetItemNames();
                }
                EditorGUILayout.PropertyField(items, true);
            }
            else
                EditorExtensions.LabelFieldCustom("Need " + itemManager.displayName + " to continue.", FontStyle.Bold, Color.red);
            items.arraySize = maxItems.intValue;
        }
            
        DisplayNewUserFields();
        DisplayAllUsers();

    }

    private void DisplayNewUserFields()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("New User:");
        source.UserNameInput = EditorGUILayout.TextField(source.UserNameInput);
        if (GUILayout.Button("Create User"))
        {
            source.CreateUser(source.Users.Count);
            sourceRef.Update();
        }
        EditorGUILayout.Space();
    }

    void DisplayAllUsers()
    {
        for (int i = 0; i < source.Users.Count; i++)
        {
            if (i == 0)
                EditorGUILayout.LabelField("-------------------------");
            var user = users.GetArrayElementAtIndex(i);
            var playerName = user.FindPropertyRelative("playerName");
            //var playerSkinInd = user.FindPropertyRelative("playerSkinInd");
            //var userId = user.FindPropertyRelative("userId");
            //var saveSlotId = user.FindPropertyRelative("saveSlotId");
            var maxHealth = user.FindPropertyRelative("maxHealth");
            var lives = user.FindPropertyRelative("lives");
            var points = user.FindPropertyRelative("points");
            //var levelUnlocked = user.FindPropertyRelative("levelUnlocked");
            var levelName = user.FindPropertyRelative("levelName");
            //var curCheckPoint = user.FindPropertyRelative("curCheckPoint");
            var inventoryItems = user.FindPropertyRelative("inventoryItems");

            EditorGUILayout.LabelField("User Name: ", playerName.stringValue);
            EditorGUILayout.LabelField("Max Health: ", maxHealth.intValue.ToString());
            EditorGUILayout.LabelField("Lives: ", lives.intValue.ToString());
            EditorGUILayout.LabelField("Points: ", points.intValue.ToString());
            EditorGUILayout.LabelField("Level Unlocked: ", levelName.stringValue);
            EditorGUILayout.LabelField("Items: ");
            for (int ind = 0; ind < inventoryItems.arraySize; ind++)
            {
                var item = inventoryItems.GetArrayElementAtIndex(ind);
                var itemName_s = item.FindPropertyRelative("itemName");
                var itemName = itemName_s.stringValue;
                EditorGUILayout.LabelField("Item " + ind + ":",  itemName);
            }
            

            if (GUILayout.Button("Delete User"))
            {
                source.RemoveUser(i);
                sourceRef.Update();
            }
            EditorGUILayout.LabelField("-------------------------");
        }
    }

}
