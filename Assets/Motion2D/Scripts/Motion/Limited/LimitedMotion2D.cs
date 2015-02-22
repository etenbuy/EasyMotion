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
    protected float duration;

    /// <summary>
    /// 進捗率関数の種類
    /// </summary>
    private ProgFuncBase.FuncType progFuncType = ProgFuncBase.FuncType.Liner;

    /// <summary>
    /// 進捗率関数
    /// </summary>
    private ProgFuncBase progFunc;

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
        OnLimitedStart();
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
        OnLimitedUpdate(progFunc.GetProgress(progress));

        return updateNext;
    }

    /// <summary>
    /// 時限モーションの初期化処理(派生クラスで実装する)
    /// </summary>
    protected virtual void OnLimitedStart() {
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

        if ( progFunc == null ) {
            progFunc = ProgFuncBase.CreateInstance(progFuncType);
        }

        return result
            .Concat(BitConverter.GetBytes(duration))
            .Concat(BitConverter.GetBytes((int)progFuncType))
            .Concat(progFunc.Serialize()).ToArray();
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
        progFuncType = (ProgFuncBase.FuncType)BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);
        progFunc = ProgFuncBase.GetDeserialized(progFuncType, bytes, offset, out offset);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        duration = UnityEditor.EditorGUILayout.FloatField("Duration", duration);
        var prevType = progFuncType;
        progFuncType = (ProgFuncBase.FuncType)UnityEditor.EditorGUILayout.EnumPopup("Function", progFuncType);

        if ( progFuncType != prevType || progFunc == null ) {
            // 型が変更された
            progFunc = ProgFuncBase.CreateInstance(progFuncType);
        }

        progFunc.DrawGUI();
    }
#endif
}
