///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   LinerMotionEditor.cs                                                             //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   直線モーションエディタ拡張。                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEngine;

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
    /// シーン上の描画
    /// </summary>
    /// <param name="sceneView"></param>
    private void OnSceneView(SceneView sceneView) {
        //var com = target as LinerMotion;

        //Handles.BeginGUI();

        //Vector3 from = serializedObject.FindProperty("from").vector2Value;
        //Vector3 to = serializedObject.FindProperty("to").vector2Value;
        ////from.z = com.transform.localPosition.z;
        ////to.z = com.transform.localPosition.z;

        //var sceneCamera = SceneView.currentDrawingSceneView.camera;

        //Handles.DrawLine(from, to);

        //Handles.EndGUI();
    }
}
