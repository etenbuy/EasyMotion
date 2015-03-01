///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   AccTimeFunc.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.03.01                                                                       //
//  Desc    :   時間進行の加減速関数。                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 時間進行の加減速関数。
/// </summary>
public class AccTimeFunc : TimeFuncBase {
    /// <summary>
    /// 変化前の時間の進行速度
    /// </summary>
    private float fromTimeScale = 1;

    /// <summary>
    /// 変化後の時間の進行速度
    /// </summary>
    private float toTimeScale = 1;

    /// <summary>
    /// 変化時間
    /// </summary>
    private float duration = 1;

    /// <summary>
    /// 時間を取得する
    /// </summary>
    /// <param name="time">入力値</param>
    /// <returns>出力値</returns>
    public override float GetTime(float time) {
        float result;
        if ( time < duration ) {
            result = fromTimeScale + (toTimeScale - fromTimeScale) / (duration * 2) * time;
            result *= time;
        } else {
            result = toTimeScale * time + (fromTimeScale - toTimeScale) * duration / 2;
        }
        return result;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(fromTimeScale))
            .Concat(BitConverter.GetBytes(toTimeScale))
            .Concat(BitConverter.GetBytes(duration)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        fromTimeScale = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        toTimeScale = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
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
        fromTimeScale = UnityEditor.EditorGUILayout.FloatField("From Time Scale", fromTimeScale);
        toTimeScale = UnityEditor.EditorGUILayout.FloatField("To Time Scale", toTimeScale);
        duration = UnityEditor.EditorGUILayout.FloatField("Duration", duration);
    }
#endif
}
