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

/// <summary>
/// 2D���[�V�������B
/// </summary>
[System.Serializable]
public class MotionBase2D {
    /// <summary>
    /// ���[�V�������J�n����܂ł̎���
    /// </summary>
    [SerializeField]
    private float delay = 0;

    /// <summary>
    /// ���݈ʒu
    /// </summary>
    public Vector2 position { get; protected set; }

    /// <summary>
    /// �����ʒu
    /// </summary>
    protected Vector2 initPosition { get; private set; }

    /// <summary>
    /// ���[�V�����̎��s���J�n����B
    /// </summary>
    /// <param name="behav">�X�N���v�g</param>
    public void StartMotion(MonoBehaviour behav) {
        position = initPosition = behav.transform.localPosition;
        behav.StartCoroutine(ExecuteMotion());
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

    private IEnumerator ExecuteMotion() {
        // �J�n�܂ł̈�莞�ԑҋ@
        if ( delay != 0 ) {
            yield return new WaitForSeconds(delay);
        }

        // ���[�V�������s
        if ( OnStart() ) {
            while ( OnUpdate() ) {
                yield return 0;
            }
        }
    }
}
