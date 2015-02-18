///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MoveLiner2D.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   直線に移動し続けるモーション。                                                   //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

/// <summary>
/// 直線に移動し続けるモーション。
/// </summary>
public class MoveLiner2D : EternalMotion2D {
    /// <summary>
    /// 移動速度
    /// </summary>
    private Vector2 velocity;

    /// <summary>
    /// 永久モーションの更新処理
    /// </summary>
    /// <param name="time">経過時間</param>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnEternalUpdate(float time) {
        position = initPosition + velocity * time;
        return true;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        return result
            .Concat(BitConverter.GetBytes(velocity.x))
            .Concat(BitConverter.GetBytes(velocity.y)).ToArray();
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>デシリアライズに使用したバイトサイズ</returns>
    public override int Deserialize(byte[] bytes) {
        var offset = base.Deserialize(bytes);

        velocity.x = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);
        velocity.y = BitConverter.ToSingle(bytes, offset);
        offset += sizeof(float);

        return offset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();
        velocity = UnityEditor.EditorGUILayout.Vector2Field("Velocity", velocity);
    }

    /// <summary>
    /// Gizmoを描画する
    /// </summary>
    protected override void DrawGizmos() {
        var to = initPosition + velocity;
        DrawLine(initPosition, to);
        DrawArrowCap(to, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);
    }
#endif
}
