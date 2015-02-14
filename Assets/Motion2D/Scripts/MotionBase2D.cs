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
    /// 移動開始までの時間
    /// </summary>
    [SerializeField]
    protected float delay = 0;

    /// <summary>
    /// 自身のtransform
    /// </summary>
    private Transform selfTrans;

#if UNITY_EDITOR
    /// <summary>
    /// 自身の初期位置
    /// </summary>
    private Vector2 initPosition;
#endif

    /// <summary>
    /// 初期化
    /// </summary>
    protected void Awake() {
        selfTrans = transform;
#if UNITY_EDITOR
        initPosition = selfTrans.localPosition;
#endif
    }

    /// <summary>
    /// 位置座標(本クラスより位置情報にアクセスする際に使用する)
    /// </summary>
    public Vector2 Position2D {
        get {
#if UNITY_EDITOR
            selfTrans = transform;
#endif
            return (Vector2)selfTrans.localPosition;
        }
        set {
#if UNITY_EDITOR
            selfTrans = transform;
#endif
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
