///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionBase2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   2Dモーション基底。                                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 2Dモーション基底。
/// </summary>
[System.Serializable]
public class MotionBase2D {
    /// <summary>
    /// モーションを開始するまでの時間
    /// </summary>
    public float delay;

    /// <summary>
    /// 現在位置
    /// </summary>
    public Vector2 position { get; protected set; }

    /// <summary>
    /// 初期位置
    /// </summary>
    protected Vector2 initPosition { get; private set; }

    /// <summary>
    /// モーションの実行を開始する。
    /// </summary>
    /// <param name="behav">スクリプト</param>
    public void StartMotion(MonoBehaviour behav) {
        position = initPosition = behav.transform.localPosition;
        behav.StartCoroutine(ExecuteMotion(behav.transform));
    }

    /// <summary>
    /// モーションの初期化処理(派生クラスで実装する)
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected virtual bool OnStart() {
        return false;
    }

    /// <summary>
    /// モーションの更新処理(派生クラスで実装する)
    /// </summary>
    /// <returns>true:モーション継続 / false:以降のモーションを継続しない</returns>
    protected virtual bool OnUpdate() {
        return false;
    }

    /// <summary>
    /// モーションを実行するコルーチン
    /// </summary>
    /// <param name="trans">モーション結果を反映する対象のトランスフォーム</param>
    /// <returns></returns>
    private IEnumerator ExecuteMotion(Transform trans) {
        // 開始までの一定時間待機
        if ( delay != 0 ) {
            yield return new WaitForSeconds(delay);
        }

        // モーション実行
        if ( OnStart() ) {
            while ( true ) {
                var nextUpdate = OnUpdate();
                // 位置更新
                trans.localPosition = position;
                yield return 0;

                if ( !nextUpdate ) {
                    break;
                }
            }
        }
    }

    /// <summary>
    /// シリアライズ
    /// </summary>
    /// <returns>シリアライズされたバイナリ配列</returns>
    public virtual byte[] Serialize() {
        var result = BitConverter.GetBytes(delay);
        return result;
    }

    /// <summary>
    /// デシリアライズ
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns>デシリアライズに使用したバイトサイズ</returns>
    public virtual int Deserialize(byte[] bytes) {
        delay = BitConverter.ToSingle(bytes, 0);
        return sizeof(float);
    }

#if UNITY_EDITOR
    /// <summary>
    /// インスペクタ上のGUIを描画する
    /// </summary>
    public virtual void DrawGUI() {
        delay = UnityEditor.EditorGUILayout.FloatField("Delay", delay);
    }

    /// <summary>
    /// Gizmoを描画する(Editorからの呼び出し用)
    /// </summary>
    /// <param name="trans"></param>
    public void DrawGizmos(Transform trans) {
        initPosition = position = trans.localPosition;
        DrawGizmos();
    }

    /// <summary>
    /// Gizmoを描画する
    /// </summary>
    protected virtual void DrawGizmos() {
    }

    private static Color editorColor = Color.cyan;

    /// <summary>
    /// 線を描画する
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    protected static void DrawLine(Vector2 from, Vector2 to) {
        Gizmos.color = editorColor;
        Gizmos.DrawLine(from, to);
    }

    /// <summary>
    /// 矢印の矢の部分を描画する
    /// </summary>
    /// <param name="from"></param>
    /// <param name="angle"></param>
    /// <param name="color"></param>
    protected static void DrawArrowCap(Vector2 from, float angle) {
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
            editorColor,
            editorColor,
            editorColor,
        };

        // メッシュ描画
        Graphics.DrawMeshNow(mesh, from, Quaternion.Euler(0, 0, angle));

        UnityEngine.Object.DestroyImmediate(mesh);
    }

    /// <summary>
    /// カメラスケール
    /// </summary>
    public static float CameraScale {
        get {
            var sceneCamera = UnityEditor.SceneView.lastActiveSceneView.camera;
            return sceneCamera.orthographicSize / sceneCamera.pixelHeight;
        }
    }
#endif
}
