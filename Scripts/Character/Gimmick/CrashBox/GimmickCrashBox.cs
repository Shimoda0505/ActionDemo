using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 壊れる箱の管理クラス
/// </summary>
public class GimmickCrashBox : GimmickState
{

    [Header("参照関連")]
    [SerializeField, Tooltip("オーディオ管理クラス")] private AudioManager _audioManager = default;

    [Header("箱のコンポーネント関連")]
    private Rigidbody _thisRb = default;//本体のRigidbody
    private Vector3 _thisPos = default;//本体の初期位置
    private Quaternion _thisRotate = default;//本体の初期角度
    private GameObject _boxChild = default;//子オブジェクト
    private Collider _boxCollider = default;//BoxCollider
    private MeshRenderer _meshRenderer = default;//MeshRenderer

    [Header("壊れた箱のコンポーネント関連")]
    private List<GameObject> _crachBoxObj = new List<GameObject>();//壊れたボックスのオブジェクト
    private List<Collider> _crachBoxCollider = new List<Collider>();//壊れたボックスのコライダー
    private List<Rigidbody> _crachBoxRb = new List<Rigidbody>();//壊れたボックスの物理
    private List<Vector3> _crachBoxPos = new List<Vector3>();//壊れたボックスの初期位置
    private List<Quaternion> _crachBoxRotate = new List<Quaternion>();//壊れたボックスの初期角度

    [Header("ステータス関連")]
    [SerializeField, Tooltip("元に戻るまでのインターバル"), Range(1f, 10f)] public int _reverseInterval;
    [SerializeField, Tooltip("破片の物理をなくすまでのインターバル"), Range(0.1f, 10f)] public float _falseRbInterval;
    private bool _isCrach = default;//壊れている最中



    //処理部---------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected override void Start()
    {

        //基底
        base.Start();
        try
        {

            //コンポーネント取得
            _boxChild = gameObject.transform.GetChild(0).gameObject;//子オブジェクト
            _boxCollider = gameObject.GetComponent<Collider>();//Collider
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();//MeshRenderer

            //本体
            _thisRb = gameObject.GetComponent<Rigidbody>();//Rigidbody
            _thisPos = gameObject.transform.position;//初期位置
            _thisRotate = gameObject.transform.rotation;//初期角度

            //壊れたときの破片オブジェクトの数を取得
            int childCount = _boxChild.transform.childCount;
            //壊れたときの破片オブジェクトを順に取得する
            for (int j = 0; j < childCount; j++)
            {

                //全て格納
                GameObject childObject = _boxChild.transform.GetChild(j).gameObject;//壊れたときの破片
                _crachBoxObj.Add(childObject);//壊れたときの破片のオブジェクト
                _crachBoxCollider.Add(childObject.GetComponent<Collider>());//壊れたボックスのコライダー
                _crachBoxRb.Add(childObject.GetComponent<Rigidbody>());//壊れたボックスの物理
                _crachBoxPos.Add(childObject.transform.position);//壊れたときの破片の座標
                _crachBoxRotate.Add(childObject.transform.rotation);//壊れたときの破片の角度
            }
        }
        catch { print("GimmickCrashBoxControllerでエラー"); }
    }


    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    public override void Damage(int attack)
    {

        //壊れている最中は処理しない
        if (_isCrach) { return; }

        //基底
         base.Damage(attack);


        if (_motionEnum == MotionEnum.DEATH)
        {

            //本体の物理終了
            _thisRb.isKinematic = true;

            //描画、判定を消す
            _boxCollider.enabled = false;
            _meshRenderer.enabled = false;
            //壊れている最中
            _isCrach = true;

            //壊れるボックスを表示
            _boxChild.SetActive(true);
            //破壊オーディオ
            _audioManager.StartAudio(1, 0);

            //破片の物理をなくすまでのインターバル
            StartCoroutine("FalseRb");
            //元に戻るまでのインターバル開始
            StartCoroutine("ReverseBox");
        }
    }

    /* [ コルーチン関連 ] */
    /// <summary>
    /// 破片の物理をなくすまでのインターバル
    /// </summary>
    private IEnumerator FalseRb()
    {

        //待機
        yield return new WaitForSeconds(_falseRbInterval);

        try
        {

            for (int i = 0; i < _crachBoxObj.Count; i++)
            {

                //コライダーを非アクティブ
                _crachBoxCollider[i].enabled = false;
                //物理挙動を非アクティブ
                _crachBoxRb[i].isKinematic = true;
            }
        }
        catch { print("GimmickCrashBoxControllerでエラー"); }
    }
    /// <summary>
    /// 元に戻るまでのインターバル
    /// </summary>
    private IEnumerator ReverseBox()
    {

        //待機
        yield return new WaitForSeconds(_reverseInterval);

        try
        {

            //本体の初期設定
            gameObject.transform.position = _thisPos;
            gameObject.transform.rotation = _thisRotate;

            for (int i = 0; i < _crachBoxObj.Count; i++)
            {

                //座標角度を元に戻す
                _crachBoxObj[i].transform.position = _crachBoxPos[i];
                _crachBoxObj[i].transform.rotation = _crachBoxRotate[i];
                //コライダーをアクティブ
                _crachBoxCollider[i].enabled = true;
                //物理挙動をアクティブ
                _crachBoxRb[i].isKinematic = false;
            }

            //本体の初期設定
            _thisRb.isKinematic = false;

            //壊れるボックスを表示
            _boxChild.SetActive(false);

            //描画、判定を消す
            _boxCollider.enabled = true;
            _meshRenderer.enabled = true;
            //壊れてい ない
            _isCrach = false;

            //ステータスの初期化
            ResetState();
        }
        catch { print("GimmickCrashBoxControllerでエラー"); }
    }
}
