///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EternalMotion2D.cs                                                               //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   永久モーション。                                                                 //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 永久モーション。
/// </summary>
public class EternalMotion2D : MotionBase2D {
    /// <summary>
    /// 時間関数の種類
    /// </summary>
    private TimeFuncBase.FuncType timeFuncType = TimeFuncBase.FuncType.None;

    /// <summary>
    /// 時間関数
    /// </summary>
    private TimeFuncBase timeFunc;

    /// <summary>
    /// 開始時刻
    /// </summary>
    private float startTime;

    /// <summary>
    /// 前フレームの時刻
    /// </summary>
    private float prevTime;

    /// <summary>
    /// モーションの初期化処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnStart() {
        startTime = Time.time;
        prevTime = timeFunc.GetTime(0);
        OnEternalStart();
        return true;
    }

    /// <summary>
    /// モーションの更新処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnUpdate() {
        float time = timeFunc.GetTime(Time.time - startTime);
        float deltaTime = time - prevTime;
        prevTime = time;

        // 永久モーション更新
        return OnEternalUpdate(time, deltaTime);
    }

    /// <summary>
    /// 永久モーションの初期化処理(派生クラスで実装する)
    /// </summary>
    protected virtual void OnEternalStart() {
    }

    /// <summary>
    /// 永久モーションの更新処理(派生クラスで実装する)
    /// </summary>
    /// <param name="time">モーション開始からの経過時間</param>
    /// <param name="deltaTime">前回フレームからの経過時間</param>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected virtual bool OnEternalUpdate(float time, float deltaTime) {
        return false;
    }

    /// <summary>
    /// 時間関数に影響されない時間
    /// </summary>
    protected float realTime {
        get {
            return Time.time - startTime;
        }
    }

    /// <summary>
    /// 実時間に影響されないデルタ時間
    /// </summary>
    protected float realDeltaTime {
        get {
            return Time.deltaTime;
        }
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        if ( timeFunc == null ) {
            timeFunc = TimeFuncBase.CreateInstance(timeFuncType);
        }

        return result
            .Concat(BitConverter.GetBytes((int)timeFuncType))
            .Concat(timeFunc.Serialize()).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        timeFuncType = (TimeFuncBase.FuncType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        timeFunc = TimeFuncBase.GetDeserialized(timeFuncType, bytes, offset, out offset);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        var prevType = timeFuncType;
        timeFuncType = (TimeFuncBase.FuncType)UnityEditor.EditorGUILayout.EnumPopup("Function", timeFuncType);

        if ( timeFuncType != prevType || timeFunc == null ) {
            // 型が変更された
            timeFunc = TimeFuncBase.CreateInstance(timeFuncType);
        }

        timeFunc.DrawGUI();
    }
#endif
}
