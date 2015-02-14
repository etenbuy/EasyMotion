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
}
