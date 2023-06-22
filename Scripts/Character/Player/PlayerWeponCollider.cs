using UnityEngine;
using System.Collections;



public class PlayerWeponCollider : MonoBehaviour
{

    
    private const string _playerTag = "Player";//�v���C���[�̃^�O
    private int _attack = default;//�U����
    private float _hitInterval = default;//�A���q�b�g�����Ȃ����߂̃C���^�[�o��
    private GameObject _hitObj = default;//�q�b�g�����Ώ�


    //���\�b�h��----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// ����̗L��,�U���͂̕ύX
    /// </summary>
    public void ChangeCollider(bool isCollider, int attack)
    {

        try
        {

            //�U���͂̕ύX
            _attack = attack;

            //���薳��
            if (!isCollider) { gameObject.GetComponent<Collider>().enabled = false; }
            //����L��
            else if (isCollider) { gameObject.GetComponent<Collider>().enabled = true; }
        }
        catch { print("PlayerWeponCollider�ŃG���["); }
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {

            //�Փː悪�v���C���[�Ȃ珈�����Ȃ�
            if (other.gameObject.tag == _playerTag) { return; }
            //�O��Ɠ����ΏۂȂ珈�����Ȃ�
            if (_hitObj != null) { if (_hitObj == other.gameObject) { return; } }

            //CharacterState��ێ����Ă���I�u�W�F�N�g�Ȃ珈��
            if (other.gameObject.GetComponent<CharacterState>())
            {

                print("�v���C���[��" + other.gameObject + "��" + _attack + "�̃_���[�W");

                //�U������
                other.gameObject.GetComponent<CharacterState>().Damage(_attack);
                //�q�b�g�����Ώ�
                _hitObj = other.gameObject;
            }
        }
        catch { print("PlayerWeponCollider�ŃG���["); }
    }

    public IEnumerator EndAttack()
    {

        //�ҋ@
        yield return new WaitForSeconds(_hitInterval);

        //�q�b�g�����Ώ�
        _hitObj = null;
    }
}
