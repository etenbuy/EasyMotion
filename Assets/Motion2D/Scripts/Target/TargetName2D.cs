///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   TargetName2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.23                                                                       //
//  Desc    :   ���O�w��̖ڕW�����N���X�B                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// ���O�w��̖ڕW�����N���X�B
/// </summary>
public class TargetName2D : TargetBase2D {
    /// <summary>
    /// �ڕW���̖��O
    /// </summary>
    private string name = string.Empty;

    /// <summary>
    /// �ڕW���̃^�O��
    /// </summary>
    private string tag = string.Empty;

    /// <summary>
    /// �ڕW����Transform�Q��
    /// </summary>
    private Transform targetTrans;

    /// <summary>
    /// �ڕW����Transform
    /// </summary>
    public override Transform transform {
        get {
            if ( targetTrans == null && !string.IsNullOrEmpty(name) ) {
                // ���O�ŖڕW���T��
                var obj = GameObject.Find(name);
                if ( obj != null ) {
                    targetTrans = obj.transform;
                }
            }

            if ( targetTrans == null && !string.IsNullOrEmpty(tag) ) {
                // �^�O���ŖڕW���T��
                var obj = GameObject.FindGameObjectWithTag(tag);
                if ( obj != null ) {
                    targetTrans = obj.transform;
                }
            }

            return targetTrans;
        }
    }

    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        var nameBytes = System.Text.Encoding.ASCII.GetBytes(name);
        var tagBytes = System.Text.Encoding.ASCII.GetBytes(tag);

        return result
            .Concat(BitConverter.GetBytes(nameBytes.Length))
            .Concat(nameBytes)
            .Concat(BitConverter.GetBytes(tagBytes.Length))
            .Concat(tagBytes).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃f�[�^</param>
    /// <param name="offset">�f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        var length = BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        name = System.Text.Encoding.ASCII.GetString(bytes, offset, length);
        offset += length;

        length = BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        tag = System.Text.Encoding.ASCII.GetString(bytes, offset, length);
        offset += length;

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        name = UnityEditor.EditorGUILayout.TextField("Name", name);
        tag = UnityEditor.EditorGUILayout.TagField("Tag", tag);
    }
#endif
}
