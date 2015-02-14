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
using System.Collections.Generic;

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
        Liner,
    };

    /// <summary>
    /// ���[�V�����^�C�v�Ǝ��s���^�̕R�t�����
    /// </summary>
    private static Dictionary<MotionType, Type> typeAssoc = new Dictionary<MotionType, Type>() {
        { MotionType.Stop, typeof(MotionBase2D) },
        { MotionType.MoveTo, typeof(MoveTo2D) },
        { MotionType.MoveArc, typeof(MoveArc2D) },
        { MotionType.Liner, typeof(LinerMotion2D) },
    };

    /// <summary>
    /// ���s���^���擾����
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Type GetType(MotionType type) {
        return typeAssoc[type];
    }

    // ����
    public MotionType type;
    public float delay;
    public bool fromCurrent = true;
    public bool relative = false;
    public Vector2 from;

    // LimitedMotion����
    public float duration;

    // MoveTo
    public Vector2 to;

    // MoveArc
    public float fromAngle;
    public float rotateAngle;
    public float radius;

    // LinerMotion
    public Vector2 velocity;
}
