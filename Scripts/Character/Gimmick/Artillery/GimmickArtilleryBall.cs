using System.Collections;
using UnityEngine;



public class GimmickArtilleryBall : GimmickState
{

    [Header("コンポーネント関連")]
    private Rigidbody _rb = default;//Rigidbody
    private SphereCollider _colllider = default;//CapsuleCollider
    private MeshRenderer _meshRenderer = default;//MeshRenderer
    private AudioManager _audioManager = default;//オーディオ管理クラス


    [Header("アクティブ関連")]
    [SerializeField, Tooltip("消えるまでの時間")] private float _activeInterval = default;
    [SerializeField, Tooltip("拡大後のコライダー")] private float _scaleMax = default;
    [SerializeField, Tooltip("爆発Effect")] private GameObject _exObj = default;
    private float _scaleDef = default;//デフォルトサイズのコライダー


    //処理部---------------------------------------------------------------------------------------------------------------------------------------------------------------
    protected override void Start()
    {

        //基底
        base.Start();


        //コンポーネント取得
        _rb = this.gameObject.GetComponent<Rigidbody>();
        _colllider = this.gameObject.GetComponent<SphereCollider>();
        _meshRenderer = this.gameObject.GetComponent<MeshRenderer>();

        //デフォルトサイズのコライダーを補完
        _scaleDef = _colllider.radius;
        //爆発Effectを非アクティブ
        _exObj.SetActive(false);
    }



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// AudioManagerをセット
    /// </summary>
    /// <param name="audioManager"></param>
    public void SetAudioManager(AudioManager audioManager) { _audioManager = audioManager; }


    private void OnTriggerEnter(Collider other)
    {

        try
        {

            switch (_motionEnum)
            {

                case MotionEnum.NOMAL:

                    //物理挙動終了
                    _rb.isKinematic = true;
                    //拡大後のコライダー
                    _colllider.radius = _scaleMax;
                    //描画を消す
                    _meshRenderer.enabled = false;

                    //爆発Effectをアクティブ
                    if (_exObj) { _exObj.SetActive(true); }

                    //爆発音
                    if (_audioManager) { _audioManager.StartAudio(2, 1); }
                    //これにダメージ
                    Damage(_maxHp);
                    break;


                case MotionEnum.DAMAGE:
                    break;
                case MotionEnum.DOWN_WEAK:
                    break;
                case MotionEnum.DOWN_STRONG:
                    break;


                case MotionEnum.DEATH:

                    //衝突対象がプレイヤーなら攻撃
                    if (other.gameObject.tag == _playerTag)
                    {

                        print("大砲が" + other.gameObject + "に" + _attack + "のダメージ");
                        _playerManager.GetComponent<CharacterState>().Damage(_attack);
                    }
                    else
                    {

                        print("大砲が" + other.gameObject + "に" + _attack + "のダメージ");
                        if (other.gameObject.GetComponent<CharacterState>()) { other.gameObject.GetComponent<CharacterState>().Damage(_attack); }
                    }

                    //時間経過後非アクティブ
                    StartCoroutine("ActiveInterval");
                    break;


                default:
                    break;
            }
        }
        catch { print("GimmickArtilleryBallでエラー"); }
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

            //物理挙動開始
            _rb.isKinematic = false;
            //デフォルトサイズのコライダー
            _colllider.radius = _scaleDef;
            //描画を消す
            _meshRenderer.enabled = true;

            //ステータスの初期化
            ResetState();

            //爆発Effectを非アクティブ
            _exObj.SetActive(false);

            //このオブジェクトを非アクティブ
            this.gameObject.SetActive(false);
        }
        catch { print("GimmickArtilleryBallでエラー"); }
    }
}
