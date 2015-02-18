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
#if UNITY_EDITOR
    private Vector2 initPos;
    protected Vector2 initPosition {
        get {
            if ( Application.isPlaying ) {
                return initPos;
            } else {
                return position;
            }
        }
        set {
            initPos = value;
        }
    }
#else
    protected Vector2 initPosition { get; private set; }
#endif

    /// <summary>
    /// モーションの実行を開始する。
    /// </summary>
    /// <param name="behav">スクリプト</param>
    public void StartMotion(MonoBehaviour behav) {
#if UNITY_EDITOR
        state = State.Disable;
#endif
        initPosition = behav.transform.localPosition;
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
#if UNITY_EDITOR
        state = State.Waiting;
#endif

        // 開始までの一定時間待機
        if ( delay != 0 ) {
            yield return new WaitForSeconds(delay);
        }

        // モーション実行
        if ( OnStart() ) {
#if UNITY_EDITOR
            state = State.Running;
#endif
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
#if UNITY_EDITOR
        state = State.Disable;
#endif
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
    /// 実行時のモーション状態定義
    /// </summary>
    private enum State {
        Disable,
        Waiting,
        Running,
    }

    /// <summary>
    /// 実行時のモーション状態
    /// </summary>
    private State state = State.Disable;

    /// <summary>
    /// エディタに表示するGizmo色
    /// </summary>
    private static Color editorColor = Color.cyan;

    /// <summary>
    /// モーションを実行していないときの色
    /// </summary>
    private static Color disableColor = Color.gray;

    /// <summary>
    /// モーション実行まで待機中の色
    /// </summary>
    private static Color waitingColor = Color.blue;

    /// <summary>
    /// モーション実行中の色
    /// </summary>
    private static Color runningColor = Color.magenta;

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
        if ( !Application.isPlaying ) {
            position = trans.localPosition;
        }
        DrawGizmos();
    }

    /// <summary>
    /// Gizmoを描画する
    /// </summary>
    protected virtual void DrawGizmos() {
    }

    /// <summary>
    /// Gizmo表示色
    /// </summary>
    private Color gizmoColor {
        get {
            if ( !Application.isPlaying ) {
                // 非実行時はエディタ色を返す
                return editorColor;
            }

            // 各実行状態に応じた色を返す
            switch ( state ) {
            case State.Disable:
                return disableColor;
            case State.Waiting:
                return waitingColor;
            case State.Running:
                return runningColor;
            }

            return disableColor;
        }
    }

    /// <summary>
    /// 線を描画する
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    protected void DrawLine(Vector2 from, Vector2 to) {
        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(from, to);
    }

    /// <summary>
    /// 矢印の矢の部分を描画する
    /// </summary>
    /// <param name="from"></param>
    /// <param name="angle"></param>
    /// <param name="color"></param>
    protected void DrawArrowCap(Vector2 from, float angle) {
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

        var color = gizmoColor;
        mesh.colors = new Color[] {
            color,
            color,
            color,
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
