using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Skill))]
public class ObjectInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        // SkillType 필드를 가져옵니다.
        var SkillType = (SkillType)serializedObject.FindProperty("skillType").intValue;
        // SkillType 필드를 Inspector에 노출 시켜줍니다.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillType"));

        // Inspector에서 ObjectType Enum을 변경하게 되면 해당 타입에 맞게 노출시켜줄 필드를 정의해줍니다.
        switch (SkillType)
        {
            case SkillType.continuousAttack:
                {
                    // 프로퍼티를 가져옴과 동시에 Inspector에 노출시켜 줍니다.
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("continuousAtk.enableTime"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("continuousAtk.attackTime"));
                }
                break;

            case SkillType.grenadeAttack:
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("grenadeAtk.scanner"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("grenadeAtk.skillRange"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("grenadeAtk.attackPoint"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("grenadeAtk.skillImpact"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("grenadeAtk.skillJoystickMovement"));
                }
                break;
        }

        // 변경된 프로퍼티를 저장해줍니다.
        serializedObject.ApplyModifiedProperties();
    }
}