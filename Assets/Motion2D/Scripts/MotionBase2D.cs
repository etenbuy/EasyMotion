///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionBase2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   2Dモーション基底クラスを定義する。                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 2Dモーション基底クラス
/// </summary>
public class MotionBase2D : MonoBehaviour {
    /// <summary>
    /// 現在位置を始点とするかどうか
    /// </summary>
    [SerializeField]
    protected bool fromCurrent = false;

    /// <summary>
    /// 始点
    /// </summary>
    [SerializeField]
    protected Vector2 from = Vector2.zero;

    /// <summary>
    /// 自身のtransform
    /// </summary>
    private Transform selfTrans;

    /// <summary>
    /// 自身の初期位置
    /// </summary>
    private Vector2 initPosition;

    /// <summary>
    /// 初期化
    /// </summary>
    protected void Awake() {
        selfTrans = transform;
        initPosition = selfTrans.localPosition;
    }

    /// <summary>
    /// 直線移動
    /// </summary>
    /// <param name="from">始点</param>
    /// <param name="to">終点</param>
    /// <param name="delay">モーション開始までの時間</param>
    /// <param name="duration">モーション時間</param>
    /// <returns></returns>
    protected IEnumerator Line(Vector2 from, Vector2 to, float delay, float duration) {
        var startTime = Time.time + delay;
        var endTime = startTime + duration;

        // 開始まで待機
        while ( Time.time < startTime ) {
            yield return 0;
        }

        // 直線モーション実行
        while ( Time.time < endTime ) {
            // 現在位置計算
            var progress = (Time.time - startTime) / duration;
            var current = (1 - progress) * from + progress * to;

            // 位置反映
            Position2D = current;

            yield return 0;
        }

        // 終了
        Position2D = to;
    }

    /// <summary>
    /// 旋回移動
    /// </summary>
    /// <param name="from">始点</param>
    /// <param name="fromAngle">初角度</param>
    /// <param name="rotateAngle">旋回角度</param>
    /// <param name="radius">旋回半径</param>
    /// <param name="delay">モーション開始までの時間</param>
    /// <param name="duration">モーション時間</param>
    /// <returns></returns>
    protected IEnumerator Curve(Vector2 from, float fromAngle, float rotateAngle, float radius, float delay, float duration) {
        var startTime = Time.time + delay;
        var endTime = startTime + duration;

        // 開始まで待機
        while ( Time.time < startTime ) {
            yield return 0;
        }

        if ( rotateAngle < 0 ) {
            // 右旋回の場合
            fromAngle += 180;
        }

        // 旋回モーション実行
        rotateAngle *= Mathf.Deg2Rad;
        fromAngle *= Mathf.Deg2Rad;

        var fromSin = Mathf.Sin(fromAngle);
        var fromCos = Mathf.Cos(fromAngle);

        while ( Time.time < endTime ) {
            // 現在位置計算
            var progress = (Time.time - startTime) / duration;
            var curAngle = fromAngle + progress * rotateAngle;

            Position2D = from + new Vector2(
                -fromSin + Mathf.Sin(curAngle),
                fromCos - Mathf.Cos(curAngle)
            ) * radius;

            yield return 0;
        }

        // 終了
        var toAngle = fromAngle + rotateAngle;
        Position2D = from + new Vector2(
            -fromSin + Mathf.Sin(toAngle),
            fromCos - Mathf.Cos(toAngle)
        ) * radius;
    }

    /// <summary>
    /// 位置座標(本クラスより位置情報にアクセスする際に使用する)
    /// </summary>
    protected Vector2 Position2D {
        get {
            return (Vector2)selfTrans.localPosition;
        }
        set {
            selfTrans.localPosition = new Vector3(
                value.x,
                value.y,
                selfTrans.localPosition.z
            );
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// 初期位置
    /// </summary>
    protected Vector2 InitPosition2D {
        get {
            return GetInitPosition2D(fromCurrent);
        }
    }

    /// <summary>
    /// 初期位置を取得する
    /// </summary>
    /// <param name="fromCurrent"></param>
    /// <returns></returns>
    protected Vector2 GetInitPosition2D(bool fromCurrent) {
        if ( Application.isPlaying ) {
            return initPosition;
        } else if ( fromCurrent ) {
            return transform.localPosition;
        } else {
            return from;
        }
    }
#endif
}
