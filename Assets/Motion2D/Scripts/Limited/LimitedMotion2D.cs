///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LimitedMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   時限モーション。                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 時限モーション。
/// </summary>
public class LimitedMotion2D : MotionBase2D {
    /// <summary>
    /// 移動時間
    /// </summary>
    private float duration;

    /// <summary>
    /// 終了時刻
    /// </summary>
    private float endTime;

    /// <summary>
    /// モーションの初期化処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnStart() {
        endTime = Time.time + duration;
        return true;
    }

    /// <summary>
    /// モーションの更新処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnUpdate() {
        // モーションの進捗率計算
        var progress = 1 - (endTime - Time.time) / duration;

        // 次回更新するかどうかのチェック(progressが1に到達したら終了)
        var updateNext = progress < 1;
        if ( !updateNext ) {
            // 更新しなければ1として終了
            progress = 1;
        }

        // 時限モーション更新
        OnLimitedUpdate(progress);

        return updateNext;
    }

    /// <summary>
    /// 時限モーションの更新処理(派生クラスで実装する)
    /// </summary>
    /// <param name="progress">進捗率</param>
    protected virtual void OnLimitedUpdate(float progress) {
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result.Concat(BitConverter.GetBytes(duration)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        duration = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        duration = UnityEditor.EditorGUILayout.FloatField("Duration", duration);
    }
#endif
}
