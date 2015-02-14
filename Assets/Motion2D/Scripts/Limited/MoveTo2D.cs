///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveTo2D.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   直線モーション。                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 直線モーション
/// </summary>
public class MoveTo2D : LimitedMotion2D {
    /// <summary>
    /// 終点
    /// </summary>
    [SerializeField]
    private Vector2 to = Vector2.zero;

    /// <summary>
    /// 直線移動コルーチンを実行する
    /// </summary>
    private void Start() {
        var toPos = to;

        if ( fromCurrent ) {
            from = Position2D;

            if ( relative ) {
                toPos += from;
            }
        }

        StartMotion(Move(this, from, toPos, duration));
    }

    /// <summary>
    /// 直線移動
    /// </summary>
    /// <param name="motion">モーションオブジェクト</param>
    /// <param name="from">始点</param>
    /// <param name="to">終点</param>
    /// <param name="duration">モーション時間</param>
    /// <returns></returns>
    public static IEnumerator Move(MotionBase2D motion, Vector2 from, Vector2 to, float duration) {
        var startTime = Time.time;
        var endTime = startTime + duration;

        // 直線モーション実行
        while ( Time.time < endTime ) {
            // 現在位置計算
            var progress = (Time.time - startTime) / duration;
            var current = (1 - progress) * from + progress * to;

            // 位置反映
            motion.Position2D = current;

            yield return 0;
        }

        // 終了
        motion.Position2D = to;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 軌道を示す矢印を描画する
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Vector2 DrawArrow(Vector2 from, Vector2 to, Color color) {
        MotionGizmo.DrawArrow(new Vector2[] { from, to }, color);
        return to;
    }

    /// <summary>
    /// 軌跡の描画(Editor用)
    /// </summary>
    private void OnDrawGizmos() {
        DrawArrow(FromPosition, ToPosition, GizmoColor);
    }

    /// <summary>
    /// 始点(Editor用)
    /// </summary>
    private Vector2 FromPosition {
        get {
            return InitPosition2D;
        }
    }

    /// <summary>
    /// 終点(Editor用)
    /// </summary>
    private Vector2 ToPosition {
        get {
            return relative ? to + InitPosition2D : to;
        }
    }

    /// <summary>
    /// 移動時の速さ
    /// </summary>
    public override float Speed {
        get {
            var line = ToPosition - FromPosition;
            var curSpeed = 0f;
            if ( duration != 0 ) {
                curSpeed = line.magnitude / duration;
            }
            return curSpeed;
        }
        set {
            SetSpeed(value, FromPosition, ToPosition, out duration);
        }
    }

    public static void SetSpeed(float speed, Vector2 from, Vector2 to, out float duration) {
        var line = to - from;
        if ( speed == 0 ) {
            duration = 0;
        } else {
            duration = line.magnitude / speed;
        }
    }
#endif
}
