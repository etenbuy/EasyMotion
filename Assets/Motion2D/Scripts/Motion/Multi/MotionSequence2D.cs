///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionSequence2D.cs                                                              //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.18                                                                       //
//  Desc    :   モーションの連続動作。                                                           //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// モーションの連続動作。
/// </summary>
public class MotionSequence2D : MotionBase2D {
    /// <summary>
    /// 動作対象のモーション
    /// </summary>
    private MotionBase2D[] motions;

    /// <summary>
    /// 現在実行中のモーション
    /// </summary>
    private int current;

    /// <summary>
    /// 現在の向き
    /// </summary>
    private float curAngle;

    /// <summary>
    /// モーション切り替えイベント
    /// </summary>
    /// <param name="motion"></param>
    public delegate void OnChange(int motion);

    /// <summary>
    /// モーション切り替えイベント
    /// </summary>
    public OnChange onChange;

    /// <summary>
    /// モーションの初期化処理
    /// </summary>
    protected override void OnInit() {
        base.OnInit();

        foreach ( var motion in motions ) {
            motion.InitMotion(transform);
        }
    }

    /// <summary>
    /// モーションの初期化処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnStart() {
        if ( motions.Length == 0 ) {
            return false;
        }

        current = 0;
        motions[0].StartMotion();
        curAngle = motions[0].direction;

        return true;
    }

    /// <summary>
    /// モーションの更新処理
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected override bool OnUpdate() {
        if ( current >= motions.Length ) {
            // 実行するモーションが存在しなければ終了
            return false;
        }

        // モーションの状態更新
        var nextUpdate = motions[current].UpdateMotion(false);
        position = motions[current].position;

        // 向きの更新
        curAngle = motions[current].direction;

        if ( !nextUpdate ) {
            // 次のモーションに遷移
            if ( ++current >= motions.Length ) {
                // 実行するモーションが存在しなければ終了
                return false;
            }

            if ( onChange != null ) {
                // モーション変更イベント実行
                onChange(current);
            }

            // 次のモーション初期化
            transform.localPosition = new Vector3(position.x, position.y, transform.localPosition.z);
            motions[current].InitMotion(transform, motions[current - 1].direction);
            motions[current].StartMotion();

            // 向きの更新
            curAngle = motions[current].direction;
        }

        return true;
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public override byte[] Serialize() {
        var result = base.Serialize();

        if ( motions == null ) {
            return result.Concat(BitConverter.GetBytes((int)0)).ToArray();
        }

        // モーション数
        result = result.Concat(BitConverter.GetBytes(motions.Length)).ToArray();

        // モーションデータ
        for ( int i = 0 ; i < motions.Length ; ++i ) {
            var type = EasyMotion2D.GetSerializedType(motions[i].GetType());
            result = result
                .Concat(BitConverter.GetBytes((int)type))
                .Concat(motions[i].Serialize()).ToArray();
        }

        return result;
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes">シリアライズ済みモーションデータ</param>
    /// <param name="offset">モーションデータの開始位置</param>
    /// <returns>デシリアライズに使用したバイトサイズにoffsetを加算した値</returns>
    public override int Deserialize(byte[] bytes, int offset) {
        offset = base.Deserialize(bytes, offset);

        // モーション数
        var motionNum = BitConverter.ToInt32(bytes, offset);
        offset += sizeof(int);

        // モーションデータ
        motions = new MotionBase2D[motionNum];
        for ( int i = 0 ; i < motionNum ; ++i ) {
            var type = (EasyMotion2D.MotionType)BitConverter.ToInt32(bytes, offset);
            offset += sizeof(int);
            motions[i] = EasyMotion2D.GetDeserializedMotion(type, bytes, offset, out offset);
        }

        return offset;
    }

    /// <summary>
    /// 現在の向き
    /// </summary>
    public override float currentDirection {
        get {
            return curAngle;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void DrawGUI() {
        base.DrawGUI();

        if ( motions == null ) {
            return;
        }

        for ( int i = 0 ; i < motions.Length ; ++i ) {
            // ボタン表示
            GUILayout.BeginHorizontal();

            if ( GUILayout.Button("Up", GUILayout.Width(60)) ) {
                OnUp(i);
            }
            if ( GUILayout.Button("Down", GUILayout.Width(60)) ) {
                OnDown(i);
            }
            if ( GUILayout.Button("Insert New", GUILayout.Width(80)) ) {
                OnInsertNew(i);
            }
            if ( GUILayout.Button("Remove", GUILayout.Width(80)) ) {
                OnRemove(i--);
                GUILayout.EndHorizontal();
                continue;
            }

            GUILayout.EndHorizontal();

            // 各モーションのGUI描画
            ++UnityEditor.EditorGUI.indentLevel;

            var currentType = EasyMotion2D.GetSerializedType(motions[i].GetType());
            var newType = (EasyMotion2D.MotionType)UnityEditor.EditorGUILayout.EnumPopup("Motion Type", currentType);
            if ( newType == EasyMotion2D.MotionType.Sequence ) {
                newType = currentType;
            }

            if ( newType != currentType ) {
                motions[i] = EasyMotion2D.CreateInstance(newType);
            }
            motions[i].DrawGUI();

            --UnityEditor.EditorGUI.indentLevel;
        }

        // 末尾への新規追加ボタン表示
        if ( GUILayout.Button("Add New", GUILayout.Width(80)) ) {
            OnInsertNew(motions.Length);
        }
    }

    /// <summary>
    /// 上移動ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnUp(int index) {
        if ( index == 0 ) {
            return;
        }

        Replace(index, index - 1);
    }

    /// <summary>
    /// 下移動ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnDown(int index) {
        if ( index >= motions.Length - 1 ) {
            return;
        }

        Replace(index, index + 1);
    }

    /// <summary>
    /// 挿入ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnInsertNew(int index) {
        var newMotions = new List<MotionBase2D>(motions);
        newMotions.Insert(index, EasyMotion2D.CreateInstance(EasyMotion2D.MotionType.Stop));
        motions = newMotions.ToArray();
    }

    /// <summary>
    /// 削除ボタンがクリックされた
    /// </summary>
    /// <param name="index">モーションのインデックス</param>
    private void OnRemove(int index) {
        var newMotions = new List<MotionBase2D>(motions);
        newMotions.RemoveAt(index);
        motions = newMotions.ToArray();
    }

    /// <summary>
    /// モーションを入れ替える
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    private void Replace(int index1, int index2) {
        var tmp = motions[index1];
        motions[index1] = motions[index2];
        motions[index2] = tmp;
    }

    /// <summary>
    /// Gizmoを描画する
    /// </summary>
    /// <param name="from">現在位置</param>
    /// <returns>移動後の位置</returns>
    public override Vector2 DrawGizmos(Vector2 from) {
        if ( motions == null ) {
            return initPosition;
        }

        // 各モーションのGizmoを描画
        foreach ( var motion in motions ) {
            // Gizmoの描画
            from = motion.DrawGizmos(transform, from);
        }

        return from;
    }

    /// <summary>
    /// 速さ取得
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <returns>設定された速さ</returns>
    public override float GetSpeed(Vector2 from) {
        if ( motions.Length == 0 ) {
            return 0;
        }

        float avgSpeed = 0;

        drawGizmos = false;
        foreach ( var motion in motions ) {
            var to = motion.DrawGizmos(from);
            avgSpeed += motion.GetSpeed(from);
            from = to;
        }
        drawGizmos = true;

        return avgSpeed / motions.Length;
    }

    /// <summary>
    /// 速さ設定
    /// </summary>
    /// <param name="from">開始位置</param>
    /// <param name="speed">速さ</param>
    public override void SetSpeed(Vector2 from, float speed) {
        drawGizmos = false;
        foreach ( var motion in motions ) {
            var to = motion.DrawGizmos(from);
            motion.SetSpeed(from, speed);
            from = to;
        }
        drawGizmos = true;
    }
#endif
}
