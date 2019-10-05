using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CanEditMultipleObjects]
[CustomEditor(typeof(PlayerAnimations))]
public class PlayerAnimationsEditor : UnitAnimationsEditor
{
    private SerializedProperty animInputHorizontal;
    private SerializedProperty animInputVertical;
    private SerializedProperty animDoubleJump;
    private SerializedProperty animCrouch;
    private SerializedProperty animClimbing;
    private SerializedProperty animRunning;
    private SerializedProperty animDashing;
    private SerializedProperty animRolling;

    public override void GetProperties()
    {
        base.GetProperties();
        animInputHorizontal = sourceRef.FindProperty("animInputHorizontal");
        animInputVertical = sourceRef.FindProperty("animInputVertical");
        animDoubleJump = sourceRef.FindProperty("animDoubleJump");
        animCrouch = sourceRef.FindProperty("animCrouch");
        animRunning = sourceRef.FindProperty("animRunning");
        animClimbing = sourceRef.FindProperty("animClimbing");
        animDashing = sourceRef.FindProperty("animDashing");
        animRolling = sourceRef.FindProperty("animRolling");
    }

    public override void DisplaySyncedParameters()
    {
        base.DisplaySyncedParameters();
        animInputHorizontal.IndexStringField(parameters);
        animInputVertical.IndexStringField(parameters);
        animCrouch.IndexStringField(parameters);
        animClimbing.IndexStringField(parameters);
        animRunning.IndexStringField(parameters);
        animDashing.IndexStringField(parameters);
        animRolling.IndexStringField(parameters);
    }

    public override void DisplayMovementParameters()
    {
        base.DisplayMovementParameters();
        IndexStringField(animDoubleJump);
    }


}
