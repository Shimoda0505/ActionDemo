using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// Audio管理クラス
/// </summary>
public class AudioManager : MonoBehaviour
{

    [Header("BGMのパッケージ関連")]
    [field: SerializeField, Tooltip("オーディオソース")] private AudioSource _bgmAudioSource;
    [field: SerializeField, Tooltip("ボリューム"), Range(0f, 1f)] private float _bgmVolume;
    private float _masterBgmVolume = 1;//マスターボリュームBgm


    [Header("SEのパッケージ関連")]
    public List<SePackage> _sePackages = new List<SePackage>();
    [Serializable, Tooltip("オーディオのパッケージ")]
    public class SePackage
    {
        [SerializeField, Tooltip("名前")] private string _name;
        [field: SerializeField, Tooltip("オーディオソース")] private AudioSource _audioSource;
        [field: SerializeField, Tooltip("ボリューム"), Range(0f, 1f)] private float _volume;
        [field: SerializeField, Tooltip("音源")] private AudioClip[] _audioClips;

        public AudioSource AudioSource { get { return _audioSource; } }
        public float Volume { get { return _volume; } }
        public AudioClip[] AudioClips { get { return _audioClips; } }
    }
    private float _masterSeVolume = 1;//マスターボリュームSe



    //処理部---------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //Bgm音量変更
        _bgmAudioSource.volume = _bgmVolume * _masterBgmVolume;

        //Se音量変更
        for (int i = 0; i < _sePackages.Count; i++) { _sePackages[i].AudioSource.volume = _sePackages[i].Volume * _masterSeVolume; }
    }



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Seの再生
    /// </summary>
    public void StartAudio(int packageNumber, int audioNumber)
    {

        try
        {

            //音源音再生
            _sePackages[packageNumber].AudioSource.PlayOneShot(_sePackages[packageNumber].AudioClips[audioNumber]);
            print(packageNumber + "番の" + audioNumber + "番音源が再生");
        }
        catch { print("StartAudioで" + packageNumber + "番の" + audioNumber + "番音源がエラー"); }
    }
    /// <summary>
    /// SEのマスターボリュームの変更
    /// </summary>
    public void ChangeSeVolume(float vol)
    {

        //マスターボリュームの変更
        _masterSeVolume = vol;
        //音量変更
        for (int i = 0; i < _sePackages.Count; i++) { _sePackages[i].AudioSource.volume = _sePackages[i].Volume * _masterSeVolume; }
    }
    /// <summary>
    /// BGMのマスターボリュームの変更
    /// </summary>
    public void ChangeBgmVolume(float vol)
    {

        //マスターボリュームの変更
        _masterBgmVolume = vol;
        //音量変更
        _bgmAudioSource.volume = _bgmVolume * _masterBgmVolume;
    }
}
