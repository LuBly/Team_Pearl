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

        // _objectType 필드를 가져옵니다.
        var SkillType = (SkillType)serializedObject.FindProperty("skillType").intValue;
        // _objectType 필드를 Inspector에 노출 시켜줍니다.
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillType"));

        // Inspector에서 ObjectType Enum을 변경하게 되면 해당 타입에 맞게 노출시켜줄 필드를 정의해줍니다.
        switch (SkillType)
        {
            case SkillType.continuousAttack:
                {
                    // CarInfo는 클래스이므로 '_carInfo.name' 형태로 클래스 내부에 접근할 수 있습니다.
                    // 프로퍼티를 가져옴과 동시에 Inspector에 노출시켜 줍니다.
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("continuousAtk.enableTime"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("continuousAtk.attackTime"));
                }
                break;

            case SkillType.quickAttack:
                {
                    
                }
                break;
        }

        // 변경된 프로퍼티를 저장해줍니다.
        serializedObject.ApplyModifiedProperties();
    }
}