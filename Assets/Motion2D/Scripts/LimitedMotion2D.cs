///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LimitedMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
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
    /// �ړ��J�n�܂ł̎���
    /// </summary>
    [SerializeField]
    protected float delay = 0;

    /// <summary>
    /// �ړ�����
    /// </summary>
    [SerializeField]
    protected float duration = 0;

    /// <summary>
    /// ���[�V�������J�n����
    /// </summary>
    /// <param name="motion"></param>
    protected Coroutine StartMotion(IEnumerator motion) {
        return StartCoroutine(ExecuteMotion(motion));
    }

    /// <summary>
    /// ���[�V�������s
    /// </summary>
    /// <param name="motion"></param>
    /// <returns></returns>
    private IEnumerator ExecuteMotion(IEnumerator motion) {
        // �J�n�܂őҋ@
        yield return new WaitForSeconds(delay);
        // ���[�V�������s
        yield return StartCoroutine(motion);
    }
}
