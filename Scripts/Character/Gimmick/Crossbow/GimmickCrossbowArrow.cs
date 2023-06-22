using System.Collections;
using UnityEngine;



/// <summary>
/// �N���X�{�E�̖�̋����Ǘ��N���X
/// </summary>
public class GimmickCrossbowArrow : GimmickState
{

    [Header("�R���|�[�l���g�֘A")]
    private Rigidbody _rb = default;//Rigidbody
    private CapsuleCollider _colllider = default;//CapsuleCollider


    [Header("�X�e�[�^�X�֘A")]
    [SerializeField, Tooltip("������܂ł̎���")] private float _activeInterval= default;
    private bool _isHit = false;//�Փ˃t���O


    //������---------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected override void Start()
    {

        //���
        base.Start();

        try
        {

            //�R���|�[�l���g�擾
            _rb = this.gameObject.GetComponent<Rigidbody>();
            _colllider = this.gameObject.GetComponent<CapsuleCollider>();
        }
        catch { print("GimmickCrossbowArrow�ŃG���["); }
    }



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {

        try
        {

            //1�x�Փ˂����Ȃ珈�����Ȃ�
            if (_isHit) { return; }

            //�Փ˃t���O
            _isHit = true;

            //���������I��
            _rb.isKinematic = true;
            //�R���C�_�[���A�N�e�B�u
            _colllider.enabled = false;
            //�ՓˑΏۂƐe�q�֌W���\�z
            transform.parent = other.gameObject.transform;

            //�ՓˑΏۂ��v���C���[�Ȃ�U��
            if (other.gameObject.tag == _playerTag)
            {

                print("�|��v���C���[��"  + _attack + "�̃_���[�W");
                _playerManager.Damage(_attack); 
            }

            //���Ԍo�ߌ��A�N�e�B�u
            StartCoroutine("ActiveInterval");
        }
        catch { print("GimmickCrossbowArrow�ŃG���["); }
    }

    /// <summary>
    /// ���Ԍo�ߌ��A�N�e�B�u
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActiveInterval()
    {

        //�ҋ@
        yield return new WaitForSeconds(_activeInterval);

        try
        {

            //�Փ˃t���O��������
            _isHit = false;

            //���������J�n
            _rb.isKinematic = false;
            //�R���C�_�[���A�N�e�B�u
            _colllider.enabled = true;
            //�e�q�֌W������
            transform.parent = null;
            //���̃I�u�W�F�N�g���A�N�e�B�u
            this.gameObject.SetActive(false);
        }
        catch { print("GimmickCrossbowArrow�ŃG���["); }
    }
}
