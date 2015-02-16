///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   EasyMotion2DEditor.cs                                                            //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   EasyMotion2Dクラスのエディタ拡張。                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// EasyMotion2Dクラスのエディタ拡張。
/// </summary>
[CustomEditor(typeof(EasyMotion2D))]
public class EasyMotion2DEditor : Editor {
    /// <summary>
    /// 初期化。
    /// </summary>
    public void Awake() {
        var type = (EasyMotion2D.MotionType)serializedObject.FindProperty("type").enumValueIndex;

        var script = target as EasyMotion2D;
        script.motion = EasyMotion2D.GetDeserializedMotion(type, script.serializedMotion);
    }

    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(Application.isPlaying);

        var script = target as EasyMotion2D;

        // モーション型選択描画
        var currentType = script.type;
        var newType = script.type = (EasyMotion2D.MotionType)EditorGUILayout.EnumPopup("Motion Type", currentType);

        var motion = script.motion;
        if ( motion == null ) {
            motion = EasyMotion2D.GetDeserializedMotion(currentType, script.serializedMotion);
        }

        if ( newType != currentType ) {
            // モーション型が変更された
            motion = EasyMotion2D.CreateInstance(newType);
            script.serializedMotion = motion.Serialize();

            script.motion = motion;
        }

        // モーション設定用のGUI描画
        motion.DrawGUI();

        EditorGUI.EndDisabledGroup();

        if ( GUI.changed ) {
            // 値が編集されたらシリアライズデータを更新
            script.serializedMotion = motion.Serialize();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
