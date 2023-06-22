using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 大砲ギミックの管理クラス
/// </summary>
public class GimmickArtilleryController : MonoBehaviour
{
    [Header("参照関連")]
    [SerializeField, Tooltip("オブジェクトプール")] private ObjectPoolController _objectPoolController = default;
    [SerializeField, Tooltip("オーディオ管理クラス")] private AudioManager _audioManager = default;

    [Header("弾のパッケージ関連")]
    [SerializeField] private List<ArtilleryPackage> _artilleryPackage = new List<ArtilleryPackage>();
    [Serializable, Tooltip("弾のパッケージ")]
    public class ArtilleryPackage
    {
        [field: SerializeField, Tooltip("弾の発射位置")] private Transform _artilleryShotTransform;
        [field: SerializeField, Tooltip("発射までのインターバル"), Range(0.1f, 10f)] private float _shotInterval;
        [field: SerializeField, Tooltip("発射後のインターバル"), Range(0.1f, 10f)] private float _nextShotInterval;
        [field: SerializeField, Tooltip("弾速"), Range(0.1f, 100f)] private float _shotSpeed;

        /// <summary>
        /// 弾の発射位置
        /// </summary>
        public Transform ArtilleryShotTransform { get { return _artilleryShotTransform; } }
        /// <summary>
        /// 発射までのインターバル
        /// </summary>
        public float ShotInterval { get { return _shotInterval; } }
        /// <summary>
        /// 発射後のインターバル
        /// </summary>
        public float NextShotInterval { get { return _nextShotInterval; } }
        /// <summary>
        /// 弾速
        /// </summary>
        public float ShotSpeed { get { return _shotSpeed; } }
    }

    private enum ShotAnimType
    {
        [InspectorName("発射")] SHOOT,
        [InspectorName("発射")] RELOAD,
    }



    //処理部---------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        try
        {

            //発射開始
            for (int i = 0; i < _artilleryPackage.Count; i++) { StartCoroutine(ShotInterval(i)); }
        }
        catch { print("GimmickArtilleryControllerエラー"); }
    }



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /* [ コルーチン関連 ] */
    /// <summary>
    /// 発射までのインターバル
    /// </summary>
    /// <param name="listNumber"></param>
    /// <returns></returns>
    private IEnumerator ShotInterval(int listNumber)
    {

        //待機
        yield return new WaitForSeconds(_artilleryPackage[listNumber].ShotInterval);

        try
        {

            //発射オーディオ
            _audioManager.StartAudio(2, 0);

            //オブジェクトプールから弾を生成と配置
            GameObject shotArrow = _objectPoolController.GetSetObj(_artilleryPackage[listNumber].ArtilleryShotTransform);
            //弾の発射
            shotArrow.GetComponent<Rigidbody>().velocity = _artilleryPackage[listNumber].ArtilleryShotTransform.forward * _artilleryPackage[listNumber].ShotSpeed;
            //AudioManagerの設定
            shotArrow.GetComponent<GimmickArtilleryBall>().SetAudioManager(_audioManager);

            //発射後のインターバル
            StartCoroutine(NextShotInterval(listNumber));
        }
        catch { print("GimmickArtilleryControllerエラー"); }
    }
    /// <summary>
    /// 発射後のインターバル
    /// </summary>
    /// <param name="listNumber"></param>
    /// <returns></returns>
    private IEnumerator NextShotInterval(int listNumber)
    {

        //待機
        yield return new WaitForSeconds(_artilleryPackage[listNumber].NextShotInterval);

        try
        {

            //発射までのインターバル
            StartCoroutine(ShotInterval(listNumber));
        }
        catch { print("GimmickArtilleryControllerエラー"); }
    }
}
