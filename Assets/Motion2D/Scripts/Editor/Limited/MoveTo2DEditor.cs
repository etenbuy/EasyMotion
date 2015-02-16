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
    /// <param name="motion">���[�V�����I�u�W�F�N�g</param>
    public static void DrawGUI(Editor editor, MoveTo2D motion) {
        EditorGUILayout.PropertyField(editor.serializedObject.FindProperty("to"));
    }
}
