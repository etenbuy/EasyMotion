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
        if ( Application.isPlaying ) {
            return;
        }
        var script = target as EasyMotion2D;
        script.motion = EasyMotion2D.GetDeserializedMotion(script.type, script.serializedMotion);
        script.rotation = RotationBase2D.GetDeserializedRotation(script.rotationType, script.serializedRotation);
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


        // セパレータ描画
        EditorGUILayout.Space();


        // 回転動作型選択描画
        var currentRotType = script.rotationType;
        var newRotType = script.rotationType = (RotationBase2D.RotationType)EditorGUILayout.EnumPopup("Rotation Type", currentRotType);

        var rotation = script.rotation;
        if ( rotation == null ) {
            rotation = RotationBase2D.GetDeserializedRotation(currentRotType, script.serializedRotation);
        }

        if ( newRotType != currentRotType ) {
            // 回転動作型が変更された
            rotation = RotationBase2D.CreateInstance(newRotType);
            script.serializedRotation = rotation.Serialize();

            script.rotation = rotation;
        }

        // 回転動作回転動作設定用のGUI描画
        rotation.DrawGUI();


        EditorGUI.EndDisabledGroup();

        if ( GUI.changed ) {
            // 値が編集されたらシリアライズデータを更新
            script.serializedMotion = motion.Serialize();
            script.serializedRotation = script.rotation.Serialize();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
