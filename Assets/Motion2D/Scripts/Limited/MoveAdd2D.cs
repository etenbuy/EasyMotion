///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveAdd2D.cs                                                                     //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   ���݈ʒu����w�肵���ړ��ʂ����ړ����郂�[�V�����B                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// ���݈ʒu����w�肵���ړ��ʂ����ړ����郂�[�V�����B
/// </summary>
public class MoveAdd2D : LimitedMotion2D {
    /// <summary>
    /// �ړ���̈ʒu
    /// </summary>
    private Vector2 move;

    /// <summary>
    /// �������[�V�����̍X�V����
    /// </summary>
    /// <param name="progress">�i����</param>
    protected override void OnLimitedUpdate(float progress) {
        position = initPosition + progress * move;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(move.x))
            .Concat(BitConverter.GetBytes(move.y)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        move.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        move.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        move = UnityEditor.EditorGUILayout.Vector2Field("Move", move);
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    /// <param name="from">���݈ʒu</param>
    /// <returns>�ړ���̈ʒu</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        var to = from + move;
        DrawLine(from, to);
        DrawArrowCap(to, Mathf.Atan2(move.y, move.x) * Mathf.Rad2Deg);

        return to;
    }

    /// <summary>
    /// �����擾
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <returns>�ݒ肳�ꂽ����</returns>
    public override float GetSpeed(Vector2 from) {
        var curSpeed = 0f;
        if ( duration != 0 ) {
            curSpeed = move.magnitude / duration;
        }
        return curSpeed;
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="speed">����</param>
    public override void SetSpeed(Vector2 from, float speed) {
        if ( speed == 0 ) {
            duration = 0;
        } else {
            duration = move.magnitude / speed;
        }
    }
#endif
}
