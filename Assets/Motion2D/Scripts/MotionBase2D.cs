///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionBase2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   2Dモーション基底。                                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 2Dモーション基底。
/// </summary>
[System.Serializable]
public class MotionBase2D {
    /// <summary>
    /// モーションを開始するまでの時間
    /// </summary>
    public float delay;

    /// <summary>
    /// 現在位置
    /// </summary>
    public Vector2 position { get; protected set; }

    /// <summary>
    /// 初期位置
    /// </summary>
    protected Vector2 initPosition { get; private set; }

    /// <summary>
    /// モーションの実行を開始する。
    /// </summary>
    /// <param name="behav">スクリプト</param>
    public void StartMotion(MonoBehaviour behav) {
        position = initPosition = behav.transform.localPosition;
        behav.StartCoroutine(ExecuteMotion(behav.transform));
    }

    /// <summary>
    /// モーションの初期化処理(派生クラスで実装する)
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected virtual bool OnStart() {
        return false;
    }

    /// <summary>
    /// モーションの更新処理(派生クラスで実装する)
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected virtual bool OnUpdate() {
        return false;
    }

    /// <summary>
    /// モーションを実行するコルーチン
    /// </summary>
    /// <param name="trans">モーション結果を反映する対象のトランスフォーム</param>
    /// <returns></returns>
    private IEnumerator ExecuteMotion(Transform trans) {
        // 開始までの一定時間待機
        if ( delay != 0 ) {
            yield return new WaitForSeconds(delay);
        }

        // モーション実行
        if ( OnStart() ) {
            while ( true ) {
                var nextUpdate = OnUpdate();
                // 位置更新
                trans.localPosition = position;
                yield return 0;

                if ( !nextUpdate ) {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public virtual byte[] Serialize() {
        var result = BitConverter.GetBytes(delay);
        return result;
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>デシリアライズに使用したバイトサイズ</returns>
    public virtual int Deserialize(byte[] bytes) {
        delay = BitConverter.ToSingle(bytes, 0);
        return sizeof(float);
    }
}
