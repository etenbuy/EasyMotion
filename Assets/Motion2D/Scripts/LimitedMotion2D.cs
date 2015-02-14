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

#if UNITY_EDITOR
    private bool isMoving = false;
#endif

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
#if UNITY_EDITOR
        isMoving = true;
#endif

        // �J�n�܂őҋ@
        yield return new WaitForSeconds(delay);
        // ���[�V�������s
        yield return StartCoroutine(motion);

#if UNITY_EDITOR
        isMoving = false;
#endif
    }

#if UNITY_EDITOR
    /// <summary>
    /// Gizmo�\���F
    /// </summary>
    protected Color GizmoColor {
        get {
            if ( !Application.isPlaying ) {
                return MotionGizmo.EditorColor;
            } else if ( isMoving ) {
                return MotionGizmo.MovingColor;
            } else {
                return MotionGizmo.DisableColor;
            }
        }
    }
#endif
}
