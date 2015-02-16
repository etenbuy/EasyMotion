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
    private delegate void DrawGUI(MotionBase2D motion);
    private static Dictionary<EasyMotion2D.MotionType, DrawGUI> drawGui = new Dictionary<EasyMotion2D.MotionType, DrawGUI>() {
        { EasyMotion2D.MotionType.Stop, MotionBase2DEditor.DrawGUI },
        { EasyMotion2D.MotionType.MoveTo, MoveTo2DEditor.DrawGUI },
    };

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

        // モーションの各GUI描画
        drawGui[newType](motion);

        // TODO 削除
        EditorGUILayout.PropertyField(serializedObject.FindProperty("serializedMotion"), true);

        EditorGUI.EndDisabledGroup();

        if ( GUI.changed ) {
            script.serializedMotion = motion.Serialize();
            script.UpdateDeserializedMotion();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
