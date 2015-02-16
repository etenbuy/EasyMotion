///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveTo2DEditor.cs                                                                //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   MoveTo2D�N���X�̃G�f�B�^�g���B                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using System.Collections;

/// <summary>
/// MoveTo2D�N���X�̃G�f�B�^�g���B
/// </summary>
public class MoveTo2DEditor : Editor {
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    /// <param name="editor">�Ăяo�����̃G�f�B�^�X�N���v�g</param>
    /// <param name="motionBase">���[�V�����I�u�W�F�N�g</param>
    public static void DrawGUI(MotionBase2D motionBase) {
        var motion = motionBase as MoveTo2D;
        LimitedMotion2DEditor.DrawGUI(motion);
        motion.to = EditorGUILayout.Vector2Field("To", motion.to);
    }
}
