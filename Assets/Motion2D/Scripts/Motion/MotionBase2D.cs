///////////////////////////////////////////////////////////////////////////////////////////////////
//                                                                                               //
//  File    :   MotionBase2D.cs                                                                  //
//  Author  :   ftvoid                                                                           //
//  Date    :   2015.02.15                                                                       //
//  Desc    :   2D���[�V�������B                                                               //
//                                                                                               //
///////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 2D���[�V�������B
/// </summary>
[Serializable]
public class MotionBase2D {
    /// <summary>
    /// ���[�V�������J�n����܂ł̎���
    /// </summary>
    public float delay;

    /// <summary>
    /// ���݈ʒu
    /// </summary>
    public Vector2 position;

    /// <summary>
    /// �����ʒu
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
    /// ���s���̃��[�V������Ԓ�`
    /// </summary>
    private enum State {
        Disable,
        Waiting,
        Running,
    }

    /// <summary>
    /// ���s���̃��[�V�������
    /// </summary>
    private State state = State.Disable;

    /// <summary>
    /// GameObject��Transform
    /// </summary>
    protected Transform transform;

    /// <summary>
    /// �J�n����
    /// </summary>
    private float startTime;

    /// <summary>
    /// OnStart()�͌Ăяo���ꂽ���ǂ���
    /// </summary>
    private bool onStartCalled = false;

    /// <summary>
    /// ���[�V�����̎��s���J�n����B
    /// </summary>
    /// <param name="objTrans">GameObject��Transform</param>
    public void StartMotion(Transform objTrans) {
        state = delay > 0 ? State.Waiting : State.Running;
        transform = objTrans.transform;
        startTime = Time.time;
        position = initPosition = transform.localPosition;
    }

    /// <summary>
    /// ���[�V�����̏�Ԃ��X�V����
    /// <param name="updateTransform"></param>
    /// </summary>
    public bool UpdateMotion(bool updateTransform = true) {
        switch ( state ) {
        case State.Waiting:
            // �J�n�܂ł̈�莞�ԑҋ@
            if ( Time.time - startTime >= delay ) {
                // ���[�V�������s�ɑJ��
                state = State.Running;
            }
            return true;

        case State.Running:
            // ���[�V�������s
            if ( !onStartCalled ) {
                onStartCalled = true;
                if ( !OnStart() ) {
                    return false;
                }
            }

            // �X�V����
            var nextUpdate = OnUpdate();

            if ( updateTransform ) {
                // �ʒu�X�V
                transform.position = new Vector3(position.x, position.y, transform.localPosition.z);
            }

            if ( !nextUpdate ) {
                // ���[�V�����I���Ȃ疳����ԂɑJ��
                state = State.Disable;
            }

            return nextUpdate;
        }

        return false;
    }

    /// <summary>
    /// ���[�V�����̏���������(�h���N���X�Ŏ�������)
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected virtual bool OnStart() {
        return false;
    }

    /// <summary>
    /// ���[�V�����̍X�V����(�h���N���X�Ŏ�������)
    /// </summary>
    /// <returns>true:���[�V�����p�� / false:�ȍ~�̃��[�V�������p�����Ȃ�</returns>
    protected virtual bool OnUpdate() {
        return false;
    }

    /// <summary>
    /// �V���A���C�Y
    /// </summary>
    /// <returns>�V���A���C�Y���ꂽ�o�C�i���z��</returns>
    public virtual byte[] Serialize() {
        var result = BitConverter.GetBytes(delay);
        return result;
    }

    /// <summary>
    /// �f�V���A���C�Y
    /// </summary>
    /// <param name="bytes">�V���A���C�Y�ς݃��[�V�����f�[�^</param>
    /// <param name="offset">���[�V�����f�[�^�̊J�n�ʒu</param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y��offset�����Z�����l</returns>
    public virtual int Deserialize(byte[] bytes, int offset) {
        delay = BitConverter.ToSingle(bytes, offset);
        return offset + sizeof(float);
    }

    /// <summary>
    /// ���݂̌���
    /// </summary>
    public float direction {
        get {
            if ( state != State.Running ) {
                return NO_DIRECTION;
            }
            return currentDirection;
        }
    }

    /// <summary>
    /// ���������݂��Ȃ����Ƃ�\���萔
    /// </summary>
    public const float NO_DIRECTION = float.MaxValue;

    /// <summary>
    /// ���݂̌���(�h���N���X�Ŏ�������)
    /// </summary>
    public virtual float currentDirection {
        get {
            return NO_DIRECTION;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// �G�f�B�^�ɕ\������Gizmo�F
    /// </summary>
    private static Color editorColor = Color.cyan;

    /// <summary>
    /// ���[�V���������s���Ă��Ȃ��Ƃ��̐F
    /// </summary>
    private static Color disableColor = Color.gray;

    /// <summary>
    /// ���[�V�������s�܂őҋ@���̐F
    /// </summary>
    private static Color waitingColor = Color.blue;

    /// <summary>
    /// ���[�V�������s���̐F
    /// </summary>
    private static Color runningColor = Color.magenta;

    /// <summary>
    /// �C���X�y�N�^���GUI��`�悷��
    /// </summary>
    public virtual void DrawGUI() {
        delay = UnityEditor.EditorGUILayout.FloatField("Delay", delay);
    }

    /// <summary>
    /// Gizmo��`�悷��(Editor����̌Ăяo���p)
    /// </summary>
    /// <param name="trans"></param>
    public void DrawGizmos(Transform trans) {
        if ( !Application.isPlaying ) {
            position = trans.localPosition;
        }
        DrawGizmos(initPosition);
    }

    /// <summary>
    /// Gizmo��`�悷��(�h���N���X�Ŏ�������)
    /// </summary>
    /// <param name="from">���݈ʒu</param>
    /// <returns>�ړ���̈ʒu</returns>
    public virtual Vector2 DrawGizmos(Vector2 from) {
        return from;
    }

    /// <summary>
    /// Gizmo�\���F
    /// </summary>
    private Color gizmoColor {
        get {
            if ( !Application.isPlaying ) {
                // ����s���̓G�f�B�^�F��Ԃ�
                return editorColor;
            }

            // �e���s��Ԃɉ������F��Ԃ�
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
    /// Gizmo��`�悷�邩�ǂ���
    /// </summary>
    protected static bool drawGizmos = true;

    /// <summary>
    /// ����`�悷��
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    protected void DrawLine(Vector2 from, Vector2 to) {
        if ( !drawGizmos ) {
            return;
        }

        Gizmos.color = gizmoColor;
        Gizmos.DrawLine(from, to);
    }

    /// <summary>
    /// ����`�悷��
    /// </summary>
    /// <param name="points"></param>
    protected void DrawLine(Vector2[] points) {
        if ( !drawGizmos ) {
            return;
        }

        Gizmos.color = gizmoColor;
        for ( int i = 1 ; i < points.Length ; ++i ) {
            Gizmos.DrawLine(points[i - 1], points[i]);
        }
    }

    /// <summary>
    /// ���̖�̕�����`�悷��
    /// </summary>
    /// <param name="from"></param>
    /// <param name="angle"></param>
    /// <param name="color"></param>
    protected void DrawArrowCap(Vector2 from, float angle) {
        if ( !drawGizmos ) {
            return;
        }

        var scale = CameraScale;

        // ���b�V���ݒ�
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

        // ���b�V���`��
        Graphics.DrawMeshNow(mesh, from, Quaternion.Euler(0, 0, angle));

        UnityEngine.Object.DestroyImmediate(mesh);
    }

    /// <summary>
    /// �J�����X�P�[��
    /// </summary>
    public static float CameraScale {
        get {
            var sceneCamera = UnityEditor.SceneView.lastActiveSceneView.camera;
            return sceneCamera.orthographicSize / sceneCamera.pixelHeight;
        }
    }

    /// <summary>
    /// �����擾
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <returns>�ݒ肳�ꂽ����</returns>
    public virtual float GetSpeed(Vector2 from) {
        return 0;
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="from">�J�n�ʒu</param>
    /// <param name="speed">����</param>
    public virtual void SetSpeed(Vector2 from, float speed) {
    }
#endif
}