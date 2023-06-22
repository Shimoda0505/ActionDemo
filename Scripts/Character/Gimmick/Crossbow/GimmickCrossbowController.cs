using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;



/// <summary>
/// �N���X�{�E�̋����Ǘ��N���X
/// </summary>
public class GimmickCrossbowController : MonoBehaviour
{

    [Header("�Q�Ɗ֘A")]
    [SerializeField, Tooltip("�I�u�W�F�N�g�v�[��")] private ObjectPoolController _objectPoolController = default;
    [SerializeField, Tooltip("�I�[�f�B�I�Ǘ��N���X")] private AudioManager _audioManager = default;

    [Header("�N���X�{�E�̃p�b�P�[�W�֘A")]
    [SerializeField]private List<CrossbowPackage> _crossbowPackage = new List<CrossbowPackage>();
    [Serializable, Tooltip("�N���X�{�E�̃p�b�P�[�W")]
    public class CrossbowPackage
    {
        [field: SerializeField, Tooltip("�N���X�{�E�̃I�u�W�F�N�g")] private GameObject _crossbowObj;
        [field: SerializeField, Tooltip("�N���X�{�E�̔��ˈʒu")] private Transform _crossbowShotTransform;
        [field: SerializeField, Tooltip("���˂܂ł̃C���^�[�o��"),Range(0.1f,10f)] private float _shotInterval;
        [field: SerializeField, Tooltip("���ˌ�̃C���^�[�o��"), Range(0.1f, 10f)] private float _nextShotInterval;
        [field: SerializeField, Tooltip("�e��"), Range(0.1f, 100f)] private float _shotSpeed;
        [NonSerialized]private Animator _anim;//�e�X��Animator  


        /// <summary>
        /// �N���X�{�E�̃I�u�W�F�N�g
        /// </summary>
        public GameObject CrossbowObj { get { return _crossbowObj; } }
        /// <summary>
        /// �N���X�{�E�̔��ˈʒu
        /// </summary>
        public Transform CrossbowShotTransform { get { return _crossbowShotTransform; }  }
        /// <summary>
        /// ���˂܂ł̃C���^�[�o��
        /// </summary>
        public float ShotInterval { get { return _shotInterval; } }
        /// <summary>
        /// ���ˌ�̃C���^�[�o��
        /// </summary>
        public float NextShotInterval { get { return _nextShotInterval; }}
        /// <summary>
        /// �e��
        /// </summary>
        public float ShotSpeed { get { return _shotSpeed; }}
        /// <summary>
        /// �e�X��Animator
        /// </summary>
        public Animator Anim { get { return _anim; } set { _anim = value; } }
    }

    private enum ShotAnimType
    {
        [InspectorName("����")]SHOOT,
        [InspectorName("����")] RELOAD,
    }

    private GameObject obj;

    //������---------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        try
        {
               
            for (int i = 0; i < _crossbowPackage.Count; i++)
            {

                //�e�X��Animator��ݒ�
                _crossbowPackage[i].Anim = _crossbowPackage[i].CrossbowObj.GetComponent<Animator>();

                //���ˊJ�n
                StartCoroutine(ShotInterval(i));
            }
        }
        catch { print("GimmickCrossbowController�G���["); }
    }




    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /* [ �R���[�`���֘A ] */
    /// <summary>
    /// ���˂܂ł̃C���^�[�o��
    /// </summary>
    /// <param name="listNumber"></param>
    /// <returns></returns>
    private IEnumerator ShotInterval(int listNumber)
    {

        //�ҋ@
        yield return new WaitForSeconds(_crossbowPackage[listNumber].ShotInterval);

        try
        {

            //���˃A�j���[�V����
            _crossbowPackage[listNumber].Anim.SetTrigger(ShotAnimType.SHOOT.ToString());
            //���˃I�[�f�B�I
            _audioManager.StartAudio(0, 0);

            //�I�u�W�F�N�g�v�[������e�𐶐��Ɣz�u
            GameObject shotArrow = _objectPoolController.GetSetObj(_crossbowPackage[listNumber].CrossbowShotTransform);
            //�e�̔���
            shotArrow.GetComponent<Rigidbody>().velocity = _crossbowPackage[listNumber].CrossbowShotTransform.forward * _crossbowPackage[listNumber].ShotSpeed;

            //���ˌ�̃C���^�[�o��
            StartCoroutine(NextShotInterval(listNumber));
        }
        catch { print("GimmickCrossbowController�G���["); }
    }
    /// <summary>
    /// ���ˌ�̃C���^�[�o��
    /// </summary>
    /// <param name="listNumber"></param>
    /// <returns></returns>
    private IEnumerator NextShotInterval(int listNumber)
    {

        //�ҋ@
        yield return new WaitForSeconds(_crossbowPackage[listNumber].NextShotInterval);

        try
        {

            //�����[�h�A�j���[�V����
            _crossbowPackage[listNumber].Anim.SetTrigger(ShotAnimType.RELOAD.ToString());
            //���˂܂ł̃C���^�[�o��
            StartCoroutine(ShotInterval(listNumber));
        }
        catch { print("GimmickCrossbowController�G���["); }
    }
}
