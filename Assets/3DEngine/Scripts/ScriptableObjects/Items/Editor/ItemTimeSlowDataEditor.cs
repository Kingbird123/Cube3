using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemTimeSlowData))]
public class ItemTimeSlowDataEditor : ItemAimableDataEditor
{
    protected new ItemTimeSlowData Source { get { return (ItemTimeSlowData)source; } }
    public SerializedProperty slowTimeScale;
    public SerializedProperty physicsTimeScale;
    public SerializedProperty crossfadeTime;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetLinkedType<ItemTimeSlow>();
    }

    protected override void GetProperties()
    {
        base.GetProperties();
        slowTimeScale = sourceRef.FindProperty("slowTimeScale");
        physicsTimeScale = sourceRef.FindProperty("physicsTimeScale");
        crossfadeTime = sourceRef.FindProperty("crossfadeTime");
    }

    protected override void SetProperties()
    {
        base.SetProperties();
        EditorGUILayout.LabelField("Time Slow Properties", boldStyle);
        EditorGUILayout.PropertyField(slowTimeScale);
        EditorGUILayout.PropertyField(physicsTimeScale);
        EditorGUILayout.PropertyField(crossfadeTime);

    }

}
