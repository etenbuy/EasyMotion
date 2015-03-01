///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   RotateToTarget2D.cs                                                              //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   �ڕW���ɉ�]���铮��B                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �ڕW���ɉ�]���铮��B
/// </summary>
public class RotateToTarget2D : RotationBase2D {
    /// <summary>
    /// �ڕW�����
    /// </summary>
    private TargetName2D target;

    /// <summary>
    /// ��]���鑬��
    /// </summary>
    private float rotateSpeed;

    /// <summary>
    /// �����̂��炵�p�x
    /// </summary>
    private float angleOffset;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public RotateToTarget2D() {
        target = new TargetName2D();
    }

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
        // �ڕW����Transform�擾
        var targetTrans = target.transform;

        if ( targetTrans != null ) {
            // �ڕW���ւ̌����v�Z
            var targetPos = targetTrans.position;
            if ( transform.parent != null ) {
                targetPos = transform.parent.InverseTransformPoint(targetPos);
            }
            var targetDir = (Vector2)targetPos - motion.position;
            var toAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            toAngle += angleOffset;

            // ��]�ʌv�Z
            float diffAngle = RotationBase2D.AdjustAngleRange(toAngle - angle, -180);
            var rotAngle = rotateSpeed * Time.deltaTime;

            // �����X�V
            if ( rotAngle > Mathf.Abs(diffAngle) ) {
                // �ڕW�p�x�𒴂��ĉ�]����ꍇ�͖ڕW�p�x�Ɉ�v
                angle = toAngle;
            } else {
                // �ڕW�p�x�𒴂��Ȃ��ꍇ�͂��̕����ɉ�]
                angle += diffAngle < 0 ? -rotAngle : rotAngle;
            }
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
            .Concat(target.Serialize())
            .Concat(BitConverter.GetBytes(rotateSpeed))
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

        offset = target.Deserialize(bytes, offset);
        rotateSpeed = BitConverter.ToSingle(bytes, offset);
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

        target.DrawGUI();
        rotateSpeed = UnityEditor.EditorGUILayout.FloatField("Rotate Speed", rotateSpeed);
        angleOffset = UnityEditor.EditorGUILayout.FloatField("Angle Offset", angleOffset);
    }
#endif
}
