///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveTo2D.cs                                                                      //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   指定位置に移動するモーション。                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 指定位置に移動するモーション。
/// </summary>
public class MoveTo2D : LimitedMotion2D {
    /// <summary>
    /// 移動先の位置
    /// </summary>
    private Vector2 to;

    /// <summary>
    /// 時限モーションの更新処理
    /// </summary>
    /// <param name="progress">進捗率</param>
    protected override void OnLimitedUpdate(float progress) {
        position = (1 - progress) * initPosition + progress * to;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(to.x))
            .Concat(BitConverter.GetBytes(to.y)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        to.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        to.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        to = UnityEditor.EditorGUILayout.Vector2Field("To", to);
    }

    /// <summary>
    /// Gizmoを描画する
    /// </summary>
    /// <param name="from">現在位置</param>
    /// <returns>移動後の位置</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        DrawLine(from, to);
        var dir = to - from;
        DrawArrowCap(to, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        return to;
    }
#endif
}
