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
    private Direction2D fromDirection;

    /// <summary>
    /// �O�i���鑬��
    /// </summary>
    private float speed;

    /// <summary>
    /// ���񐬕��̎��Ԋ֐��̎��
    /// </summary>
    private TimeFuncBase.FuncType rotateTimeFuncType = TimeFuncBase.FuncType.None;

    /// <summary>
    /// ���񐬕��̎��Ԋ֐�
    /// </summary>
    private TimeFuncBase rotateTimeFunc;

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
    /// �ڕW�����������烂�[�V�������I�����邩�ǂ���
    /// </summary>
    private bool lookEnd;

    /// <summary>
    /// ���݂̌���
    /// </summary>
    private float curAngle = NO_DIRECTION;

    /// <summary>
    /// 1�t���[���O�̐��񎞍�
    /// </summary>
    private float prevRotateTime;

    /// <summary>
    /// �ڕW���Ɍ������Ƃ��ɌĂ΂��C�x���g
    /// </summary>
    public MotionEvent onTarget;

    public ChaseMotion2D() {
        fromDirection = new Direction2D(this);
    }

    /// <summary>
    /// �i�v���[�V�����̏���������
    /// </summary>
    protected override void OnEternalStart() {
        curAngle = fromDirection.direction;
        prevRotateTime = rotateTimeFunc.GetTime(realTime);
    }

    /// <summary>
    /// �i�v���[�V�����̍X�V����(�h���N���X�Ŏ�������)
    /// </summary>
    /// <param name="time">���[�V�����J�n����̌o�ߎ���</param>
    /// <param name="deltaTime">�O��t���[������̌o�ߎ���</param>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnEternalUpdate(float time, float deltaTime) {
        var result = true;

        // �ڕW����Transform�擾
        var targetTrans = target.transform;
        if ( targetTrans != null ) {
            // �ڕW���ւ̌����v�Z
            var targetPos = targetTrans.position;
            if ( transform.parent != null ) {
                targetPos = transform.parent.InverseTransformPoint(targetPos);
            }
            var targetDir = (Vector2)targetPos - position;
            var toAngle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

            // ��]�ʌv�Z
            float diffAngle = RotationBase2D.AdjustAngleRange(toAngle - curAngle, -180);
            var rotateTime = rotateTimeFunc.GetTime(realTime);
            var rotAngle = rotateSpeed * (rotateTime - prevRotateTime);
            prevRotateTime = rotateTime;

            // �����X�V
            if ( rotAngle > Mathf.Abs(diffAngle) ) {
                // �ڕW�p�x�𒴂��ĉ�]����ꍇ�͖ڕW�p�x�Ɉ�v
                curAngle = toAngle;

                if ( onTarget != null ) {
                    // �ڕW���Ɍ������C�x���g���s
                    onTarget();
                }

                if ( lookEnd ) {
                    // ���[�V�����I��
                    result = false;
                }

            } else {
                // �ڕW�p�x�𒴂��Ȃ��ꍇ�͂��̕����ɉ�]
                curAngle += diffAngle < 0 ? -rotAngle : rotAngle;
            }
        }

        // �ʒu�X�V
        var curAngleRad = curAngle * Mathf.Deg2Rad;
        position += new Vector2(Mathf.Cos(curAngleRad), Mathf.Sin(curAngleRad)) * speed * deltaTime;

        return result;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        if ( rotateTimeFunc == null ) {
            rotateTimeFunc = TimeFuncBase.CreateInstance(rotateTimeFuncType);
        }

        if ( target == null ) {
            target = TargetBase2D.CreateInstance(targetType);
        }

        return result
            .Concat(fromDirection.Serialize())
            .Concat(BitConverter.GetBytes(speed))
            .Concat(BitConverter.GetBytes((int)rotateTimeFuncType))
            .Concat(rotateTimeFunc.Serialize())
            .Concat(BitConverter.GetBytes(rotateSpeed))
            .Concat(BitConverter.GetBytes((int)targetType))
            .Concat(target.Serialize())
            .Concat(BitConverter.GetBytes(lookEnd)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        offset = fromDirection.Deserialize(bytes, offset);
        speed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        rotateTimeFuncType = (TimeFuncBase.FuncType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        rotateTimeFunc = TimeFuncBase.GetDeserialized(rotateTimeFuncType, bytes, offset, out offset);

        rotateSpeed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        targetType = (TargetBase2D.TargetType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        target = TargetBase2D.GetDeserialized(targetType, bytes, offset, out offset);

        lookEnd = BitConverter.ToBoolean(bytes, offset);
        offset += sizeof(bool);

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

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();

        fromDirection.DrawGUI();
        speed = UnityEditor.EditorGUILayout.FloatField("Speed", speed);

        var prevTimeFuncType = rotateTimeFuncType;
        rotateTimeFuncType = (TimeFuncBase.FuncType)UnityEditor.EditorGUILayout.EnumPopup("Rotate Function", rotateTimeFuncType);
        if ( rotateTimeFuncType != prevTimeFuncType || rotateTimeFunc == null ) {
            // �^���ύX���ꂽ
            rotateTimeFunc = TimeFuncBase.CreateInstance(rotateTimeFuncType);
        }
        rotateTimeFunc.DrawGUI();

        rotateSpeed = UnityEditor.EditorGUILayout.FloatField("Rotate Speed", rotateSpeed);

        var prevType = targetType;
        targetType = (TargetBase2D.TargetType)UnityEditor.EditorGUILayout.EnumPopup("Target Type", targetType);
        if ( targetType != prevType || target == null ) {
            // �^���ύX���ꂽ
            target = TargetBase2D.CreateInstance(targetType);
        }
        target.DrawGUI();

        lookEnd = UnityEditor.EditorGUILayout.Toggle("Look End", lookEnd);
    }

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    /// <param name="from">���݈ʒu</param>
    /// <returns>�ړ���̈ʒu</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        if ( !Application.isPlaying ) {
            return from;
        }

        return position;
    }

    /// <summary>
    /// �����擾
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <returns>�ݒ肳�ꂽ����</returns>
    public override float GetSpeed(Vector2 from) {
        return speed;
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="speed">����</param>
    public override void SetSpeed(Vector2 from, float speed) {
        this.speed = speed;
    }

    /// <summary>
    /// �I�[�ʒu�̌������擾����
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="fromAngle">�J�n�p�x</param>
    /// <returns>�I�[�ʒu�̌���</returns>
    public override float GetEndDirection(Vector2 from, float fromAngle) {
        if ( !Application.isPlaying || curAngle == NO_DIRECTION ) {
            return fromAngle;
        }

        return curAngle;
    }
#endif
}
