using UnityEngine;



/// <summary>
/// ���͊Ǘ��N���X
/// </summary>
public class InputManager : MonoBehaviour
{

    [Header("�擾�R���|�[�l���g")]
    private InputData_S _inputData;//InputSystem�N���X
    [SerializeField,Tooltip("�v���C���[����N���X")]private PlayerManager _playerManager = default;
    [SerializeField, Tooltip("�J��������N���X")] private CameraManager _cameraManager = default;
    [SerializeField, Tooltip("Canvas����N���X")] private CanvasManager _canvasManager = default;


    [Header("���͊֘A")]
    [SerializeField, Tooltip("���͂̃f�b�g�]�[��"), Range(0, 1)] private float _deadZoneStick = default;


    [Header("�t���O�֘A")]
    private bool _isOption = false;//�I�v�V������ʂ��J���Ă��邩


    //������------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    private void Update()
    {

        //�Q�[���p�b�h���h�����Ă��Ȃ��ꍇ�������Ȃ�
        if (!_inputData.ControllerNull()) { print("�Q�[���p�b�h���h�����Ă��܂���"); return; }


        /****[�v���C���[�֘A]****/
        //�v���C���[�̓��͊Ǘ�
        //PlayerState��Null�ł͂Ȃ��Ȃ珈��
        if (_playerManager)
        {

            //�ړ��̑������
            _playerManager.PlayerController().PlayerMoveInput(_inputData.StickLeftBool(_deadZoneStick));

            //�I�v�V������ʂ��J���Ă�ꍇ�͏������Ȃ�
            if (!_isOption)
            {

                //�v���C���[�̕ύX
                _playerManager.ChangePlayer(_inputData.DpadRightDown());

                //�A�N�V�����̑������
                _playerManager.PlayerController().PlayerActionInput((_inputData.ButtonSouthDown(), _inputData.ButtonSouthUp()), (_inputData.ButtonWestDown(), _inputData.ButtonWestUp()));

                //�U���̑������
                _playerManager.PlayerController().PlayerAttackInput((_inputData.ShoulderRightDown(), _inputData.ShoulderRightUp()), (_inputData.TriggerRightDown(), _inputData.TriggerRightUp()),
                                                    (_inputData.ShoulderLeftDown(), _inputData.ShoulderLeftUp()), (_inputData.TriggerLeftDown(), _inputData.TriggerLeftUp()), _inputData.ButtonNorth());
            }
        }
        else { print("PlayerManager��Null"); }


        /****[�J�����֘A]****/
        //�J�����̓��͊Ǘ�
        //CameraController��Null�ł͂Ȃ��Ȃ珈��
        if (_cameraManager)
        {

            //�J�����̑������
            _cameraManager.CameraInput(_inputData.StickRightBool(_deadZoneStick), _inputData.StickRightDown(), _inputData.StickRightBool(_deadZoneStick), _inputData.StickRightValue(_deadZoneStick));
        }
        else { print("CameraManager��Null"); }


        /****[�I�v�V�����֘A]****/
        //�I�v�V�����̓��͊Ǘ�
        //OptionController��Null�ł͂Ȃ��Ȃ珈��
        if (_canvasManager)
        {

            //�I�v�V�����̑������
            _isOption = _canvasManager.CanvasInput(_inputData.ButtonStartDown(), _inputData.ButtonEast(), _inputData.ButtonWest(), _inputData.ButtonSouthDown(),
                                                  (_inputData.ShoulderLeftDown() ,_inputData.ShoulderRightDown()),
                                                  (_inputData.DpadUpDown(),_inputData.DpadDownDown(),_inputData.DpadLeftDown(),_inputData.DpadRightDown()));
        }
        else { print("CanvasManager��Null"); }
    }

    private void FixedUpdate()
    {

        //�Q�[���p�b�h���h�����Ă��Ȃ��ꍇ�������Ȃ�
        if (!_inputData.ControllerNull()) { return; }


        /****[�v���C���[�֘A]****/
        //�v���C���[�̈ړ��Ǘ�
        //PlayerState��Null�ł͂Ȃ��Ȃ珈��
        if (_playerManager.PlayerController())
        {

            //�v���C���[�̈ړ�����
            _playerManager.PlayerController().PlayerMove(_inputData.StickLeftValue(_deadZoneStick));
        }
        else { print("PlayerState��Null"); }
    }

    private void LateUpdate()
    {

        //�Q�[���p�b�h���h�����Ă��Ȃ��ꍇ�������Ȃ�
        if (!_inputData.ControllerNull()) { return; }


        /****[�J�����֘A]****/
        //�J�����̈ړ��Ǘ�
        //CameraController��Null�ł͂Ȃ��Ȃ珈��
        if (_cameraManager)
        {

            //�J�����̈ړ�����
            _cameraManager.CameraMove(_inputData.StickRightValue(_deadZoneStick));
        }
        else { print("CameraManager��Null"); }
    }
}
