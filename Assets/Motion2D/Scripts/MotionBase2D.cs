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
    /// 回転の種類
    /// </summary>
    [SerializeField]
    private SerializedMotion2D.RotateType rotateType;

    /// <summary>
    /// 回転角度の基準位置
    /// </summary>
    [SerializeField]
    private float rotateOffset = 0;

    /// <summary>
    /// 即座に目標角度に回転するかどうか
    /// </summary>
    [SerializeField]
    private bool rotateImmediate = true;

    /// <summary>
    /// 回転速度(rotateImmediate=false時有効)
    /// </summary>
    [SerializeField]
    private float rotateSpeed = 360;

    /// <summary>
    /// 自身のtransform
    /// </summary>
    private Transform selfTrans;

    /// <summary>
    /// 1フレーム前の位置
    /// </summary>
    private Vector2 prevPosition;

    /// <summary>
    /// 1フレーム前の回転角度
    /// </summary>
    private float prevForward;

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
        prevPosition = Position2D;
#if UNITY_EDITOR
        initPosition = prevPosition;
#endif

        if ( rotateType != SerializedMotion2D.RotateType.None ) {
            prevForward = selfTrans.localRotation.eulerAngles.z - rotateOffset;
            UpdateRotation();
            StartCoroutine(RotationCoroutine());
        }
    }

    /// <summary>
    /// 位置座標(本クラスより位置情報にアクセスする際に使用する)
    /// </summary>
    public Vector2 Position2D {
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

    /// <summary>
    /// 前方の向き
    /// </summary>
    protected virtual float Forward {
        get {
            var pos = Position2D;
            var diff = pos - prevPosition;
            prevPosition = pos;
            if ( diff != Vector2.zero ) {
                prevForward = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            }
            return prevForward;
        }
    }

    /// <summary>
    /// 回転処理用コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator RotationCoroutine() {
        while ( true ) {
            UpdateRotation();
            yield return 0;
        }
    }

    /// <summary>
    /// 回転角度を更新する
    /// </summary>
    private void UpdateRotation() {
        float curRotation = selfTrans.localRotation.eulerAngles.z - rotateOffset;
        float toRotation = curRotation;

        switch ( rotateType ) {
        case SerializedMotion2D.RotateType.Forward:
            toRotation = Forward;
            break;
        case SerializedMotion2D.RotateType.To:
            // TODO 実装
            break;
        default:
            return;
        }

        float nextRotation = toRotation;
        if ( !rotateImmediate ) {
            // 向きの回転
            var diffRotation = AdjustAngleRange(toRotation - curRotation, -180);
            var rot = rotateSpeed * Time.deltaTime;
            if ( Mathf.Abs(diffRotation) > rot ) {
                if ( diffRotation < 0 ) {
                    rot = -rot;
                }
                nextRotation = curRotation + rot;
            }
        }

        selfTrans.localRotation = Quaternion.Euler(0, 0, nextRotation + rotateOffset);
    }

    /// <summary>
    /// 角度を指定範囲に補正する
    /// </summary>
    /// <param name="angle">補正対象の角度</param>
    /// <param name="minAngle">角度範囲の最小値</param>
    /// <returns>補正された角度</returns>
    protected static float AdjustAngleRange(float angle, float minAngle) {
        var maxAngle = minAngle + 360f;

        if ( angle >= minAngle && angle < maxAngle ) {
            return angle;
        }

        angle -= minAngle;
        angle = angle % 360f;
        if ( angle < 0 ) {
            angle += 360f;
        }
        angle += minAngle;

        return angle;
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

    /// <summary>
    /// 移動時の速さ
    /// </summary>
    public virtual float Speed {
        get { return 0; }
        set {}
    }

    /// <summary>
    /// 速さ補正ウィンドウを開く
    /// </summary>
    [ContextMenu("Set Speed")]
    protected void OpenAdjustSpeedWindow() {
        // ウィンドウを開く
        AdjustSpeed.Open(Speed, (speed) => {
            Speed = speed;
        });
    }
#endif
}
