///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   RotateForward2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   �O���ɉ�]���铮��B                                                             //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �O���ɉ�]���铮��B
/// </summary>
public class RotateForward2D : RotationBase2D {
    /// <summary>
    /// ��]�p���x
    /// </summary>
    private float speed;

    /// <summary>
    /// �����̂��炵�p�x
    /// </summary>
    private float angleOffset;

    /// <summary>
    /// ��]����̏���������
    /// </summary>
    /// <returns>true:��]����p�� / false:�ȍ~�̉�]������p�����Ȃ�</returns>
    protected override bool OnStart() {
        return true;
    }

    /// <summary>
    /// ��]����̍X�V����
    /// </summary>
    /// <returns>true:��]����p�� / false:�ȍ~�̉�]������p�����Ȃ�</returns>
    protected override bool OnUpdate() {
        // ���݂̌����擾
        var toAngle = motion.direction;

        if ( toAngle == MotionBase2D.NO_DIRECTION ) {
            // ���������݂��Ȃ���Ή������Ȃ�
            return true;
        }

        // ���炵�p�x���Z
        toAngle += angleOffset;

        // ��]�ʌv�Z
        float diffAngle = AdjustAngleRange(toAngle - angle, -180);
        var rotAngle = speed * Time.deltaTime;

        // �����X�V
        if ( rotAngle > Mathf.Abs(diffAngle) ) {
            // �ڕW�p�x�𒴂��ĉ�]����ꍇ�͖ڕW�p�x�Ɉ�v
            angle = toAngle;
        } else {
            // �ڕW�p�x�𒴂��Ȃ��ꍇ�͂��̕����ɉ�]
            angle += diffAngle < 0 ? -rotAngle : rotAngle;
        }

        return true;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(speed))
            .Concat(BitConverter.GetBytes(angleOffset)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݉�]����f�[�^</param>
    /// <param name="offset">��]����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        speed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        angleOffset = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();

        speed = UnityEditor.EditorGUILayout.FloatField("Speed", speed);
        angleOffset = UnityEditor.EditorGUILayout.FloatField("Angle Offset", angleOffset);
    }
#endif
}
