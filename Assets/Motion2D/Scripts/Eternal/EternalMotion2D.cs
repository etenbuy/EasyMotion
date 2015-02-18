///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EternalMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   �i�v���[�V�����B                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// �i�v���[�V�����B
/// </summary>
public class EternalMotion2D : MotionBase2D {
    /// <summary>
    /// �J�n����
    /// </summary>
    private float startTime;

    /// <summary>
    /// ���[�V�����̏���������
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnStart() {
        startTime = Time.time;
        return true;
    }

    /// <summary>
    /// ���[�V�����̍X�V����
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected override bool OnUpdate() {
        // �i�v���[�V�����X�V
        return OnEternalUpdate(Time.time - startTime);
    }

    /// <summary>
    /// �i�v���[�V�����̍X�V����(�h���N���X�Ŏ�������)
    /// </summary>
    /// <param name="time">�o�ߎ���</param>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected virtual bool OnEternalUpdate(float time) {
        return false;
    }
}
