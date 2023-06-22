using System.Collections;
using UnityEngine;



/// <summary>
/// クロスボウの矢の挙動管理クラス
/// </summary>
public class GimmickCrossbowArrow : GimmickState
{

    [Header("コンポーネント関連")]
    private Rigidbody _rb = default;//Rigidbody
    private CapsuleCollider _colllider = default;//CapsuleCollider


    [Header("ステータス関連")]
    [SerializeField, Tooltip("消えるまでの時間")] private float _activeInterval= default;
    private bool _isHit = false;//衝突フラグ


    //処理部---------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected override void Start()
    {

        //基底
        base.Start();

        try
        {

            //コンポーネント取得
            _rb = this.gameObject.GetComponent<Rigidbody>();
            _colllider = this.gameObject.GetComponent<CapsuleCollider>();
        }
        catch { print("GimmickCrossbowArrowでエラー"); }
    }



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {

        try
        {

            //1度衝突したなら処理しない
            if (_isHit) { return; }

            //衝突フラグ
            _isHit = true;

            //物理挙動終了
            _rb.isKinematic = true;
            //コライダーを非アクティブ
            _colllider.enabled = false;
            //衝突対象と親子関係を構築
            transform.parent = other.gameObject.transform;

            //衝突対象がプレイヤーなら攻撃
            if (other.gameObject.tag == _playerTag)
            {

                print("弓矢がプレイヤーに"  + _attack + "のダメージ");
                _playerManager.Damage(_attack); 
            }

            //時間経過後非アクティブ
            StartCoroutine("ActiveInterval");
        }
        catch { print("GimmickCrossbowArrowでエラー"); }
    }

    /// <summary>
    /// 時間経過後非アクティブ
    /// </summary>
    /// <returns></returns>
    private IEnumerator ActiveInterval()
    {

        //待機
        yield return new WaitForSeconds(_activeInterval);

        try
        {

            //衝突フラグを初期化
            _isHit = false;

            //物理挙動開始
            _rb.isKinematic = false;
            //コライダーをアクティブ
            _colllider.enabled = true;
            //親子関係を解除
            transform.parent = null;
            //このオブジェクトを非アクティブ
            this.gameObject.SetActive(false);
        }
        catch { print("GimmickCrossbowArrowでエラー"); }
    }
}
