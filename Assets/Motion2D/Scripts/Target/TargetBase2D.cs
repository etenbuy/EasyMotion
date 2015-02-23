///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   TargetBase2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.23                                                                       //
//  Desc    :   2D空間上の目標物情報を管理する基底クラス。                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 2D空間上の目標物情報を管理する基底クラス。
/// </summary>
public abstract class TargetBase2D {
    /// <summary>
    /// 種類
    /// </summary>
    public enum TargetType {
        Name,
    };

    /// <summary>
    /// 実行時型の定義
    /// </summary>
    private static Dictionary<TargetType, Type> runtimeType = new Dictionary<TargetType, Type>() {
        { TargetType.Name, typeof(TargetName2D) },
    };

    /// <summary>
    /// 目標物のTransform
    /// </summary>
    public abstract Transform transform { get; }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public virtual byte[] Serialize() {
        return new byte[0];
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public virtual int Deserialize(byte[] bytes, int offset) {
        return offset;
    }

    /// <summary>
    /// 新規の実行時オブジェクトを生成する
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static TargetBase2D CreateInstance(TargetType type) {
        return Activator.CreateInstance(runtimeType[type]) as TargetBase2D;
    }

    /// <summary>
    /// デシリアライズされたオブジェクトを取得する
    /// </summary>
    /// <param name="type">種類</param>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <param name="nextOffset">次のデータの開始位置</param>
    /// <returns>実行時オブジェクト</returns>
    public static TargetBase2D GetDeserialized(TargetType type, byte[] bytes, int offset, out int nextOffset) {
        // オブジェクト作成
        var target = CreateInstance(type);
        // デシリアライズ
        nextOffset = target.Deserialize(bytes, offset);
        return target;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public virtual void DrawGUI() {
    }
#endif
}
