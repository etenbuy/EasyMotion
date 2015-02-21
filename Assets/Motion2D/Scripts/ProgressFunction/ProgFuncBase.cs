///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   ProgFuncBase.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.21                                                                       //
//  Desc    :   進捗度合関数の基底クラス。                                                       //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 進捗度合関数の基底クラス。
/// </summary>
public class ProgFuncBase {
    /// <summary>
    /// 関数の種類
    /// </summary>
    public enum FuncType {
        Liner,
        Acc,
    };

    /// <summary>
    /// 実行時型の定義
    /// </summary>
    private static Dictionary<FuncType, Type> runtimeType = new Dictionary<FuncType, Type>() {
        { FuncType.Liner, typeof(ProgFuncBase) },
        { FuncType.Acc, typeof(AccFunc) },
    };

    /// <summary>
    /// 進捗度合を取得する
    /// </summary>
    /// <param name="progress">入力値</param>
    /// <returns>出力値</returns>
    public virtual float GetProgress(float progress) {
        return progress;
    }

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
    public static ProgFuncBase CreateInstance(ProgFuncBase.FuncType type) {
        return Activator.CreateInstance(runtimeType[type]) as ProgFuncBase;
    }

    /// <summary>
    /// デシリアライズされたオブジェクトを取得する
    /// </summary>
    /// <param name="type">関数の種類</param>
    /// <param name="bytes">シリアライズ済みデータ</param>
    /// <param name="offset">データの開始位置</param>
    /// <param name="nextOffset">次のデータの開始位置</param>
    /// <returns>実行時オブジェクト</returns>
    public static ProgFuncBase GetDeserialized(FuncType type, byte[] bytes, int offset, out int nextOffset) {
        // オブジェクト作成
        var func = CreateInstance(type);
        // デシリアライズ
        nextOffset = func.Deserialize(bytes, offset);
        return func;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public virtual void DrawGUI() {
    }
#endif
}
