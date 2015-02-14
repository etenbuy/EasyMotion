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

#if UNITY_EDITOR
    /// <summary>
    /// �O������������`�悷��
    /// </summary>
    /// <param name="from"></param>
    /// <param name="direction"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Vector2 DrawArrow(Vector2 from, Vector2 direction, Color color) {
        var to = from + direction.normalized * MotionGizmo.CameraScale * 300;
        MotionGizmo.DrawArrowCap(to, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, color);

        to = from + direction.normalized * MotionGizmo.CameraScale * 10000;
        MotionGizmo.DrawArrow(new Vector2[] { from, to }, color, false, false);

        return direction;
    }

    /// <summary>
    /// �O�Ղ̕`��(Editor�p)
    /// </summary>
    private void OnDrawGizmos() {
        DrawArrow(InitPosition2D, velocity, GizmoColor);
    }

    /// <summary>
    /// �����␳�E�B���h�E���J��
    /// </summary>
    [ContextMenu("Set Speed")]
    private void SetSpeed() {
        AdjustSpeed.Open(Speed, (speed) => {
            Speed = speed;
        });
    }

    /// <summary>
    /// �ړ����̑���
    /// </summary>
    public float Speed {
        get {
            return velocity.magnitude;
        }
        set {
            velocity = velocity.normalized * value;
        }
    }
#endif
}
