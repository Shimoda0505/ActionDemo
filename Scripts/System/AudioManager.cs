using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Audio�Ǘ��N���X
/// </summary>
public class AudioManager : MonoBehaviour
{

    [Header("BGM�̃p�b�P�[�W�֘A")]
    [field: SerializeField, Tooltip("�I�[�f�B�I�\�[�X")] private AudioSource _bgmAudioSource;
    [field: SerializeField, Tooltip("�{�����[��"), Range(0f, 1f)] private float _bgmVolume;
    private float _masterBgmVolume = 1;//�}�X�^�[�{�����[��Bgm


    [Header("SE�̃p�b�P�[�W�֘A")]
    public List<SePackage> _sePackages = new List<SePackage>();
    [Serializable, Tooltip("�I�[�f�B�I�̃p�b�P�[�W")]
    public class SePackage
    {
        [SerializeField, Tooltip("���O")] private string _name;
        [field: SerializeField, Tooltip("�I�[�f�B�I�\�[�X")] private AudioSource _audioSource;
        [field: SerializeField, Tooltip("�{�����[��"), Range(0f, 1f)] private float _volume;
        [field: SerializeField, Tooltip("����")] private AudioClip[] _audioClips;

        public AudioSource AudioSource { get { return _audioSource; } }
        public float Volume { get { return _volume; } }
        public AudioClip[] AudioClips { get { return _audioClips; } }
    }
    private float _masterSeVolume = 1;//�}�X�^�[�{�����[��Se



    //������---------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //Bgm���ʕύX
        _bgmAudioSource.volume = _bgmVolume * _masterBgmVolume;

        //Se���ʕύX
        for (int i = 0; i < _sePackages.Count; i++) { _sePackages[i].AudioSource.volume = _sePackages[i].Volume * _masterSeVolume; }
    }



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Se�̍Đ�
    /// </summary>
    public void StartAudio(int packageNumber, int audioNumber)
    {

        try
        {

            //�������Đ�
            _sePackages[packageNumber].AudioSource.PlayOneShot(_sePackages[packageNumber].AudioClips[audioNumber]);
            print(packageNumber + "�Ԃ�" + audioNumber + "�ԉ������Đ�");
        }
        catch { print("StartAudio��" + packageNumber + "�Ԃ�" + audioNumber + "�ԉ������G���["); }
    }
    /// <summary>
    /// SE�̃}�X�^�[�{�����[���̕ύX
    /// </summary>
    public void ChangeSeVolume(float vol)
    {

        //�}�X�^�[�{�����[���̕ύX
        _masterSeVolume = vol;
        //���ʕύX
        for (int i = 0; i < _sePackages.Count; i++) { _sePackages[i].AudioSource.volume = _sePackages[i].Volume * _masterSeVolume; }
    }
    /// <summary>
    /// BGM�̃}�X�^�[�{�����[���̕ύX
    /// </summary>
    public void ChangeBgmVolume(float vol)
    {

        //�}�X�^�[�{�����[���̕ύX
        _masterBgmVolume = vol;
        //���ʕύX
        _bgmAudioSource.volume = _bgmVolume * _masterBgmVolume;
    }
}
