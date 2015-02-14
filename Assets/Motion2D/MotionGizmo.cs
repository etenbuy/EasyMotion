///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionGizmo.cs                                                                   //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.14                                                                       //
//  Desc    :   Motion関連のGizmo表示をまとめたクラス。                                          //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

/// <summary>
/// Motion関連のGizmo表示をまとめたクラス。
/// </summary>
public class MotionGizmo {
    public static Color EditorColor = Color.cyan;
    public static Color MovingColor = Color.magenta;
    public static Color DisableColor = Color.gray;

    /// <summary>
    /// 矢印の描画
    /// </summary>
    /// <param name="points"></param>
    /// <param name="color"></param>
    /// <param name="showStartCap"></param>
    /// <param name="showEndCap"></param>
    public static void DrawArrow(Vector2[] points, Color color, bool showStartCap = false, bool showEndCap = true) {
        if ( points.Length < 2 ) {
            // 線が存在しなければ何もしない
            return;
        }

        // 矢印の矢の角度計算
        float startCapAngle = 0;
        float endCapAngle = 0;

        if ( showStartCap ) {
            var dir = points[0] - points[1];
            startCapAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        if ( showEndCap ) {
            var dir = points[points.Length - 1] - points[points.Length - 2];
            endCapAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }

        // 矢印の矢の描画
        DrawArrow(points, color, showStartCap, showEndCap, startCapAngle, endCapAngle);
    }

    /// <summary>
    /// 矢印の描画
    /// </summary>
    /// <param name="points"></param>
    /// <param name="color"></param>
    /// <param name="showStartCap"></param>
    /// <param name="showEndCap"></param>
    /// <param name="startCapAngle"></param>
    /// <param name="endCapAngle"></param>
    public static void DrawArrow(Vector2[] points, Color color, bool showStartCap, bool showEndCap, float startCapAngle, float endCapAngle) {
        if ( points.Length < 2 ) {
            // 線が存在しなければ何もしない
            return;
        }

        // 始点の設定
        var from = points[0];

        // 線の色設定
        Gizmos.color = color;

        // 線の描画
        foreach ( var point in points ) {
            Gizmos.DrawLine(from, point);
            from = point;
        }

        // 矢印の矢の描画
        if ( showStartCap ) {
            DrawArrowCap(points[0], startCapAngle, color);
        }
        if ( showEndCap ) {
            DrawArrowCap(from, endCapAngle, color);
        }
    }

    /// <summary>
    /// 矢印の矢の部分を描画する
    /// </summary>
    /// <param name="from"></param>
    /// <param name="angle"></param>
    /// <param name="color"></param>
    public static void DrawArrowCap(Vector2 from, float angle, Color color) {
        var scale = CameraScale;

        // メッシュ設定
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[3] {
            new Vector3( 0,-10) * scale,
            new Vector3( 0, 10) * scale,
            new Vector3(20,  0) * scale,
        };

        mesh.triangles = new int[] {
            0, 1, 2
        };

        mesh.normals = new Vector3[] {
            Vector3.forward,
            Vector3.forward,
            Vector3.forward,
        };

        mesh.colors = new Color[] {
            color,
            color,
            color,
        };

        // メッシュ描画
        Graphics.DrawMeshNow(mesh, from, Quaternion.Euler(0, 0, angle));

        Object.DestroyImmediate(mesh);
    }

    public static float CameraScale {
        get {
            var sceneCamera = SceneView.lastActiveSceneView.camera;
            return sceneCamera.orthographicSize / sceneCamera.pixelHeight;
        }
    }
}

#endif