///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionBase2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.01                                                                       //
//  Desc    :   2Dモーション基底クラスを定義する。                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

/// <summary>
/// 2Dモーション基底クラス
/// </summary>
public class MotionBase2D : MonoBehaviour {
    /// <summary>
    /// 自身のtransform
    /// </summary>
    private Transform selfTrans;

    /// <summary>
    /// 初期化
    /// </summary>
    protected void Awake() {
        selfTrans = transform;
    }

    /// <summary>
    /// 直線移動
    /// </summary>
    /// <param name="from">始点</param>
    /// <param name="to">終点</param>
    /// <param name="delay">モーション開始までの時間</param>
    /// <param name="duration">モーション時間</param>
    /// <returns></returns>
    protected IEnumerator Line(Vector2 from, Vector2 to, float delay, float duration) {
        var startTime = Time.time + delay;
        var endTime = startTime + duration;

        // 開始まで待機
        while ( Time.time < startTime ) {
            yield return 0;
        }

        // 直線モーション実行
        while ( Time.time < endTime ) {
            // 現在位置計算
            var progress = (Time.time - startTime) / duration;
            var current = (1 - progress) * from + progress * to;

            // 位置反映
            Position2D = current;

            yield return 0;
        }

        // 終了
        Position2D = to;
    }

    /// <summary>
    /// 旋回移動
    /// </summary>
    /// <param name="from">始点</param>
    /// <param name="fromAngle">初角度</param>
    /// <param name="rotateAngle">旋回角度</param>
    /// <param name="radius">旋回半径</param>
    /// <param name="delay">モーション開始までの時間</param>
    /// <param name="duration">モーション時間</param>
    /// <returns></returns>
    protected IEnumerator Curve(Vector2 from, float fromAngle, float rotateAngle, float radius, float delay, float duration) {
        var startTime = Time.time + delay;
        var endTime = startTime + duration;

        // 開始まで待機
        while ( Time.time < startTime ) {
            yield return 0;
        }

        if ( rotateAngle < 0 ) {
            // 右旋回の場合
            fromAngle += 180;
        }

        // 旋回モーション実行
        rotateAngle *= Mathf.Deg2Rad;
        fromAngle *= Mathf.Deg2Rad;

        var fromSin = Mathf.Sin(fromAngle);
        var fromCos = Mathf.Cos(fromAngle);

        while ( Time.time < endTime ) {
            // 現在位置計算
            var progress = (Time.time - startTime) / duration;
            var curAngle = fromAngle + progress * rotateAngle;

            Position2D = from + new Vector2(
                -fromSin + Mathf.Sin(curAngle),
                fromCos - Mathf.Cos(curAngle)
            ) * radius;

            yield return 0;
        }

        // 終了
        var toAngle = fromAngle + rotateAngle;
        Position2D = from + new Vector2(
            -fromSin + Mathf.Sin(toAngle),
            fromCos - Mathf.Cos(toAngle)
        ) * radius;
    }

    /// <summary>
    /// 位置座標(本クラスより位置情報にアクセスする際に使用する)
    /// </summary>
    protected Vector2 Position2D {
        get {
            return (Vector2)selfTrans.localPosition;
        }
        set {
            selfTrans.localPosition = new Vector3(
                value.x,
                value.y,
                selfTrans.localPosition.z
            );
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// 直線矢印を描画する
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="fromCurrent"></param>
    protected void DrawLineArrow(Vector2 from, Vector2 to, bool fromCurrent) {
        Gizmos.color = Color.cyan;

        // 始点計算
        var fromPos = (fromCurrent && !Application.isPlaying) ? (Vector2)transform.localPosition : from;

        // 直線の描画
        Gizmos.DrawLine(fromPos, to);

        // 矢印の描画
        var dir = to - fromPos;
        DrawArrowCap(to, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    /// <summary>
    /// 円弧矢印を描画する
    /// </summary>
    /// <param name="from"></param>
    /// <param name="fromAngle"></param>
    /// <param name="rotateAngle"></param>
    /// <param name="radius"></param>
    /// <param name="fromCurrent"></param>
    protected void DrawArcArrow(Vector2 from, float fromAngle, float rotateAngle, float radius, bool fromCurrent) {
        Gizmos.color = Color.cyan;

        // 円の頂点数
        const int POINT_NUM = 45;

        // 角度情報
        var fromAngleRad = fromAngle * Mathf.Deg2Rad;
        var isRight = rotateAngle < 0;
        if ( isRight ) {
            // 右旋回の場合
            fromAngleRad -= Mathf.PI;
        }

        var fromSin = Mathf.Sin(fromAngleRad);
        var fromCos = Mathf.Cos(fromAngleRad);
        var rotateAngleRad = rotateAngle * Mathf.Deg2Rad;
        var toAngleRad = fromAngleRad + rotateAngleRad;

        if ( isRight ) {
            var tmp = toAngleRad;
            toAngleRad = fromAngleRad;
            fromAngleRad = tmp;
        }

        // 始点計算
        var fromPos = (fromCurrent && !Application.isPlaying) ? (Vector2)transform.localPosition : from;

        // 軌跡の描画
        for ( int i = 0 ; i < POINT_NUM ; ++i ) {
            var curAngle = 2 * Mathf.PI * i / POINT_NUM + fromAngleRad;
            var nextAngle = 2 * Mathf.PI * (i + 1) / POINT_NUM + fromAngleRad;

            // 描画範囲外の角度なら何もしない
            if ( curAngle > toAngleRad ) {
                continue;
            }

            // 円弧の端のずれ補正
            if ( nextAngle > toAngleRad ) {
                nextAngle = toAngleRad;
            }

            // 円弧の描画
            Gizmos.DrawLine(
                fromPos + new Vector2(-fromSin + Mathf.Sin(curAngle), fromCos - Mathf.Cos(curAngle)) * radius,
                fromPos + new Vector2(-fromSin + Mathf.Sin(nextAngle), fromCos - Mathf.Cos(nextAngle)) * radius);
        }

        // 矢印の描画
        if ( isRight ) {
            var toPos = fromPos + new Vector2(-fromSin + Mathf.Sin(fromAngleRad), fromCos - Mathf.Cos(fromAngleRad)) * radius;
            DrawArrowCap(toPos, fromAngleRad * Mathf.Rad2Deg + 180);
        } else {
            var toPos = fromPos + new Vector2(-fromSin + Mathf.Sin(toAngleRad), fromCos - Mathf.Cos(toAngleRad)) * radius;
            DrawArrowCap(toPos, toAngleRad * Mathf.Rad2Deg);
        }
    }

    /// <summary>
    /// 矢印の矢の部分の描画
    /// </summary>
    /// <param name="from"></param>
    /// <param name="angle"></param>
    protected void DrawArrowCap(Vector2 from, float angle) {
        var sceneCamera = UnityEditor.SceneView.lastActiveSceneView.camera;
        var scale = sceneCamera.orthographicSize / sceneCamera.pixelHeight;

        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[3] {
            new Vector3( 0, -10) * scale,
            new Vector3( 0,  10) * scale,
            new Vector3( 20, 0 ) * scale,
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
            Color.cyan,
            Color.cyan,
            Color.cyan,
        };

        Graphics.DrawMeshNow(mesh, from, Quaternion.Euler(0, 0, angle));

        DestroyImmediate(mesh);
    }
#endif
}
