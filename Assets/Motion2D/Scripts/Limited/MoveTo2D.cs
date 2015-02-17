///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveTo2D.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   �w��ʒu�Ɉړ����郂�[�V�����B                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �w��ʒu�Ɉړ����郂�[�V�����B
/// </summary>
public class MoveTo2D : LimitedMotion2D {
    /// <summary>
    /// �ړ���̈ʒu
    /// </summary>
    public Vector2 to;

    /// <summary>
    /// �������[�V�����̍X�V����
    /// </summary>
    /// <param name="progress">�i����</param>
    protected override void OnLimitedUpdate(float progress) {
        position = (1 - progress) * initPosition + progress * to;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(to.x))
            .Concat(BitConverter.GetBytes(to.y)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y</returns>
    public override int Deserialize(byte[] bytes) {
        var offset = base.Deserialize(bytes);

        to.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        to.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        to = UnityEditor.EditorGUILayout.Vector2Field("To", to);
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    protected override void DrawGizmos() {
        DrawLine(initPosition, to);
        var dir = to - initPosition;
        DrawArrowCap(to, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
#endif
}
