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
    /// 初期の時間進行速度
    /// </summary>
    private float initTimeScale = 1;

    /// <summary>
    /// 時間進行の加速度
    /// </summary>
    private float acceleration = 0;

    /// <summary>
    /// 時間を取得する
    /// </summary>
    /// <param name="time">入力値</param>
    /// <returns>出力値</returns>
    public override float GetTime(float time) {
        return (acceleration * time / 2 + initTimeScale) * time;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(initTimeScale))
            .Concat(BitConverter.GetBytes(acceleration)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        initTimeScale = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        acceleration = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        initTimeScale = UnityEditor.EditorGUILayout.FloatField("Init Time Scale", initTimeScale);
        acceleration = UnityEditor.EditorGUILayout.FloatField("Acceleration", acceleration);
    }
#endif
}
