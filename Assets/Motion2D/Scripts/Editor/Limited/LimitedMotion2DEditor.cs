///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LimitedMotion2DEditor.cs                                                         //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   LimitedMotion2D�N���X�̃G�f�B�^�g���B                                            //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using System.Collections;

/// <summary>
/// LimitedMotion2D�N���X�̃G�f�B�^�g���B
/// </summary>
public class LimitedMotion2DEditor : Editor {
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    /// <param name="motion">���[�V�����I�u�W�F�N�g</param>
    public static void DrawGUI(LimitedMotion2D motion) {
        EditorGUILayout.FloatField("Duration", motion.duration);
    }
}
