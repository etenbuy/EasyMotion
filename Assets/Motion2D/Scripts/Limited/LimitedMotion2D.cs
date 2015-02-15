///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LimitedMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   �������[�V�����B                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// �������[�V�����B
/// </summary>
public class LimitedMotion2D : MotionBase2D {
    /// <summary>
    /// �ړ�����
    /// </summary>
    [SerializeField]
    protected float duration = 0;

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
}
