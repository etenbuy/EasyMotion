///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EasyMotion2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   2D��̃��[�V�����Ǘ��B                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 2D��̃��[�V�����Ǘ��B
/// </summary>
public class EasyMotion2D : MonoBehaviour {
    /// <summary>
    /// ���[�V�����̎�ޒ�`
    /// </summary>
    private enum MotionType {
        Stop,
        MoveTo,
        MoveArc,
        Liner,
    };

    /// <summary>
    /// ���[�V�����̎��
    /// </summary>
    [SerializeField]
    private MotionType type = MotionType.Stop;

    /// <summary>
    /// ���s�Ώۂ̃��[�V����
    /// </summary>
    [SerializeField]
    private MotionBase2D motion;

    /// <summary>
    /// �������B
    /// </summary>
    private void Start() {
        motion.StartMotion(this);
    }
}
