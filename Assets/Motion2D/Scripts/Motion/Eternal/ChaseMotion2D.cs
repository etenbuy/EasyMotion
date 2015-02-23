///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   ChaseMotion2D.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.23                                                                       //
//  Desc    :   �ڕW���ɒǔ����铮��B                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �ڕW���ɒǔ����铮��B
/// </summary>
public class ChaseMotion2D : EternalMotion2D {
    /// <summary>
    /// �����̌���
    /// </summary>
    private float fromAngle;

    /// <summary>
    /// �O�i���鑬��
    /// </summary>
    private float speed;

    /// <summary>
    /// ���񑬓x
    /// </summary>
    private float rotateSpeed;

    /// <summary>
    /// �ڕW���I�u�W�F�N�g�̎��
    /// </summary>
    private TargetBase2D.TargetType targetType;

    /// <summary>
    /// �ǔ��Ώۂ̖ڕW��
    /// </summary>
    private TargetBase2D target;

    /// <summary>
    /// ���݂̌���
    /// </summary>
    private float curAngle;

    /// <summary>
    /// �i�v���[�V�����̏���������
    /// </summary>
    protected override void OnEternalStart() {
        curAngle = fromAngle;
    }

    /// <summary>
    /// �i�v���[�V�����̍X�V����
    /// </summary>
    /// <param name="time">�o�ߎ���</param>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnEternalUpdate(float time) {
        // �ڕW����Transform�擾
        var targetTrans = target.transform;
        if ( targetTrans == null ) {
            // �ڕW�������݂��Ȃ�
            return true;
        }

        // �ڕW���ւ̌����v�Z
        Vector2 targetDir = targetTrans.localPosition - transform.localPosition;
        var toAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

        // ��]�ʌv�Z
        float diffAngle = RotationBase2D.AdjustAngleRange(toAngle - curAngle, -180);
        var rotAngle = speed * Time.deltaTime;

        // �����X�V
        if ( rotAngle > Mathf.Abs(diffAngle) ) {
            // �ڕW�p�x�𒴂��ĉ�]����ꍇ�͖ڕW�p�x�Ɉ�v
            curAngle = toAngle;
        } else {
            // �ڕW�p�x�𒴂��Ȃ��ꍇ�͂��̕����ɉ�]
            curAngle += diffAngle < 0 ? -rotAngle : rotAngle;
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
            .Concat(BitConverter.GetBytes(fromAngle))
            .Concat(BitConverter.GetBytes(speed))
            .Concat(BitConverter.GetBytes(rotateSpeed))
            .Concat(BitConverter.GetBytes((int)targetType))
            .Concat(target.Serialize()).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        fromAngle = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        speed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        rotateSpeed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        targetType = (TargetBase2D.TargetType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        target = TargetBase2D.GetDeserialized(targetType, bytes, offset, out offset);

        return offset;
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    public override float currentDirection {
        get {
            return curAngle;
        }
    }
}
