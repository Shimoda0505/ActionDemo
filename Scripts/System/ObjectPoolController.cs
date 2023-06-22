using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �I�u�W�F�N�g�v�[��
/// </summary>
public class ObjectPoolController : MonoBehaviour
{


    [Header("�I�u�W�F�N�g�v�[���֘A")]
    [SerializeField, Tooltip("�v�[���ɐ����������I�u�W�F�N�g����")] private GameObject _poolObj;
    [SerializeField, Tooltip("�ŏ��ɐ�������I�u�W�F�N�g�̍ő吔 ")] private int _maxPool;
    private List<GameObject> _poolObjList = new List<GameObject>();//���������I�u�W�F�N�g�p�̃��X�g



    //������-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {

        //�ŏ��ɌŒ萔�v���n�u�I�u�W�F�N�g���C���X�^���g
        //�I�u�W�F�N�g���ő吔�܂Ő���
        for (int i = 0; i < _maxPool; i++)
        {

            // �I�u�W�F�N�g�𐶐�����
            GameObject newObj = CreateNewObj();
            //���������I�u�W�F�N�g�̕���������false
            newObj.SetActive(false);
            // ���X�g�ɕۑ����Ă���
            _poolObjList.Add(newObj);
        }
    }



    //���\�b�h��----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //�p�u���b�N���\�b�h---------------------------------------------------
    /// <summary>
    /// �I�u�W�F�N�g�̔z�u�ƕԋp
    /// </summary>
    /// <param name="tr"></param>
    /// <returns></returns>
    public GameObject GetSetObj(Transform tr)
    {

        //�I�u�W�F�N�g�v�[������e�𐶐�
        GameObject obj = GetObj();

        //�I�u�W�F�N�g�̔z�u
        obj.transform.position = tr.position;//�ʒu
        obj.transform.rotation = tr.rotation;//�p�x

        //�Ăяo�����̃N���X�ɂ��̃I�u�W�F�N�g��Ԃ�
        return obj;
    }
    /// <summary>
    /// �I�u�W�F�N�g�v�[������g�p����v���n�u�I�u�W�F�N�g���擾
    /// </summary>
    /// <returns>�v���n�u�I�u�W�F�N�g</returns>
    public GameObject GetObj()
    {

        //���g�p������Ύg�p,�Ȃ���ΐ���
        //�g�p���łȂ����̂�T���ĕԂ�
        //���X�g���ɂ���I�u�W�F�N�g��obj�Ƃ��ĕԂ�
        foreach (var obj in _poolObjList)
        {

            //�I�u�W�F�N�g����A�N�e�B�u�̂��̂�T��
            if (!obj.activeSelf)
            {

                //�I�u�W�F�N�g���A�N�e�B�u
                obj.SetActive(true);

                //�Ăяo�����̃N���X�ɂ��̃I�u�W�F�N�g��Ԃ�
                return obj;
            }
        }

        // �S�Ďg�p����������V�������
        GameObject newObj = CreateNewObj();

        //���X�g�ɕۑ����Ă���
        _poolObjList.Add(newObj);
        //�V����������I�u�W�F�N�g���A�N�e�B�u
        newObj.SetActive(true);

        //�Ăяo�����̃N���X�ɂ��̃I�u�W�F�N�g��Ԃ�
        return newObj;
    }

    //�v���C�x�[�g���\�b�h---------------------------------------------------
    /// <summary>
    /// �V�����v���n�u�I�u�W�F�N�g���C���X�^���g
    /// </summary>
    /// <returns>�V�����v���n�u�I�u�W�F�N�g</returns>
    private GameObject CreateNewObj()
    {

        //��ʊO�ɐ���
        Vector3 pos = this.gameObject.transform.position;
        //�V�����I�u�W�F�N�g�𐶐�(�����������I�u�W�F�N�g��,��ʊO��,�e�ɂȂ�I�u�W�F�N�g�Ɠ�������)
        GameObject newObj = Instantiate(_poolObj, pos, Quaternion.identity);
        //���O�ɘA�ԕt��(���X�g�ɒǉ����ꂽ��)
        newObj.name = _poolObj.name + (_poolObjList.Count + 1);

        //�Ăяo�����̃X�N���v�g�ɂ��̃I�u�W�F�N�g��Ԃ�
        return newObj;
    }
}
