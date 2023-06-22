using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// ��C�M�~�b�N�̊Ǘ��N���X
/// </summary>
public class GimmickArtilleryController : MonoBehaviour
{
    [Header("�Q�Ɗ֘A")]
    [SerializeField, Tooltip("�I�u�W�F�N�g�v�[��")] private ObjectPoolController _objectPoolController = default;
    [SerializeField, Tooltip("�I�[�f�B�I�Ǘ��N���X")] private AudioManager _audioManager = default;

    [Header("�e�̃p�b�P�[�W�֘A")]
    [SerializeField] private List<ArtilleryPackage> _artilleryPackage = new List<ArtilleryPackage>();
    [Serializable, Tooltip("�e�̃p�b�P�[�W")]
    public class ArtilleryPackage
    {
        [field: SerializeField, Tooltip("�e�̔��ˈʒu")] private Transform _artilleryShotTransform;
        [field: SerializeField, Tooltip("���˂܂ł̃C���^�[�o��"), Range(0.1f, 10f)] private float _shotInterval;
        [field: SerializeField, Tooltip("���ˌ�̃C���^�[�o��"), Range(0.1f, 10f)] private float _nextShotInterval;
        [field: SerializeField, Tooltip("�e��"), Range(0.1f, 100f)] private float _shotSpeed;

        /// <summary>
        /// �e�̔��ˈʒu
        /// </summary>
        public Transform ArtilleryShotTransform { get { return _artilleryShotTransform; } }
        /// <summary>
        /// ���˂܂ł̃C���^�[�o��
        /// </summary>
        public float ShotInterval { get { return _shotInterval; } }
        /// <summary>
        /// ���ˌ�̃C���^�[�o��
        /// </summary>
        public float NextShotInterval { get { return _nextShotInterval; } }
        /// <summary>
        /// �e��
        /// </summary>
        public float ShotSpeed { get { return _shotSpeed; } }
    }

    private enum ShotAnimType
    {
        [InspectorName("����")] SHOOT,
        [InspectorName("����")] RELOAD,
    }



    //������---------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        try
        {

            //���ˊJ�n
            for (int i = 0; i < _artilleryPackage.Count; i++) { StartCoroutine(ShotInterval(i)); }
        }
        catch { print("GimmickArtilleryController�G���["); }
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
        yield return new WaitForSeconds(_artilleryPackage[listNumber].ShotInterval);

        try
        {

            //���˃I�[�f�B�I
            _audioManager.StartAudio(2, 0);

            //�I�u�W�F�N�g�v�[������e�𐶐��Ɣz�u
            GameObject shotArrow = _objectPoolController.GetSetObj(_artilleryPackage[listNumber].ArtilleryShotTransform);
            //�e�̔���
            shotArrow.GetComponent<Rigidbody>().velocity = _artilleryPackage[listNumber].ArtilleryShotTransform.forward * _artilleryPackage[listNumber].ShotSpeed;
            //AudioManager�̐ݒ�
            shotArrow.GetComponent<GimmickArtilleryBall>().SetAudioManager(_audioManager);

            //���ˌ�̃C���^�[�o��
            StartCoroutine(NextShotInterval(listNumber));
        }
        catch { print("GimmickArtilleryController�G���["); }
    }
    /// <summary>
    /// ���ˌ�̃C���^�[�o��
    /// </summary>
    /// <param name="listNumber"></param>
    /// <returns></returns>
    private IEnumerator NextShotInterval(int listNumber)
    {

        //�ҋ@
        yield return new WaitForSeconds(_artilleryPackage[listNumber].NextShotInterval);

        try
        {

            //���˂܂ł̃C���^�[�o��
            StartCoroutine(ShotInterval(listNumber));
        }
        catch { print("GimmickArtilleryController�G���["); }
    }
}
