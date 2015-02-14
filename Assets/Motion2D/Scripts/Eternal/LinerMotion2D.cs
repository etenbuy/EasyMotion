///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerMotion2D.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   �����ړ��B                                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// �����ړ��N���X�B
/// </summary>
public class LinerMotion2D : EternalMotion2D {
    /// <summary>
    /// �ړ����x
    /// </summary>
    [SerializeField]
    private Vector2 velocity;

    /// <summary>
    /// ���[�V�����R���[�`�������s����
    /// </summary>
    private void Start() {
        if ( fromCurrent ) {
            from = Position2D;
        }

        StartMotion(Move(this, from, velocity));
    }

    /// <summary>
    /// �����ړ�
    /// </summary>
    /// <param name="motion">���[�V�����I�u�W�F�N�g</param>
    /// <param name="from">�n�_</param>
    /// <param name="velocity">�ړ����x</param>
    /// <returns></returns>
    public static IEnumerator Move(MotionBase2D motion, Vector2 from, Vector2 velocity) {
        var startTime = Time.time;

        while ( true ) {
            motion.Position2D = from + velocity * (Time.time - startTime);
            yield return 0;
        }
    }
}
