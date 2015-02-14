///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   SerializedMotion.cs                                                              //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   �V���A���C�Y�ς݃��[�V�������B                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// �V���A���C�Y�ς݃��[�V�������B
/// </summary>
[Serializable]
public class SerializedMotion2D {
    /// <summary>
    /// ���[�V�����^�C�v
    /// </summary>
    public enum MotionType {
        Stop,
        MoveTo,
        MoveArc,
    };

    // ����
    public MotionType type;
    public float delay;
    public float duration;
    public bool fromCurrent = true;
    public bool relative = false;
    public Vector2 from;

    // �����ړ��p
    public Vector2 to;

    // ����ړ��p
    public float fromAngle;
    public float rotateAngle;
    public float radius;
}
