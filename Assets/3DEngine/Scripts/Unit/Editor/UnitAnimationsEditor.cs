using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CanEditMultipleObjects]
[CustomEditor(typeof(UnitAnimations))]
public class UnitAnimationsEditor : Editor
{
    protected UnitAnimations source;
    protected SerializedObject sourceRef;
    //skins
    protected SerializedProperty anim;
    protected SerializedProperty animController;


    protected SerializedProperty forceIdle;
    protected SerializedProperty syncType;
    protected SerializedProperty crossfadeTime;

    //state fields
    protected SerializedProperty animIdle;
    protected SerializedProperty animHover;

    //param fields
    protected SerializedProperty animDirectionX;
    protected SerializedProperty animDirectionY;
    protected SerializedProperty animDirectionZ;
    protected SerializedProperty animGrounded;
    protected SerializedProperty animOnPlatform;
    protected SerializedProperty animVelocitySpeed;

    //movement fields
    protected SerializedProperty animJump;
    protected SerializedProperty animRun;

    //health
    protected SerializedProperty animHurts;
    protected SerializedProperty animDeaths;
    protected SerializedProperty animStunned;

    //attack
    protected SerializedProperty animAttacksMelee;
    protected SerializedProperty animAttacksRanged;

    protected AnimatorController animCont;
    protected AnimatorController lastAnim;
    protected GUIStyle boldStyle;
    protected AnimatorState[] states;
    protected string[] parameters;
    protected string[] stateNames;

    private int pop;

    public virtual void OnEnable()
    {
        sourceRef = serializedObject;
        source = target as UnitAnimations;
        SetGUIStyle();
        GetProperties();
    }

    public override void OnInspectorGUI()
    {
        CheckAnimField();
        SetProperties();

        sourceRef.ApplyModifiedProperties();
    }

    void SetGUIStyle()
    {
        boldStyle = new GUIStyle
        {
            fontStyle = FontStyle.Bold,
        };
    }

    public virtual void GetProperties()
    {
        anim = sourceRef.FindProperty("anim");
        animController = sourceRef.FindProperty("animController");
        forceIdle = sourceRef.FindProperty("forceIdle");
        syncType = sourceRef.FindProperty("syncType");
        crossfadeTime = sourceRef.FindProperty("crossfadeTime");
        animIdle = sourceRef.FindProperty("animIdle");
        animHover = sourceRef.FindProperty("animHover");

        animDirectionX = sourceRef.FindProperty("animDirectionX");
        animDirectionY = sourceRef.FindProperty("animDirectionY");
        animDirectionZ = sourceRef.FindProperty("animDirectionZ");

        animGrounded = sourceRef.FindProperty("animGrounded");
        animOnPlatform = sourceRef.FindProperty("animOnPlatform");
        animVelocitySpeed = sourceRef.FindProperty("animVelocitySpeed");

        animJump = sourceRef.FindProperty("animJump");
        animRun = sourceRef.FindProperty("animRun");

        animHurts = sourceRef.FindProperty("animHurts");
        animDeaths = sourceRef.FindProperty("animDeaths");
        animStunned = sourceRef.FindProperty("animStunned");

        animAttacksMelee = sourceRef.FindProperty("animAttacksMelee");
        animAttacksRanged = sourceRef.FindProperty("animAttacksRanged");
    }

    public virtual void SetProperties()
    {
        EditorGUILayout.Space();
        //skins
        EditorGUILayout.PropertyField(anim);
        EditorGUILayout.PropertyField(animController);

        if (animCont)
        {
            EditorGUILayout.PropertyField(forceIdle);
            animIdle.IndexStringField(stateNames, animCont);
            if (!forceIdle.boolValue)
            {
                EditorGUILayout.PropertyField(syncType);
                if (syncType.enumValueIndex == 1)
                    EditorGUILayout.PropertyField(crossfadeTime);
                DisplaySyncedParameters();
                DisplayHealthParameters();
                DisplayMovementParameters();
                DisplayAttackParameters();
            }
            else
            {
                animGrounded.IndexStringField(parameters);
                syncType.enumValueIndex = 1;
            }
                  
        }
        else
            EditorGUILayout.LabelField("[Need " + anim.displayName + " or " + animController.displayName + " to begin.]", boldStyle);
    }

    void CheckAnimField()
    {
        GetAnimatorController();

        if (animCont != lastAnim)
        {
            GetAnimParamNames();
            GetAnimStateNames();
            lastAnim = animCont;
        }
    }

    public virtual void DisplaySyncedParameters()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Synced Parameters", boldStyle);
        animDirectionX.IndexStringField(parameters);
        animDirectionY.IndexStringField(parameters);
        animDirectionZ.IndexStringField(parameters);
        animGrounded.IndexStringField(parameters);
        animOnPlatform.IndexStringField(parameters);
        animVelocitySpeed.IndexStringField(parameters);
    }

    public virtual void DisplayHealthParameters()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Health Animations", boldStyle);
        CreateDropDownList(animHurts);
        CreateDropDownList(animDeaths);
        IndexStringField(animStunned);
    }

    public virtual void DisplayMovementParameters()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Movement Animations", boldStyle);
        IndexStringField(animRun);
        IndexStringField(animJump);
    }

    public virtual void DisplayAttackParameters()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Attack Animations", boldStyle);
        CreateDropDownList(animAttacksMelee);
        CreateDropDownList(animAttacksRanged);
    }

    protected void IndexStringField(SerializedProperty _property)
    {
        if (syncType.enumValueIndex == 0)
            _property.IndexStringField(parameters);
        else if (syncType.enumValueIndex == 1)
            _property.IndexStringField(stateNames, animCont);
    }

    protected void CreateDropDownList(SerializedProperty _arrayProperty)
    {
        _arrayProperty.isExpanded = EditorGUILayout.Foldout(_arrayProperty.isExpanded, _arrayProperty.displayName, true);
        if (_arrayProperty.isExpanded)
        {
            EditorGUI.indentLevel++;
            _arrayProperty.arraySize = EditorGUILayout.DelayedIntField("Size", _arrayProperty.arraySize);
            for (int i = 0; i < _arrayProperty.arraySize; i++)
            {
                var prop = _arrayProperty.GetArrayElementAtIndex(i);
                var displayName = prop.displayName + " " + i;
                if (syncType.enumValueIndex == 0)
                   prop.IndexStringField(parameters);
                else if (syncType.enumValueIndex == 1)
                    prop.IndexStringField(stateNames, animCont);

            }
            EditorGUI.indentLevel--;
        }

    }

    void GetAnimatorController()
    {
        if (anim.objectReferenceValue)
        {
            var animObj = anim.GetRootValue<Animator>();
            animCont = (AnimatorController)animObj.runtimeAnimatorController;
        }
        else if (animController.objectReferenceValue)
        {
            var cont = animController.GetRootValue<RuntimeAnimatorController>();
            animCont = (AnimatorController)cont;
        }
    }

    void GetAnimStateNames()
    {
        if (animCont)
        {
            states = new AnimatorState[animCont.animationClips.Length];
            states = EditorExtensions.GetAnimatorStates(animCont);
            if (states.Length > 0)
            {
                stateNames = new string[states.Length];
                for (int i = 0; i < states.Length; i++)
                {
                    stateNames[i] = states[i].name;
                }
            }

        }
    }

    void GetAnimParamNames()
    {
        if (animCont)
        {
            parameters = new string[animCont.parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                parameters[i] = animCont.parameters[i].name;
            }
        }
    }

}
