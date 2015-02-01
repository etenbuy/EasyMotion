///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   CurveMotionEditor.cs                                                             //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   旋回モーションエディタ拡張。                                                     //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;

/// <summary>
/// 旋回モーションエディタ拡張
/// </summary>
[CustomEditor(typeof(CurveMotion))]
public class CurveMotionEditor : Editor {
    /// <summary>
    /// CurveMotionのインスペクタ上のレイアウト
    /// </summary>
    public override void OnInspectorGUI() {
        serializedObject.Update();

        var fromCurrent = serializedObject.FindProperty("fromCurrent");
        EditorGUILayout.PropertyField(serializedObject.FindProperty("fromCurrent"));

        EditorGUI.BeginDisabledGroup(fromCurrent.boolValue);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("from"));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("fromAngle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateAngle"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("radius"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("delay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));

        serializedObject.ApplyModifiedProperties();
    }
}
