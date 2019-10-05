using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Engine;
[CustomEditor(typeof(EngineEntityData))]
public class EngineEntityDataEditor : Editor
{

    protected SerializedObject sourceRef;
    protected EngineEntityData source;
    protected EngineEntityData Source { get { return source; } }
    protected GUIStyle boldStyle;

    //icon
    protected SerializedProperty avatarIcon;

    //entity
    protected SerializedProperty entityDataManager;
    protected SerializedProperty entityId;

    //ui
    protected SerializedProperty spawnUI;
    protected SerializedProperty UIToSpawn;
    protected SerializedProperty childOptions;

    protected SerializedProperty syncValuesToUI;
    protected SerializedProperty entityLayoutSyncs;

    //vitals
    protected SerializedProperty engineEventManager;
    protected SerializedProperty engineValueManager;
    protected SerializedProperty engineValueSelections;

    protected string[] entityNames;
    protected virtual void OnEnable()
    {
        source = (EngineEntityData)target;
        sourceRef = serializedObject;
        SetupGUIStyle();
        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        SetProperties();
        sourceRef.ApplyModifiedProperties();
    }

    void SetupGUIStyle()
    {
        boldStyle = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
        };
    }

    protected virtual void GetProperties()
    {
        avatarIcon = sourceRef.FindProperty("avatarIcon");

        //entity
        entityDataManager = sourceRef.FindProperty("entityDataManager");
        entityId = sourceRef.FindProperty("entityId");

        //ui
        spawnUI = sourceRef.FindProperty("spawnUI");
        UIToSpawn = sourceRef.FindProperty("UIToSpawn");
        childOptions = sourceRef.FindProperty("childOptions");
        syncValuesToUI = sourceRef.FindProperty("syncValuesToUI");
        entityLayoutSyncs = sourceRef.FindProperty("entityLayoutSyncs");
        //vitals
        engineEventManager = sourceRef.FindProperty("engineEventManager");
        engineValueManager = sourceRef.FindProperty("engineValueManager");
        engineValueSelections = sourceRef.FindProperty("engineValueSelections");

    }

    protected virtual void SetProperties()
    {
        EditorExtensions.SpritePreviewField(avatarIcon, 80, 80, true);
        DisplayEntityProperties();
        DisplayVitalityProperties();
        DisplayUIProperties();
        
    }

    protected virtual void DisplayEntityProperties()
    {
        EditorGUILayout.LabelField("Entity Properties", boldStyle);
        EditorGUILayout.PropertyField(entityDataManager);
        var entitySource = entityDataManager.GetRootValue<EntityDataManager>();
        if (entitySource)
        {
            entityNames = entitySource.GetEntityNames();
            entityId.intValue = EditorGUILayout.Popup("Entity Id", entityId.intValue, entityNames);
        }
            
    }

    protected virtual void DisplayUIProperties()
    {
        EditorGUILayout.LabelField("UI Properties", boldStyle);
        EditorGUILayout.PropertyField(spawnUI);

        if (spawnUI.boolValue)
        {
            UIToSpawn.PrefabFieldWithComponent(typeof(UIEngineValueEntity));
            EditorGUILayout.PropertyField(childOptions);
        }
        EditorGUILayout.PropertyField(syncValuesToUI);
        if (syncValuesToUI.boolValue)
        {
            entityLayoutSyncs.ArrayFieldButtons("Layout Sync", true, true, true, true, LayoutSyncField);
        }

    }

    void LayoutSyncField(SerializedProperty _property, int _ind)
    {
        var syncType = _property.FindPropertyRelative("syncType");
        var valueManager = _property.FindPropertyRelative("valueManager");
        var layoutMaster = _property.FindPropertyRelative("layoutMaster");
        var syncSelections = _property.FindPropertyRelative("syncSelections");

        if (engineValueManager.objectReferenceValue)
        {
            valueManager.objectReferenceValue = engineValueManager.objectReferenceValue;

            EditorGUILayout.PropertyField(syncType);
            EditorGUILayout.PropertyField(layoutMaster);
            if (layoutMaster.objectReferenceValue)
            {
                LayoutSyncSelectionEditorExtensions.
                    LayoutSyncSelectionArrayField(syncSelections, engineValueSelections, layoutMaster);
            }
        }
        else
            EditorExtensions.LabelFieldCustom("Need Engine Value Manager!", FontStyle.Bold, Color.red);

    }

    protected virtual void DisplayVitalityProperties()
    {
        EditorGUILayout.LabelField("Engine Value Properties", boldStyle);
        EditorGUILayout.PropertyField(engineEventManager);
        EditorGUILayout.PropertyField(engineValueManager);
        EditorGUILayout.LabelField("Engine Values To Use", boldStyle);
        if (engineValueManager.objectReferenceValue)
        {
            engineValueSelections.EngineValueEntityArrayField(engineValueManager, engineEventManager);
        }
            
    }

}
