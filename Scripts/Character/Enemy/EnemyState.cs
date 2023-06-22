using UnityEngine;


/// <summary>
/// 敵の基底クラス
/// </summary>
public class EnemyState : CharacterState
{

    [Header("ステータス関連(EnemyState)")]
    [SerializeField, Tooltip("攻撃力")] protected int _attack;
    private const string _playerManagerTag = "PlayerManager";//PlayerManagerのタグ
    protected PlayerManager _playerManager = default;//CharacterStateクラス
    private const string _playerTag = "Player";//プレイヤーのタグ
    protected GameObject _playerObj = default;//プレーヤーオブジェクト
    private CameraManager _cameraManager = default;//カメラの挙動管理クラス


    [Header("ロックオン関連(EnemyState)")]
    [SerializeField, Tooltip("ロックオン間を阻む障害物Layer")] private LayerMask _layerMask = default;
    [SerializeField, Tooltip("ロックオン範囲の最大距離"),Range(0,100)] private float _lockOnMaxDistance = default;
    [SerializeField, Tooltip("ロックオン範囲の最小距離"),Range(0,100)] private float _lockOnMinDistance = default;
    private Vector3 _rectTransform = default;//このオブジェクトのカメラ座標
    private bool _isLock = false;//ロックオン中か
    private bool  _isHit = false;//衝突したかどうか
    private bool _isRect;//カメラの画角内
    private float _distancePlayer = default;//プレイヤーとの距離


    //処理部---------------------------------------------------------------------------------------------------------------------------------------------------------------

    protected override void Start()
    {

        //基底
        base.Start();

        //取得関連
        _playerManager = GameObject.FindWithTag(_playerManagerTag).GetComponent<PlayerManager>();//PlayerManagerクラス
        _playerObj = GameObject.FindWithTag(_playerTag).gameObject;//プレーヤーオブジェクト
        _cameraManager = Camera.main.GetComponent<CameraManager>();//カメラの挙動管理クラス
    }

    private void Update()
    {

        //プレイヤーとの距離
        _distancePlayer = Vector3.Distance(_playerObj.transform.position, transform.position);
        //距離が範囲内なら
        if (_lockOnMaxDistance >= _distancePlayer && _distancePlayer >= _lockOnMinDistance)
        {

            //プレイヤーとの間に他オブジェクトが存在しなければtrue
            _isHit = !Physics.Linecast(_playerObj.transform.position + transform.up, transform.position + transform.up, _layerMask);
            //Linecastを描画
            Debug.DrawLine(_playerObj.transform.position + transform.up, transform.position + transform.up, _isHit ? Color.red : Color.green);

            //このオブジェクトのカメラ座標
            _rectTransform = Camera.main.WorldToViewportPoint(transform.position);
            //カメラの画角内ならtrue
            _isRect = 1 > _rectTransform.x && _rectTransform.x > 0 && 1 > _rectTransform.y && _rectTransform.y > 0 && _rectTransform.z > 0;

            //rayがhit中・カメラの画角内・ロックオン中ではない → Listに格納
            if (_isHit && _isRect && !_isLock) { _isLock = _cameraManager.CameraTargetList(true, gameObject); }
            //ロックオン中・!rayがhit中・!カメラの画角内 → Listから消す
            if (_isLock) { if (!_isHit || !_isRect) { _isLock = _cameraManager.CameraTargetList(false, gameObject); } }
        }
        //距離が範囲外なら
        else
        {

            //ロックオン中・!rayがhit中・!カメラの画角内 → Listから消す
            if (_isLock) { _isLock = _cameraManager.CameraTargetList(false, gameObject); }
        }
    }
}
