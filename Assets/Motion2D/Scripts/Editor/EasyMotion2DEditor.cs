///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EasyMotion2DEditor.cs                                                            //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   EasyMotion2D�N���X�̃G�f�B�^�g���B                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;
using System.Collections;

/// <summary>
/// EasyMotion2D�N���X�̃G�f�B�^�g���B
/// </summary>
//[CustomEditor(typeof(EasyMotion2D))]
public class EasyMotion2DEditor : Editor {
    private int currentType;

    /// <summary>
    /// �������B
    /// </summary>
    public void Awake() {
        currentType = serializedObject.FindProperty("type").enumValueIndex;
    }

    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        // ���[�V�����^�I��`��
        var type = serializedObject.FindProperty("type");
        var newType = type.enumValueIndex;
        EditorGUILayout.PropertyField(type);

        //var script = target as EasyMotion2D;

        //if ( newType != currentType ) {
        //    // ���[�V�����^���ύX���ꂽ
        //    script.motion = new MoveTo2D();
            
        //    currentType = newType;
        //}

        //var motion = script.motion;

        //// ���[�V�����̊eGUI�`��
        //MotionBase2DEditor.DrawGUI(motion);

        //if ( motion is LimitedMotion2D ) {
        //    LimitedMotion2DEditor.DrawGUI(motion as LimitedMotion2D);
        //}

        serializedObject.ApplyModifiedProperties();
    }
}
