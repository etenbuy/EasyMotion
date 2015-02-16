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
[System.Serializable]
public class MotionBase2D {
    /// <summary>
    /// ���[�V�������J�n����܂ł̎���
    /// </summary>
    public float delay;

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
        behav.StartCoroutine(ExecuteMotion(behav.transform));
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
    /// ���[�V���������s����R���[�`��
    /// </summary>
    /// <param name="trans">���[�V�������ʂ𔽉f����Ώۂ̃g�����X�t�H�[��</param>
    /// <returns></returns>
    private IEnumerator ExecuteMotion(Transform trans) {
        // �J�n�܂ł̈�莞�ԑҋ@
        if ( delay != 0 ) {
            yield return new WaitForSeconds(delay);
        }

        // ���[�V�������s
        if ( OnStart() ) {
            while ( true ) {
                var nextUpdate = OnUpdate();
                // �ʒu�X�V
                trans.localPosition = position;
                yield return 0;

                if ( !nextUpdate ) {
                    break;
                }
            }
        }
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
    /// <param name="bytes"></param>
    /// <returns>�f�V���A���C�Y�Ɏg�p�����o�C�g�T�C�Y</returns>
    public virtual int Deserialize(byte[] bytes) {
        delay = BitConverter.ToSingle(bytes, 0);
        return sizeof(float);
    }
}
