///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveForward2D.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.25                                                                       //
//  Desc    :   ���݂̌����ɑO�i���郂�[�V�����B                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// ���݂̌����ɑO�i���郂�[�V�����B
/// </summary>
public class MoveForward2D : EternalMotion2D {
    /// <summary>
    /// ���i���鑬��
    /// </summary>
    private float speed;

    /// <summary>
    /// �O�i�����̂��炵�p�x
    /// </summary>
    private float angleOffset;

    /// <summary>
    /// �ړ����x
    /// </summary>
    private Vector2 velocity;

    /// <summary>
    /// ���݂̌���
    /// </summary>
    private float curAngle = NO_DIRECTION;

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    protected override void OnInit() {
        base.OnInit();
        // �����ϐ��̏�����
        UpdateParam();
    }

    /// <summary>
    /// �i�v���[�V�����̏���������
    /// </summary>
    protected override void OnEternalStart() {
        // �����ϐ��̏�����
        UpdateParam();
    }

    /// <summary>
    /// �i�v���[�V�����̍X�V����(�h���N���X�Ŏ�������)
    /// </summary>
    /// <param name="time">���[�V�����J�n����̌o�ߎ���</param>
    /// <param name="deltaTime">�O��t���[������̌o�ߎ���</param>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnEternalUpdate(float time, float deltaTime) {
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
            .Concat(BitConverter.GetBytes(speed))
            .Concat(BitConverter.GetBytes(angleOffset)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        speed = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        angleOffset = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    public override float currentDirection {
        get {
            if ( Application.isPlaying ) {
                return curAngle;
            } else {
                return transform.localEulerAngles.z + angleOffset;
            }
        }
    }

    /// <summary>
    /// �����I�Ɏg�p����i�s�����E���x���X�V����
    /// </summary>
    private void UpdateParam() {
        // �i�s�����̌v�Z
        curAngle = transform.localEulerAngles.z + angleOffset;

        // �ړ����x�̌v�Z
        var curAngleRad = curAngle * Mathf.Deg2Rad;
        velocity = new Vector2(Mathf.Cos(curAngleRad), Mathf.Sin(curAngleRad)) * speed;
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

    /// <summary>
    /// Gizmo��`�悷��
    /// </summary>
    /// <param name="from">���݈ʒu</param>
    /// <returns>�ړ���̈ʒu</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        if ( !Application.isPlaying ) {
            UpdateParam();
        }

        var to = from + velocity;
        DrawLine(from, to);
        DrawArrowCap(to, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);

        return from + velocity * float.MaxValue;
    }

    /// <summary>
    /// �����擾
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <returns>�ݒ肳�ꂽ����</returns>
    public override float GetSpeed(Vector2 from) {
        return velocity.magnitude;
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="speed">����</param>
    public override void SetSpeed(Vector2 from, float speed) {
        velocity = velocity.normalized * speed;
    }
#endif
}
