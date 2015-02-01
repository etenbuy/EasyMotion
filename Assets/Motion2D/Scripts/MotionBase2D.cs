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
    /// 自身のtransform
    /// </summary>
    private Transform selfTrans;

    /// <summary>
    /// 初期化
    /// </summary>
    protected void Awake() {
        selfTrans = transform;
    }

    /// <summary>
    /// 直線移動
    /// </summary>
    /// <param name="from">始点</param>
    /// <param name="to">終点</param>
    /// <param name="delay">モーション開始までの時間</param>
    /// <param name="duration">モーション時間</param>
    /// <returns></returns>
    public IEnumerator Line(Vector2 from, Vector2 to, float delay, float duration) {
        var startTime = Time.time + delay;
        var endTime = startTime + duration;

        // 開始まで待機
        while ( Time.time < startTime ) {
            yield return 0;
        }

        // 開始
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
}
