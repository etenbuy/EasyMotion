///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EternalMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   �������[�V�����B                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// �������[�V�����̊��N���X
/// </summary>
public class EternalMotion2D : MotionBase2D {
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

#if UNITY_EDITOR
    /// <summary>
    /// Gizmo�\���F
    /// </summary>
    protected Color GizmoColor {
        get {
            if ( !Application.isPlaying ) {
                return MotionGizmo.EditorColor;
            } else {
                return MotionGizmo.MovingColor;
            }
        }
    }
#endif
}
