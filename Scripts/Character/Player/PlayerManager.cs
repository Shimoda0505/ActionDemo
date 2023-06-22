using System.Collections.Generic;
using UnityEngine;
using System;



/// <summary>
/// PlayerControllerの管理クラス
/// Playerに関するほとんどを管理
/// </summary>
public class PlayerManager : PlayerState
{

    [Header("プレイヤーデータ関連")]
    [SerializeField, Tooltip("プレーヤーオブジェクト")] private GameObject _playerObj;
    [SerializeField, Tooltip("プレーヤーのアニメーション管理クラス")] private PlayerAnimationController _playerAnimationController;
    private PlayerController _playerController;//使用するPlayerController
    private int _playerNumber = 0;//プレイヤー番号
    private int _playerLastNumber = 0;//最後のプレイヤー番号の補完


    [Header("プレイヤーのパッケージ関連")]
    public List<PlayerPackage> _playerPackages = new List<PlayerPackage>();
    [Serializable, Tooltip("プレイヤーのパッケージ")]
    public class PlayerPackage
    {
        [SerializeField, Tooltip("名前")] private string _name;
        [field: SerializeField, Tooltip("武器セットされているか")] private bool _isWeponSet;
        [field: SerializeField, Tooltip("プレイヤーの個別データ")] private PlayerData _playerData;
        [field: SerializeField, Tooltip("プレイヤーの個別クラス")] private PlayerController _playerController;
        [field: SerializeField, Tooltip("武器データ")] private GameObject[] _weapon;


        /// <summary>
        /// 武器セットされているか
        /// </summary>
        public bool IsWeponSet { get { return _isWeponSet; } }
        /// <summary>
        /// プレイヤーの個別データ
        /// </summary>
        public PlayerData NowPlayerData { get { return _playerData; } }
        /// <summary>
        /// プレイヤーの個別クラス
        /// </summary>
        public PlayerController NowPlayerController { get { return _playerController; } }
        /// <summary>
        /// 武器データ
        /// </summary>
        public GameObject[] Weapon { get { return _weapon; } }
    }


    [Header("コンポーネント関連")]
    private Transform _tr;//プレイヤーのTransform
    private Rigidbody _rb;//プレイヤーのRigidbody
    private Animator _anim;//プレイヤーのAnimator




    //処理部------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {

        try
        {

            //コンポーネント取得
            if (_playerObj)
            {

                //Transform
                _tr = _playerObj.transform;
                //Rigidbody
                _rb = _playerObj.GetComponent<Rigidbody>();
                //Animator
                _anim = _playerObj.GetComponent<Animator>();
            }
            else { print("PlayerObjがnull"); }


            //全てのPlayerControllerに初期設定
            for (int i = 0; i < _playerPackages.Count; i++)
            {

                //変数の取得
                PlayerPackage playerPackage = _playerPackages[i];

                //設定
                //コンポーネント関連の設定
                playerPackage.NowPlayerController.SetComponent(_tr, _rb, _anim);
                //PlayerDataの初期設定
                playerPackage.NowPlayerController.SetPlayerData(playerPackage.NowPlayerData);
                //PlayerDataの初期設定
                playerPackage.NowPlayerController.SetPlayerManager(this);
                //武器の初期設定
                for (int j = 0; j < playerPackage.Weapon.Length; j++)
                {

                    //武器の判定終了
                    playerPackage.Weapon[j].GetComponent<PlayerWeponCollider>().ChangeCollider(false, 0);
                    //武器を非表示
                    playerPackage.Weapon[j].SetActive(false);
                    //武器のコライダーを補完
                    playerPackage.NowPlayerController.SetPlayerWeponCollider(playerPackage.Weapon[j].GetComponent<PlayerWeponCollider>(), playerPackage.Weapon[j]);
                }
            }

            //初期のPlayerController
            _playerController = _playerPackages[_playerNumber].NowPlayerController;
            //PlayerAnimationEventsクラスのPLayerControllerの変更
            _playerAnimationController.NewPlayerController = _playerController;
            //初期のAnimator
            _anim.runtimeAnimatorController = _playerPackages[_playerNumber].NowPlayerData.ComponentPlayerData();
            //アニメーションの初期化
            _playerController.ResetAnim();
            //武器を表示
            for (int j = 0; j < _playerPackages[_playerNumber].Weapon.Length; j++) { _playerPackages[_playerNumber].Weapon[j].SetActive(true); }
        }
        catch { print("PlayerManagerでエラー"); }
    }



    //メソッド部----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// プレイヤー変更
    /// </summary>
    public void ChangePlayer(bool isDpadRightDown)
    {

        try
        {

            //アイドル中以外の時処理しない
            if (!_playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.IDLE)) { return; }

            if (isDpadRightDown)
            {

                //武器の非表示
                for (int j = 0; j < _playerPackages[_playerNumber].Weapon.Length; j++) { _playerPackages[_playerNumber].Weapon[j].SetActive(false); }

                //最後のプレイヤー番号の補完
                _playerLastNumber = _playerNumber;

                //使用するプレイヤーの番号
                for (int i = _playerNumber + 1; i < _playerPackages.Count; i++)
                {

                    //武器セットされていれば、プレイヤー番号の補完
                    if (_playerPackages[i].IsWeponSet) { _playerNumber = i; break; }
                }
                //番号が前回と同じなら0番を使用
                if (_playerLastNumber == _playerNumber) { _playerNumber = 0; }

                //武器の表示
                for (int j = 0; j < _playerPackages[_playerNumber].Weapon.Length; j++) { _playerPackages[_playerNumber].Weapon[j].SetActive(true); }

                //PlayerControllerの変更
                _playerController = _playerPackages[_playerNumber].NowPlayerController;
                //移動方向の受け渡し
                _playerController.SetMoveForward(_playerPackages[_playerLastNumber].NowPlayerController.GetMoveForward());
                //PlayerAnimationEventsクラスのPLayerControllerの変更
                _playerAnimationController.NewPlayerController = _playerController;
                //Animatorの変更
                _anim.runtimeAnimatorController = _playerPackages[_playerNumber].NowPlayerData.ComponentPlayerData();
                //アニメーションの初期化
                _playerController.ResetAnim();
            }
        }
        catch { print("PlayerManagerでエラー"); }
    }
    /// <summary>
    /// 現在のPlayerController
    /// </summary>
    public PlayerController PlayerController() { return _playerController; }
    /// <summary>
    /// ダメージ処理
    /// </summary>
    public override void Damage(int attack)
    {

        //回避中の時処理しない
        if (_playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.ROLL) ||
            _playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.DAMAGE) ||
            _playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.DOWN_STRONG) ||
            _playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.DOWN_WEAK) ||
            _playerController.GetNowMotionEnum(global::PlayerController.MotionEnum.DEATH)) { return; }


        //基底
        base.Damage(attack);

        //死亡enum
        if (_motionEnum == MotionEnum.DEATH) { _playerController.SetMotionEnum(global::PlayerController.MotionEnum.DEATH); }
        //大ダウンenum
        else if (_motionEnum == MotionEnum.DOWN_STRONG) { _playerController.SetMotionEnum(global::PlayerController.MotionEnum.DOWN_STRONG); }
        //軽ダウンenum
        else if (_motionEnum == MotionEnum.DOWN_WEAK) { _playerController.SetMotionEnum(global::PlayerController.MotionEnum.DOWN_WEAK); }
        //ダメージenum
        else if(_motionEnum == MotionEnum.DAMAGE) { _playerController.SetMotionEnum(global::PlayerController.MotionEnum.DAMAGE); }

        //通常enumに遷移
        _motionEnum = MotionEnum.NOMAL;
    }
}
