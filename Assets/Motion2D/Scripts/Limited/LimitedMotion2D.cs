///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LimitedMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   �������[�V�����B                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �������[�V�����B
/// </summary>
public class LimitedMotion2D : MotionBase2D {
    /// <summary>
    /// �ړ�����
    /// </summary>
    private float duration;

    /// <summary>
    /// �I������
    /// </summary>
    private float endTime;

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnStart() {
        endTime = Time.time + duration;
        return true;
    }

    /// <summary>
    /// ���[�V�����̍X�V����
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnUpdate() {
        // ���[�V�����̐i�����v�Z
        var progress = 1 - (endTime - Time.time) / duration;

        // ����X�V���邩�ǂ����̃`�F�b�N(progress��1�ɓ��B������I��)
        var updateNext = progress < 1;
        if ( !updateNext ) {
            // �X�V���Ȃ����1�Ƃ��ďI��
            progress = 1;
        }

        // �������[�V�����X�V
        OnLimitedUpdate(progress);

        return updateNext;
    }

    /// <summary>
    /// �������[�V�����̍X�V����(�h���N���X�Ŏ�������)
    /// </summary>
    /// <param name="progress">�i����</param>
    protected virtual void OnLimitedUpdate(float progress) {
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(duration)).ToArray();
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        duration = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        duration = UnityEditor.EditorGUILayout.FloatField("Duration", duration);
    }
#endif
}
