///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerMotionEditor.cs                                                             //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   直線モーションエディタ拡張。                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;

/// <summary>
/// 直線モーションエディタ拡張
/// </summary>
[CustomEditor(typeof(LinerMotion))]
public class LinerMotionEditor : Editor {
    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake() {
        SceneView.onSceneGUIDelegate += OnSceneView;
    }

    /// <summary>
    /// 後処理
    /// </summary>
    private void OnDestroy() {
        SceneView.onSceneGUIDelegate -= OnSceneView;
    }

    /// <summary>
    /// LinerMotionのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        var fromCurrent = serializedObject.FindProperty("fromCurrent");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fromCurrent"));

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("from"));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("to"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// 軌跡の描画
    /// </summary>
    /// <param name="sceneView"></param>
    private void OnSceneView(SceneView sceneView) {
        Handles.BeginGUI();

        var from = HandleUtility.WorldToGUIPoint(serializedObject.FindProperty("from").vector2Value);
        var to = HandleUtility.WorldToGUIPoint(serializedObject.FindProperty("to").vector2Value);

        Handles.DrawLine(from, to);

        Handles.EndGUI();
    }
}
