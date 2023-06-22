using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



/// <summary>
///Canvas制御クラス
/// </summary>
public class CanvasManager : MonoBehaviour
{

    [Header("オプション関連")]
    [SerializeField,Tooltip("Option画面")] private GameObject _optionImage = default;
    [SerializeField,Tooltip("設定画面")] private GameObject _settingImage = default;
    [SerializeField, Tooltip("終了時の長押しImage")] private Image _endPutImage = default;
    [SerializeField, Tooltip("再開時の長押しImage")] private Image _resetPutImage = default;
    [SerializeField, Tooltip("終了までの時間"), Range(1, 10)] private int _endTime = default;
    private float _endPutImageFillAmount = 0;//_endPutImageのfillAmount
    private float _resetPutImageFillAmount = 0;//_resetPutImageのfillAmount


    [Header("ステータス関連")]
    [SerializeField, Tooltip("Hpバー")] private Image _hpSlider = default;
    [SerializeField, Tooltip("スタミナバー")] private Image _staminaSlider = default;


    [Header("死亡関連")]
    [SerializeField, Tooltip("死亡UI")] private GameObject _diedImage = default;
    [SerializeField, Tooltip("終了時間"), Range(1, 10)] private int _gameEndTime = default;


    [Header("フェード関連")]
    [SerializeField, Tooltip("フェードイン")] private GameObject _fadeInImage = default;
    [SerializeField, Tooltip("フェードアウト")] private GameObject _fadeOutImage = default;
    [SerializeField, Tooltip("ゲーム開始フェードアウト")] private GameObject _gameStartFadeInImage = default;


    [Header("ロックオン関連")]
    [SerializeField, Tooltip("ロックオン画像")] private Image _lockOn = default;
    [SerializeField, Tooltip("座標オフセット")] private Vector3 _posOffset = default;
    private bool _isLook = false;//ロックオン中か
    private GameObject _lockOnObj = default;//ロックオン対象


    [Header("設定関連")]
    public List<SettingPackage> _settingPackage = new List<SettingPackage>();
    [Serializable, Tooltip("設定関連パッケージ")]
    public class SettingPackage
    {

        [SerializeField, Tooltip("名前")] private string _name;
        [field: SerializeField, Tooltip("スライダー")] private Slider _settingSlider = default;
        [field: SerializeField, Tooltip("背景")] private Image _settingImageBack = default;
        [field: SerializeField, Tooltip("テキスト")] private Text _settingText = default;
        private float _settingSliderBar = default;//現在のスライダー値

        /// <summary>
        /// スライダー
        /// </summary>
        public Slider SettingSlider { get { return _settingSlider; } set { _settingSlider = value; } }
        /// <summary>
        /// 背景
        /// </summary>
        public Image SettingImageBack { get { return _settingImageBack; } }
        /// <summary>
        /// テキスト
        /// </summary>
        public Text SettingText { get { return _settingText; } set { _settingText = value; } }
        /// <summary>
        /// 現在のスライダー値
        /// </summary>
        public float SettingSliderBar { get { return _settingSliderBar; } set { _settingSliderBar = value; } }
    }
    [SerializeField, Tooltip("カメラデータ管理クラス")] private CameraData _cameraData;
    [SerializeField, Tooltip("音管理クラス")] private AudioManager _audioManager = default;
    [SerializeField, Tooltip("Lighting")] private Light _light = default;
    private SettingPackage _nowSettingPackage = default;//現在残照中のSettingPackage
    private const float _defaultSlider = 5;//スライダーの初期値
    private const float _offsetSlider = 10;//1~10のint数値を0.1f~1fのfloat変換
    private const int _maxValue = 10;//スライダー最大値
    private const int _minValue = 0;//スライダー最小値
    private const int _cameraNumber = 0;//カメラ番号
    private const int _seNumber = 1;//SE番号
    private const int _bgmNumber = 2;//BGM番号
    private const int _lightNumber = 3;//ライト番号


    /// <summary>
    /// オプション画面のenum
    /// </summary>
    private enum MotionEnum
    {

        [InspectorName("待機")] IDLE,
        [InspectorName("動作")] MOVE,
        [InspectorName("イベント")] EVENT,
        [InspectorName("終了")] END,
    }
    private MotionEnum _motionEnum = MotionEnum.IDLE;
    /// <summary>
    /// オプション画面or設定画面
    /// </summary>
    private enum OptionSettingEnum
    {
        [InspectorName("オプション画面")] OPTION,
        [InspectorName("設定画面")] SETTING,
    }
    private OptionSettingEnum _optionSettingEnum = OptionSettingEnum.OPTION;
    /// <summary>
    /// 設定画面のenum
    /// </summary>
    private enum SettingEnum
    {
        [InspectorName("カメラ速度")] CAMERA_SPEED,
        [InspectorName("SE音量")] SE_VOL,
        [InspectorName("BGM音量")] BGM_VOL,
        [InspectorName("明暗")] LIGHT,
    }
    private SettingEnum _settingEnum = SettingEnum.CAMERA_SPEED;



    //処理部-------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //マウスカーソルを非表示
        Cursor.visible = false;


        /*[ オプション関連 ]*/
        //オプションUIを非アクティブ
        if (_optionImage) _optionImage.SetActive(false);
        //設定UIを非アクティブ
        if (_settingImage) _settingImage.SetActive(false);
        //オプション画面enum
        _optionSettingEnum = OptionSettingEnum.OPTION;
        //終了の長押しImageの非描画
        if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;
        //再開の長押しImageの非描画
        if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;

        /*[ 死亡関連 ]*/
        //死亡UIを非アクティブ
        if (_diedImage) _diedImage.SetActive(false);

        /*[ フェード関連 ]*/
        //フェードインUIを非アクティブ
        if (_fadeInImage) _fadeInImage.SetActive(false);
        //フェードアウトUIをアクティブ
        if(_fadeOutImage) _fadeOutImage.SetActive(true);
        //ゲーム開始フェードアウトUIをアクティブ
        if (_gameStartFadeInImage) _gameStartFadeInImage.SetActive(true);

        /*[ ロックオン関連 ]*/
        //ロックオンUIを非アクティブ
        if (_lockOn) _lockOn.enabled = false;

        /*[ 設定画面関連 ]*/
        for (int i = 0; i < _settingPackage.Count; i++) 
        {

            //SettingPackageの変更
            ChangeSettingPackage(0, i);

            //各背景を非表示
            _nowSettingPackage.SettingImageBack.enabled = false;
        }
    }

    private void Update()
    {

        //ロックオン画像を対象のスクリーン座標に変換
        if (_isLook) { _lockOn.rectTransform.position = Camera.main.WorldToScreenPoint(_lockOnObj.transform.position + _posOffset); }
    }



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //パブリックメソッド------------------------------------------------------------------------------
    /// <summary>
    /// Canvas関連の入力操作
    /// </summary>
    /// <returns></returns>
    public bool CanvasInput(bool isOption, bool isGameEnd, bool isGameReset, bool isEnd, (bool left, bool right) isTrigger, (bool up, bool down, bool left, bool right) isDpad)
    {


        switch (_motionEnum)
        {

            case MotionEnum.IDLE:

                //入力時に
                if (isOption) 
                {

                    //オプション画面をアクティブ
                    if (_optionImage) _optionImage.SetActive(true);
                    //動作enumに遷移
                    _motionEnum = MotionEnum.MOVE;
                    //設定enumに遷移
                    _optionSettingEnum = OptionSettingEnum.OPTION;
                }
                return false;


            case MotionEnum.MOVE:

                switch (_optionSettingEnum)
                {
                    case OptionSettingEnum.OPTION:

                        //入力時に非アクティブ
                        if (isOption)
                        {

                            //オプション画面を非アクティブ
                            if (_optionImage) _optionImage.SetActive(false);
                            //設定UIを非アクティブ
                            if (_settingImage) _settingImage.SetActive(false);

                            //fillAmountの初期化
                            _endPutImageFillAmount = 0;
                            //fillAmountの更新
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;

                            //fillAmountの初期化
                            _resetPutImageFillAmount = 0;
                            //fillAmountの更新
                            if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;

                            //待機enumに遷移
                            _motionEnum = MotionEnum.IDLE;
                            break;
                        }

                        //設定画面に遷移
                        if (isTrigger.right)
                        {
  
                            //オプションUIを非アクティブ
                            if (_optionImage) _optionImage.SetActive(false);
                            //設定UIを非アクティブ
                            if (_settingImage) _settingImage.SetActive(true);

                            //初期背景を表示
                            _settingPackage[0].SettingImageBack.enabled = true;


                            //fillAmountの初期化
                            _endPutImageFillAmount = 0;
                            //fillAmountの更新
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;

                            //fillAmountの初期化
                            _resetPutImageFillAmount = 0;
                            //fillAmountの更新
                            if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;

                            //設定enumに遷移
                            _optionSettingEnum = OptionSettingEnum.SETTING;
                            //カメラ速度enumに遷移
                            _settingEnum = SettingEnum.CAMERA_SPEED;
                            break;
                        }

                        //ゲーム終了
                        //入力時に計測開始
                        if (isGameEnd)
                        {

                            //fillAmountの加算
                            _endPutImageFillAmount += Time.deltaTime / _endTime;
                            //fillAmountの更新
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;

                            //fillAmountが最大時
                            if (_endPutImageFillAmount >= 1)
                            {

                                //フェードインUIを非アクティブ
                                if (_fadeInImage) _fadeInImage.SetActive(true);
                                //ゲームの終了の計測開始
                                StartCoroutine("EndGame");
                                //終了enum
                                _motionEnum = MotionEnum.END;
                                return true;
                            }
                        }
                        //fillAmountの初期化
                        else
                        {

                            //fillAmountの初期化
                            _endPutImageFillAmount = 0;
                            //fillAmountの更新
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;
                        }

                        //ゲームReset
                        //入力時に計測開始
                        if (isGameReset)
                        {

                            //fillAmountの加算
                            _resetPutImageFillAmount += Time.deltaTime / _endTime;
                            //fillAmountの更新
                            if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;

                            //fillAmountが最大時
                            if (_resetPutImageFillAmount >= 1)
                            {

                                //フェードインUIをアクティブ
                                if (_fadeInImage) _fadeInImage.SetActive(true);
                                //ゲーム再起動の計測開始
                                StartCoroutine("ResetGame");
                                //終了enum
                                _motionEnum = MotionEnum.END;
                                return true;
                            }
                        }
                        //fillAmountの初期化
                        else
                        {

                            //fillAmountの初期化
                            _resetPutImageFillAmount = 0;
                            //fillAmountの更新
                            if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;
                        }
                        break;


                    case OptionSettingEnum.SETTING:

                        //入力時に非アクティブ
                        if (isOption)
                        {

                            //オプション画面を非アクティブ
                            if (_optionImage) _optionImage.SetActive(false);
                            //設定UIを非アクティブ
                            if (_settingImage) _settingImage.SetActive(false);

                            //fillAmountの初期化
                            _endPutImageFillAmount = 0;
                            //fillAmountの更新
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;

                            //各背景を非表示
                            for (int i = 0; i < _settingPackage.Count; i++) { _settingPackage[i].SettingImageBack.enabled = false; }

                            //待機enumに遷移
                            _motionEnum = MotionEnum.IDLE;
                            break;
                        }

                        //操作画面に遷移
                        if (isTrigger.left)
                        {

                            //設定enumに遷移
                            _optionSettingEnum = OptionSettingEnum.OPTION;

                            //オプションUIを非アクティブ
                            if (_optionImage) _optionImage.SetActive(true);
                            //設定UIを非アクティブ
                            if (_settingImage) _settingImage.SetActive(false);

                            //各背景を非表示
                            for (int i = 0; i < _settingPackage.Count; i++) { _settingPackage[i].SettingImageBack.enabled = false; }
                            break;
                        }

                        //設定画面の遷移
                        switch (_settingEnum)
                        {

                            case SettingEnum.CAMERA_SPEED:

                                //下入力・設定場面の背景変更・SE_VOLenumに変更
                                if (isDpad.down) { BackImageEnabled(_cameraNumber, _seNumber); _settingEnum = SettingEnum.SE_VOL; }
                                //左入力・スライダーの減算
                                else if (isDpad.left) { ChangeSettingPackage(-1, _cameraNumber); }
                                //右入力・スライダーの加算
                                else if (isDpad.right) { ChangeSettingPackage(1, _cameraNumber); }
                                break;


                            case SettingEnum.SE_VOL:

                                //上入力・設定場面の背景変更・CAMERA_SPEEDenumに変更
                                if (isDpad.up) { BackImageEnabled(_seNumber, _cameraNumber); _settingEnum = SettingEnum.CAMERA_SPEED; }
                                //下入力・設定場面の背景変更・BGM_VOLenumに変更
                                else if (isDpad.down) { BackImageEnabled(_seNumber, _bgmNumber); _settingEnum = SettingEnum.BGM_VOL; }
                                //左入力・スライダーの減算
                                else if (isDpad.left) { ChangeSettingPackage(-1, _seNumber); }
                                //右入力・スライダーの加算
                                else if (isDpad.right) { ChangeSettingPackage(1, _seNumber); }
                                break;


                            case SettingEnum.BGM_VOL:

                                //上入力・設定場面の背景変更・SE_VOLenumに変更
                                if (isDpad.up) { BackImageEnabled(_bgmNumber, _seNumber); _settingEnum = SettingEnum.SE_VOL; }
                                //下入力・設定場面の背景変更・LIGHTenumに変更
                                else if (isDpad.down) { BackImageEnabled(_bgmNumber, _lightNumber); _settingEnum = SettingEnum.LIGHT; }
                                //左入力・スライダーの減算
                                else if (isDpad.left) { ChangeSettingPackage(-1, _bgmNumber); }
                                //右入力・スライダーの加算
                                else if (isDpad.right) { ChangeSettingPackage(1, _bgmNumber); }
                                break;


                            case SettingEnum.LIGHT:

                                //上入力・設定場面の背景変更・BGM_VOLenumに変更
                                if (isDpad.up) { BackImageEnabled(_lightNumber, _bgmNumber); _settingEnum = SettingEnum.BGM_VOL; }
                                //左入力・スライダーの減算
                                else if (isDpad.left) { ChangeSettingPackage(-1, _lightNumber); }
                                //右入力・スライダーの加算
                                else if (isDpad.right) { ChangeSettingPackage(1, _lightNumber); }
                                break;


                            default:

                                print("SettingEnumのcaseがありません");
                                break;
                        }
                        break;


                    default:

                        print("OptionSettingEnumのcaseがありません");
                        break;
                }
                return true;


            case MotionEnum.EVENT:

                //入力時にゲーム終了の開始
                if (isEnd)
                {

                    //フェードインUIをアクティブ
                    if (_fadeInImage) _fadeInImage.SetActive(true);
                    //ゲーム再起動の計測開始
                    StartCoroutine("ResetGame"); 
                }
                return false;


            case MotionEnum.END:
                return false;


            default:

                print("MotionEnumのcaseがありません");
                return false;
        }
    }
    /// <summary>
    /// Hpバーのスライダーを変更
    /// </summary>
    public void HpSlider(float maxHp, float nowStamina) { _hpSlider.fillAmount = nowStamina / maxHp; }
    /// <summary>
    /// スタミナバーのスライダーを変更
    /// </summary>
    public void StaminaSlider(float maxStamina, float nowStamina) { _staminaSlider.fillAmount = nowStamina / maxStamina; }
    /// <summary>
    /// 死亡時のUI
    /// </summary>
    public void StartDiedUI() 
    { 
        
        //イベントenumに遷移
        _motionEnum = MotionEnum.EVENT;
        //オプション画面を非アクティブ
        _optionImage.SetActive(false);
        //死亡UIをアクティブ
        _diedImage.SetActive(true); 
    }
    /// <summary>
    /// ロックオン画像の更新
    /// </summary>
    public void StartLockOn(bool isLock, GameObject obj)
    {

        if (isLock)
        { 
            
            //ロックオン対象の変更
            _lockOnObj = obj;
            //ロックオン中でなければ
            if (!_isLook)
            {

                //ロックオンの開始
                _isLook = true;
                //ロックオンUIをアクティブ
                if (_lockOn) _lockOn.enabled = true;
            }
        }
        else
        {

            //ロックオンの終了
            _isLook = false;
            //ロックオンUIを非アクティブ
            if (_lockOn) _lockOn.enabled = false;
        }
    }

    //プライベートメソッド------------------------------------------------------------------------------
    /// <summary>
    /// SettingPackageの変更
    /// </summary>
    private void ChangeSettingPackage(int addSubtract, int number)
    {

        //現在残照中のSettingPackageを変更
        _nowSettingPackage = _settingPackage[number];

        //スライダー値の変更
        if (addSubtract == -1) { _nowSettingPackage.SettingSliderBar--; if (_nowSettingPackage.SettingSliderBar <= _minValue) { _nowSettingPackage.SettingSliderBar = _minValue; } }//減算
        if(addSubtract == 0) { _nowSettingPackage.SettingSliderBar = _defaultSlider; }//初期値
        if (addSubtract == 1) { _nowSettingPackage.SettingSliderBar++; if (_nowSettingPackage.SettingSliderBar >= _maxValue) { _nowSettingPackage.SettingSliderBar = _maxValue; } }//加算
        
        //表記数値の変更
        _nowSettingPackage.SettingText.text = _nowSettingPackage.SettingSliderBar.ToString();
        //バリューの変更
        _nowSettingPackage.SettingSlider.value = _nowSettingPackage.SettingSliderBar / _offsetSlider;

        //各種反映
        if (number == _cameraNumber) { if (_cameraData) _cameraData.ChangeInputSpeed(_nowSettingPackage.SettingSlider.value); }//カメラ速度
        else if (number == _seNumber) { if (_audioManager) _audioManager.ChangeSeVolume(_nowSettingPackage.SettingSlider.value); }//SE
        else if (number == _bgmNumber) { if (_audioManager) _audioManager.ChangeBgmVolume(_nowSettingPackage.SettingSlider.value); }//BGM
        else if (number == _lightNumber) { if (_light) _light.intensity = _nowSettingPackage.SettingSlider.value; }//ライト
    }
    /// <summary>
    /// 設定場面の背景変更
    /// </summary>
    private void BackImageEnabled(int falseNumber,int trueNumber)
    {

        //背景を表示
        _settingPackage[falseNumber].SettingImageBack.enabled = false;
        //背景を非表示
        _settingPackage[trueNumber].SettingImageBack.enabled = true;
    }

    /* [ コルーチン関連 ] */
    /// <summary>
    /// ゲームの再起動
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetGame()
    {

        //待機
        yield return new WaitForSeconds(_gameEndTime);

        //シーンの再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// ゲームの終了
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndGame()
    {

        //待機
        yield return new WaitForSeconds(_gameEndTime);

        //ゲームの終了
        Application.Quit();
        print("ゲームの終了");
    }
}
