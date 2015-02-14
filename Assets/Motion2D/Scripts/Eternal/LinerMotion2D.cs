///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerMotion2D.cs                                                                 //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   直線移動。                                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 直線移動クラス。
/// </summary>
public class LinerMotion2D : EternalMotion2D {
    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField]
    private Vector2 velocity;

    /// <summary>
    /// モーションコルーチンを実行する
    /// </summary>
    private void Start() {
        if ( fromCurrent ) {
            from = Position2D;
        }

        StartMotion(Move(this, from, velocity));
    }

    /// <summary>
    /// 直線移動
    /// </summary>
    /// <param name="motion">モーションオブジェクト</param>
    /// <param name="from">始点</param>
    /// <param name="velocity">移動速度</param>
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
    /// 軌道を示す矢印を描画する
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
    /// 軌跡の描画(Editor用)
    /// </summary>
    private void OnDrawGizmos() {
        DrawArrow(InitPosition2D, velocity, GizmoColor);
    }

    /// <summary>
    /// 速さ補正ウィンドウを開く
    /// </summary>
    [ContextMenu("Set Speed")]
    private void SetSpeed() {
        AdjustSpeed.Open(Speed, (speed) => {
            Speed = speed;
        });
    }

    /// <summary>
    /// 移動時の速さ
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
