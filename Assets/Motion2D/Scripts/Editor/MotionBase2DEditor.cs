///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionBase2DEditor.cs                                                            //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   MotionBase2D�N���X�̃G�f�B�^�g���B                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using System.Collections;

/// <summary>
/// MotionBase2D�N���X�̃G�f�B�^�g���B
/// </summary>
public class MotionBase2DEditor : Editor {
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    /// <param name="motion">���[�V�����I�u�W�F�N�g</param>
    public static void DrawGUI(MotionBase2D motion) {
        EditorGUILayout.FloatField("Delay", motion.delay);
    }
}
