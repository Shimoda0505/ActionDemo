using UnityEngine;



/// <summary>
/// 入力管理クラス
/// </summary>
public class InputManager : MonoBehaviour
{

    [Header("取得コンポーネント")]
    private InputData_S _inputData;//InputSystemクラス
    [SerializeField,Tooltip("プレイヤー制御クラス")]private PlayerManager _playerManager = default;
    [SerializeField, Tooltip("カメラ制御クラス")] private CameraManager _cameraManager = default;
    [SerializeField, Tooltip("Canvas制御クラス")] private CanvasManager _canvasManager = default;


    [Header("入力関連")]
    [SerializeField, Tooltip("入力のデットゾーン"), Range(0, 1)] private float _deadZoneStick = default;


    [Header("フラグ関連")]
    private bool _isOption = false;//オプション画面を開いているか


    //処理部------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Update()
    {

        //ゲームパッドが刺さっていない場合処理しない
        if (!_inputData.ControllerNull()) { print("ゲームパッドが刺さっていません"); return; }


        /****[プレイヤー関連]****/
        //プレイヤーの入力管理
        //PlayerStateがNullではないなら処理
        if (_playerManager)
        {

            //移動の操作入力
            _playerManager.PlayerController().PlayerMoveInput(_inputData.StickLeftBool(_deadZoneStick));

            //オプション画面を開いてる場合は処理しない
            if (!_isOption)
            {

                //プレイヤーの変更
                _playerManager.ChangePlayer(_inputData.DpadRightDown());

                //アクションの操作入力
                _playerManager.PlayerController().PlayerActionInput((_inputData.ButtonSouthDown(), _inputData.ButtonSouthUp()), (_inputData.ButtonWestDown(), _inputData.ButtonWestUp()));

                //攻撃の操作入力
                _playerManager.PlayerController().PlayerAttackInput((_inputData.ShoulderRightDown(), _inputData.ShoulderRightUp()), (_inputData.TriggerRightDown(), _inputData.TriggerRightUp()),
                                                    (_inputData.ShoulderLeftDown(), _inputData.ShoulderLeftUp()), (_inputData.TriggerLeftDown(), _inputData.TriggerLeftUp()), _inputData.ButtonNorth());
            }
        }
        else { print("PlayerManagerがNull"); }


        /****[カメラ関連]****/
        //カメラの入力管理
        //CameraControllerがNullではないなら処理
        if (_cameraManager)
        {

            //カメラの操作入力
            _cameraManager.CameraInput(_inputData.StickRightBool(_deadZoneStick), _inputData.StickRightDown(), _inputData.StickRightBool(_deadZoneStick), _inputData.StickRightValue(_deadZoneStick));
        }
        else { print("CameraManagerがNull"); }


        /****[オプション関連]****/
        //オプションの入力管理
        //OptionControllerがNullではないなら処理
        if (_canvasManager)
        {

            //オプションの操作入力
            _isOption = _canvasManager.CanvasInput(_inputData.ButtonStartDown(), _inputData.ButtonEast(), _inputData.ButtonWest(), _inputData.ButtonSouthDown(),
                                                  (_inputData.ShoulderLeftDown() ,_inputData.ShoulderRightDown()),
                                                  (_inputData.DpadUpDown(),_inputData.DpadDownDown(),_inputData.DpadLeftDown(),_inputData.DpadRightDown()));
        }
        else { print("CanvasManagerがNull"); }
    }

    private void FixedUpdate()
    {

        //ゲームパッドが刺さっていない場合処理しない
        if (!_inputData.ControllerNull()) { return; }


        /****[プレイヤー関連]****/
        //プレイヤーの移動管理
        //PlayerStateがNullではないなら処理
        if (_playerManager.PlayerController())
        {

            //プレイヤーの移動処理
            _playerManager.PlayerController().PlayerMove(_inputData.StickLeftValue(_deadZoneStick));
        }
        else { print("PlayerStateがNull"); }
    }

    private void LateUpdate()
    {

        //ゲームパッドが刺さっていない場合処理しない
        if (!_inputData.ControllerNull()) { return; }


        /****[カメラ関連]****/
        //カメラの移動管理
        //CameraControllerがNullではないなら処理
        if (_cameraManager)
        {

            //カメラの移動処理
            _cameraManager.CameraMove(_inputData.StickRightValue(_deadZoneStick));
        }
        else { print("CameraManagerがNull"); }
    }
}
