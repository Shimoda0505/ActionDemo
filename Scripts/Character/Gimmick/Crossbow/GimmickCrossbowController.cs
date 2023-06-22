using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;



/// <summary>
/// クロスボウの挙動管理クラス
/// </summary>
public class GimmickCrossbowController : MonoBehaviour
{

    [Header("参照関連")]
    [SerializeField, Tooltip("オブジェクトプール")] private ObjectPoolController _objectPoolController = default;
    [SerializeField, Tooltip("オーディオ管理クラス")] private AudioManager _audioManager = default;

    [Header("クロスボウのパッケージ関連")]
    [SerializeField]private List<CrossbowPackage> _crossbowPackage = new List<CrossbowPackage>();
    [Serializable, Tooltip("クロスボウのパッケージ")]
    public class CrossbowPackage
    {
        [field: SerializeField, Tooltip("クロスボウのオブジェクト")] private GameObject _crossbowObj;
        [field: SerializeField, Tooltip("クロスボウの発射位置")] private Transform _crossbowShotTransform;
        [field: SerializeField, Tooltip("発射までのインターバル"),Range(0.1f,10f)] private float _shotInterval;
        [field: SerializeField, Tooltip("発射後のインターバル"), Range(0.1f, 10f)] private float _nextShotInterval;
        [field: SerializeField, Tooltip("弾速"), Range(0.1f, 100f)] private float _shotSpeed;
        [NonSerialized]private Animator _anim;//各々のAnimator  


        /// <summary>
        /// クロスボウのオブジェクト
        /// </summary>
        public GameObject CrossbowObj { get { return _crossbowObj; } }
        /// <summary>
        /// クロスボウの発射位置
        /// </summary>
        public Transform CrossbowShotTransform { get { return _crossbowShotTransform; }  }
        /// <summary>
        /// 発射までのインターバル
        /// </summary>
        public float ShotInterval { get { return _shotInterval; } }
        /// <summary>
        /// 発射後のインターバル
        /// </summary>
        public float NextShotInterval { get { return _nextShotInterval; }}
        /// <summary>
        /// 弾速
        /// </summary>
        public float ShotSpeed { get { return _shotSpeed; }}
        /// <summary>
        /// 各々のAnimator
        /// </summary>
        public Animator Anim { get { return _anim; } set { _anim = value; } }
    }

    private enum ShotAnimType
    {
        [InspectorName("発射")]SHOOT,
        [InspectorName("発射")] RELOAD,
    }

    private GameObject obj;

    //処理部---------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        try
        {
               
            for (int i = 0; i < _crossbowPackage.Count; i++)
            {

                //各々のAnimatorを設定
                _crossbowPackage[i].Anim = _crossbowPackage[i].CrossbowObj.GetComponent<Animator>();

                //発射開始
                StartCoroutine(ShotInterval(i));
            }
        }
        catch { print("GimmickCrossbowControllerエラー"); }
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
        yield return new WaitForSeconds(_crossbowPackage[listNumber].ShotInterval);

        try
        {

            //発射アニメーション
            _crossbowPackage[listNumber].Anim.SetTrigger(ShotAnimType.SHOOT.ToString());
            //発射オーディオ
            _audioManager.StartAudio(0, 0);

            //オブジェクトプールから弾を生成と配置
            GameObject shotArrow = _objectPoolController.GetSetObj(_crossbowPackage[listNumber].CrossbowShotTransform);
            //弾の発射
            shotArrow.GetComponent<Rigidbody>().velocity = _crossbowPackage[listNumber].CrossbowShotTransform.forward * _crossbowPackage[listNumber].ShotSpeed;

            //発射後のインターバル
            StartCoroutine(NextShotInterval(listNumber));
        }
        catch { print("GimmickCrossbowControllerエラー"); }
    }
    /// <summary>
    /// 発射後のインターバル
    /// </summary>
    /// <param name="listNumber"></param>
    /// <returns></returns>
    private IEnumerator NextShotInterval(int listNumber)
    {

        //待機
        yield return new WaitForSeconds(_crossbowPackage[listNumber].NextShotInterval);

        try
        {

            //リロードアニメーション
            _crossbowPackage[listNumber].Anim.SetTrigger(ShotAnimType.RELOAD.ToString());
            //発射までのインターバル
            StartCoroutine(ShotInterval(listNumber));
        }
        catch { print("GimmickCrossbowControllerエラー"); }
    }
}
