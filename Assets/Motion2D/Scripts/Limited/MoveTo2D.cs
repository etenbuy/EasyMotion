///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveTo2D.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   �w��ʒu�Ɉړ����郂�[�V�����B                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// �w��ʒu�Ɉړ����郂�[�V�����B
/// </summary>
public class MoveTo2D : LimitedMotion2D {
    /// <summary>
    /// �ړ���̈ʒu
    /// </summary>
    [SerializeField]
    private Vector2 to;

    /// <summary>
    /// �������[�V�����̍X�V����
    /// </summary>
    /// <param name="progress">�i����</param>
    protected override void OnLimitedUpdate(float progress) {
        position = (1 - progress) * initPosition + progress * to;
    }
}
