using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Web;
using System;

[CustomEditor(typeof(ItemData))]
public class ItemUseableDataEditor : ItemDataEditor
{
    protected new ItemFiniteData Source { get { return (ItemFiniteData)source; } }

    //ui syncs
    protected SerializedProperty itemSyncType;
    protected SerializedProperty layoutMaster;
    protected SerializedProperty reloadSync;
    protected SerializedProperty ammoClipAmountSync;
    protected SerializedProperty ammoIndSync;

    protected SerializedProperty ammoType;
    protected SerializedProperty preloadAmmo;
    protected SerializedProperty ammoDatas;
    protected SerializedProperty delay;
    protected SerializedProperty recoilTime;
    protected SerializedProperty instantFirstUse;
    protected SerializedProperty reloadIfEmpty;
    protected SerializedProperty reloadTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemFinite>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();

        //ui sync
        itemSyncType = sourceRef.FindProperty("itemSyncType");
        layoutMaster = sourceRef.FindProperty("layoutMaster");
        reloadSync = sourceRef.FindProperty("reloadSync");
        ammoClipAmountSync = sourceRef.FindProperty("ammoClipAmountSync");
        ammoIndSync = sourceRef.FindProperty("ammoIndSync");

        ammoType = sourceRef.FindProperty("ammoType");
        preloadAmmo = sourceRef.FindProperty("preloadAmmo");
        ammoDatas = sourceRef.FindProperty("ammoDatas");
        delay = sourceRef.FindProperty("delay");
        recoilTime = sourceRef.FindProperty("recoilTime");
        instantFirstUse = sourceRef.FindProperty("instantFirstUse");
        reloadIfEmpty = sourceRef.FindProperty("reloadIfEmpty");
        reloadTime = sourceRef.FindProperty("reloadTime");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        DisplayUsageProperties();
    }

    protected override void DisplayUIProperties()
    {
        base.DisplayUIProperties();
        if (syncValuesToUI.boolValue)
        {
            EditorExtensions.LabelFieldCustom("Item UI Syncs", FontStyle.Bold);
            EditorGUILayout.PropertyField(itemSyncType);
            EditorGUILayout.PropertyField(layoutMaster);
            if (layoutMaster.objectReferenceValue)
            {
                ammoClipAmountSync.LayoutSyncSingleField(layoutMaster, itemSyncType);
                ammoIndSync.LayoutSyncSingleField(layoutMaster, itemSyncType);
                reloadSync.LayoutSyncSingleField(layoutMaster, itemSyncType);
            }
            
        }
            
    }

    protected virtual void DisplayUsageProperties()
    {
        EditorGUILayout.LabelField("Usage Properties", boldStyle);
        ammoType.IndexStringPropertyField(entityNames);
        EditorGUILayout.PropertyField(preloadAmmo);
        if (preloadAmmo.boolValue)
            EditorGUILayout.PropertyField(ammoDatas, true);
        EditorGUILayout.PropertyField(delay);
        EditorGUILayout.PropertyField(recoilTime);
        EditorGUILayout.PropertyField(reloadIfEmpty);
        if (reloadIfEmpty.boolValue)
            EditorGUILayout.PropertyField(reloadTime);

    }

}
