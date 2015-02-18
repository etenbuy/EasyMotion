///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveLiner2D.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   �����Ɉړ��������郂�[�V�����B                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �����Ɉړ��������郂�[�V�����B
/// </summary>
public class MoveLiner2D : EternalMotion2D {
    /// <summary>
    /// �ړ����x
    /// </summary>
    private Vector2 velocity;

    /// <summary>
    /// �i�v���[�V�����̍X�V����
    /// </summary>
    /// <param name="time">�o�ߎ���</param>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnEternalUpdate(float time) {
        position = initPosition + velocity * time;
        return true;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(velocity.x))
            .Concat(BitConverter.GetBytes(velocity.y)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y</returns>
    public override int Deserialize(byte[] bytes) {
        var offset = base.Deserialize(bytes);

        velocity.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        velocity.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        velocity = UnityEditor.EditorGUILayout.Vector2Field("Velocity", velocity);
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    protected override void DrawGizmos() {
        var to = initPosition + velocity;
        DrawLine(initPosition, to);
        DrawArrowCap(to, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
    }
#endif
}
