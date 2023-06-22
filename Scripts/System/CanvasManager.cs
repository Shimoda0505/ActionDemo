using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



/// <summary>
///Canvas����N���X
/// </summary>
public class CanvasManager : MonoBehaviour
{

    [Header("�I�v�V�����֘A")]
    [SerializeField,Tooltip("Option���")] private GameObject _optionImage = default;
    [SerializeField,Tooltip("�ݒ���")] private GameObject _settingImage = default;
    [SerializeField, Tooltip("�I�����̒�����Image")] private Image _endPutImage = default;
    [SerializeField, Tooltip("�ĊJ���̒�����Image")] private Image _resetPutImage = default;
    [SerializeField, Tooltip("�I���܂ł̎���"), Range(1, 10)] private int _endTime = default;
    private float _endPutImageFillAmount = 0;//_endPutImage��fillAmount
    private float _resetPutImageFillAmount = 0;//_resetPutImage��fillAmount


    [Header("�X�e�[�^�X�֘A")]
    [SerializeField, Tooltip("Hp�o�[")] private Image _hpSlider = default;
    [SerializeField, Tooltip("�X�^�~�i�o�[")] private Image _staminaSlider = default;


    [Header("���S�֘A")]
    [SerializeField, Tooltip("���SUI")] private GameObject _diedImage = default;
    [SerializeField, Tooltip("�I������"), Range(1, 10)] private int _gameEndTime = default;


    [Header("�t�F�[�h�֘A")]
    [SerializeField, Tooltip("�t�F�[�h�C��")] private GameObject _fadeInImage = default;
    [SerializeField, Tooltip("�t�F�[�h�A�E�g")] private GameObject _fadeOutImage = default;
    [SerializeField, Tooltip("�Q�[���J�n�t�F�[�h�A�E�g")] private GameObject _gameStartFadeInImage = default;


    [Header("���b�N�I���֘A")]
    [SerializeField, Tooltip("���b�N�I���摜")] private Image _lockOn = default;
    [SerializeField, Tooltip("���W�I�t�Z�b�g")] private Vector3 _posOffset = default;
    private bool _isLook = false;//���b�N�I������
    private GameObject _lockOnObj = default;//���b�N�I���Ώ�


    [Header("�ݒ�֘A")]
    public List<SettingPackage> _settingPackage = new List<SettingPackage>();
    [Serializable, Tooltip("�ݒ�֘A�p�b�P�[�W")]
    public class SettingPackage
    {

        [SerializeField, Tooltip("���O")] private string _name;
        [field: SerializeField, Tooltip("�X���C�_�[")] private Slider _settingSlider = default;
        [field: SerializeField, Tooltip("�w�i")] private Image _settingImageBack = default;
        [field: SerializeField, Tooltip("�e�L�X�g")] private Text _settingText = default;
        private float _settingSliderBar = default;//���݂̃X���C�_�[�l

        /// <summary>
        /// �X���C�_�[
        /// </summary>
        public Slider SettingSlider { get { return _settingSlider; } set { _settingSlider = value; } }
        /// <summary>
        /// �w�i
        /// </summary>
        public Image SettingImageBack { get { return _settingImageBack; } }
        /// <summary>
        /// �e�L�X�g
        /// </summary>
        public Text SettingText { get { return _settingText; } set { _settingText = value; } }
        /// <summary>
        /// ���݂̃X���C�_�[�l
        /// </summary>
        public float SettingSliderBar { get { return _settingSliderBar; } set { _settingSliderBar = value; } }
    }
    [SerializeField, Tooltip("�J�����f�[�^�Ǘ��N���X")] private CameraData _cameraData;
    [SerializeField, Tooltip("���Ǘ��N���X")] private AudioManager _audioManager = default;
    [SerializeField, Tooltip("Lighting")] private Light _light = default;
    private SettingPackage _nowSettingPackage = default;//���ݎc�ƒ���SettingPackage
    private const float _defaultSlider = 5;//�X���C�_�[�̏����l
    private const float _offsetSlider = 10;//1~10��int���l��0.1f~1f��float�ϊ�
    private const int _maxValue = 10;//�X���C�_�[�ő�l
    private const int _minValue = 0;//�X���C�_�[�ŏ��l
    private const int _cameraNumber = 0;//�J�����ԍ�
    private const int _seNumber = 1;//SE�ԍ�
    private const int _bgmNumber = 2;//BGM�ԍ�
    private const int _lightNumber = 3;//���C�g�ԍ�


    /// <summary>
    /// �I�v�V������ʂ�enum
    /// </summary>
    private enum MotionEnum
    {

        [InspectorName("�ҋ@")] IDLE,
        [InspectorName("����")] MOVE,
        [InspectorName("�C�x���g")] EVENT,
        [InspectorName("�I��")] END,
    }
    private MotionEnum _motionEnum = MotionEnum.IDLE;
    /// <summary>
    /// �I�v�V�������or�ݒ���
    /// </summary>
    private enum OptionSettingEnum
    {
        [InspectorName("�I�v�V�������")] OPTION,
        [InspectorName("�ݒ���")] SETTING,
    }
    private OptionSettingEnum _optionSettingEnum = OptionSettingEnum.OPTION;
    /// <summary>
    /// �ݒ��ʂ�enum
    /// </summary>
    private enum SettingEnum
    {
        [InspectorName("�J�������x")] CAMERA_SPEED,
        [InspectorName("SE����")] SE_VOL,
        [InspectorName("BGM����")] BGM_VOL,
        [InspectorName("����")] LIGHT,
    }
    private SettingEnum _settingEnum = SettingEnum.CAMERA_SPEED;



    //������-------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //�}�E�X�J�[�\�����\��
        Cursor.visible = false;


        /*[ �I�v�V�����֘A ]*/
        //�I�v�V����UI���A�N�e�B�u
        if (_optionImage) _optionImage.SetActive(false);
        //�ݒ�UI���A�N�e�B�u
        if (_settingImage) _settingImage.SetActive(false);
        //�I�v�V�������enum
        _optionSettingEnum = OptionSettingEnum.OPTION;
        //�I���̒�����Image�̔�`��
        if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;
        //�ĊJ�̒�����Image�̔�`��
        if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;

        /*[ ���S�֘A ]*/
        //���SUI���A�N�e�B�u
        if (_diedImage) _diedImage.SetActive(false);

        /*[ �t�F�[�h�֘A ]*/
        //�t�F�[�h�C��UI���A�N�e�B�u
        if (_fadeInImage) _fadeInImage.SetActive(false);
        //�t�F�[�h�A�E�gUI���A�N�e�B�u
        if(_fadeOutImage) _fadeOutImage.SetActive(true);
        //�Q�[���J�n�t�F�[�h�A�E�gUI���A�N�e�B�u
        if (_gameStartFadeInImage) _gameStartFadeInImage.SetActive(true);

        /*[ ���b�N�I���֘A ]*/
        //���b�N�I��UI���A�N�e�B�u
        if (_lockOn) _lockOn.enabled = false;

        /*[ �ݒ��ʊ֘A ]*/
        for (int i = 0; i < _settingPackage.Count; i++) 
        {

            //SettingPackage�̕ύX
            ChangeSettingPackage(0, i);

            //�e�w�i���\��
            _nowSettingPackage.SettingImageBack.enabled = false;
        }
    }

    private void Update()
    {

        //���b�N�I���摜��Ώۂ̃X�N���[�����W�ɕϊ�
        if (_isLook) { _lockOn.rectTransform.position = Camera.main.WorldToScreenPoint(_lockOnObj.transform.position + _posOffset); }
    }



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //�p�u���b�N���\�b�h------------------------------------------------------------------------------
    /// <summary>
    /// Canvas�֘A�̓��͑���
    /// </summary>
    /// <returns></returns>
    public bool CanvasInput(bool isOption, bool isGameEnd, bool isGameReset, bool isEnd, (bool left, bool right) isTrigger, (bool up, bool down, bool left, bool right) isDpad)
    {


        switch (_motionEnum)
        {

            case MotionEnum.IDLE:

                //���͎���
                if (isOption) 
                {

                    //�I�v�V������ʂ��A�N�e�B�u
                    if (_optionImage) _optionImage.SetActive(true);
                    //����enum�ɑJ��
                    _motionEnum = MotionEnum.MOVE;
                    //�ݒ�enum�ɑJ��
                    _optionSettingEnum = OptionSettingEnum.OPTION;
                }
                return false;


            case MotionEnum.MOVE:

                switch (_optionSettingEnum)
                {
                    case OptionSettingEnum.OPTION:

                        //���͎��ɔ�A�N�e�B�u
                        if (isOption)
                        {

                            //�I�v�V������ʂ��A�N�e�B�u
                            if (_optionImage) _optionImage.SetActive(false);
                            //�ݒ�UI���A�N�e�B�u
                            if (_settingImage) _settingImage.SetActive(false);

                            //fillAmount�̏�����
                            _endPutImageFillAmount = 0;
                            //fillAmount�̍X�V
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;

                            //fillAmount�̏�����
                            _resetPutImageFillAmount = 0;
                            //fillAmount�̍X�V
                            if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;

                            //�ҋ@enum�ɑJ��
                            _motionEnum = MotionEnum.IDLE;
                            break;
                        }

                        //�ݒ��ʂɑJ��
                        if (isTrigger.right)
                        {
  
                            //�I�v�V����UI���A�N�e�B�u
                            if (_optionImage) _optionImage.SetActive(false);
                            //�ݒ�UI���A�N�e�B�u
                            if (_settingImage) _settingImage.SetActive(true);

                            //�����w�i��\��
                            _settingPackage[0].SettingImageBack.enabled = true;


                            //fillAmount�̏�����
                            _endPutImageFillAmount = 0;
                            //fillAmount�̍X�V
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;

                            //fillAmount�̏�����
                            _resetPutImageFillAmount = 0;
                            //fillAmount�̍X�V
                            if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;

                            //�ݒ�enum�ɑJ��
                            _optionSettingEnum = OptionSettingEnum.SETTING;
                            //�J�������xenum�ɑJ��
                            _settingEnum = SettingEnum.CAMERA_SPEED;
                            break;
                        }

                        //�Q�[���I��
                        //���͎��Ɍv���J�n
                        if (isGameEnd)
                        {

                            //fillAmount�̉��Z
                            _endPutImageFillAmount += Time.deltaTime / _endTime;
                            //fillAmount�̍X�V
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;

                            //fillAmount���ő厞
                            if (_endPutImageFillAmount >= 1)
                            {

                                //�t�F�[�h�C��UI���A�N�e�B�u
                                if (_fadeInImage) _fadeInImage.SetActive(true);
                                //�Q�[���̏I���̌v���J�n
                                StartCoroutine("EndGame");
                                //�I��enum
                                _motionEnum = MotionEnum.END;
                                return true;
                            }
                        }
                        //fillAmount�̏�����
                        else
                        {

                            //fillAmount�̏�����
                            _endPutImageFillAmount = 0;
                            //fillAmount�̍X�V
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;
                        }

                        //�Q�[��Reset
                        //���͎��Ɍv���J�n
                        if (isGameReset)
                        {

                            //fillAmount�̉��Z
                            _resetPutImageFillAmount += Time.deltaTime / _endTime;
                            //fillAmount�̍X�V
                            if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;

                            //fillAmount���ő厞
                            if (_resetPutImageFillAmount >= 1)
                            {

                                //�t�F�[�h�C��UI���A�N�e�B�u
                                if (_fadeInImage) _fadeInImage.SetActive(true);
                                //�Q�[���ċN���̌v���J�n
                                StartCoroutine("ResetGame");
                                //�I��enum
                                _motionEnum = MotionEnum.END;
                                return true;
                            }
                        }
                        //fillAmount�̏�����
                        else
                        {

                            //fillAmount�̏�����
                            _resetPutImageFillAmount = 0;
                            //fillAmount�̍X�V
                            if (_resetPutImage) _resetPutImage.fillAmount = _resetPutImageFillAmount;
                        }
                        break;


                    case OptionSettingEnum.SETTING:

                        //���͎��ɔ�A�N�e�B�u
                        if (isOption)
                        {

                            //�I�v�V������ʂ��A�N�e�B�u
                            if (_optionImage) _optionImage.SetActive(false);
                            //�ݒ�UI���A�N�e�B�u
                            if (_settingImage) _settingImage.SetActive(false);

                            //fillAmount�̏�����
                            _endPutImageFillAmount = 0;
                            //fillAmount�̍X�V
                            if (_endPutImage) _endPutImage.fillAmount = _endPutImageFillAmount;

                            //�e�w�i���\��
                            for (int i = 0; i < _settingPackage.Count; i++) { _settingPackage[i].SettingImageBack.enabled = false; }

                            //�ҋ@enum�ɑJ��
                            _motionEnum = MotionEnum.IDLE;
                            break;
                        }

                        //�����ʂɑJ��
                        if (isTrigger.left)
                        {

                            //�ݒ�enum�ɑJ��
                            _optionSettingEnum = OptionSettingEnum.OPTION;

                            //�I�v�V����UI���A�N�e�B�u
                            if (_optionImage) _optionImage.SetActive(true);
                            //�ݒ�UI���A�N�e�B�u
                            if (_settingImage) _settingImage.SetActive(false);

                            //�e�w�i���\��
                            for (int i = 0; i < _settingPackage.Count; i++) { _settingPackage[i].SettingImageBack.enabled = false; }
                            break;
                        }

                        //�ݒ��ʂ̑J��
                        switch (_settingEnum)
                        {

                            case SettingEnum.CAMERA_SPEED:

                                //�����́E�ݒ��ʂ̔w�i�ύX�ESE_VOLenum�ɕύX
                                if (isDpad.down) { BackImageEnabled(_cameraNumber, _seNumber); _settingEnum = SettingEnum.SE_VOL; }
                                //�����́E�X���C�_�[�̌��Z
                                else if (isDpad.left) { ChangeSettingPackage(-1, _cameraNumber); }
                                //�E���́E�X���C�_�[�̉��Z
                                else if (isDpad.right) { ChangeSettingPackage(1, _cameraNumber); }
                                break;


                            case SettingEnum.SE_VOL:

                                //����́E�ݒ��ʂ̔w�i�ύX�ECAMERA_SPEEDenum�ɕύX
                                if (isDpad.up) { BackImageEnabled(_seNumber, _cameraNumber); _settingEnum = SettingEnum.CAMERA_SPEED; }
                                //�����́E�ݒ��ʂ̔w�i�ύX�EBGM_VOLenum�ɕύX
                                else if (isDpad.down) { BackImageEnabled(_seNumber, _bgmNumber); _settingEnum = SettingEnum.BGM_VOL; }
                                //�����́E�X���C�_�[�̌��Z
                                else if (isDpad.left) { ChangeSettingPackage(-1, _seNumber); }
                                //�E���́E�X���C�_�[�̉��Z
                                else if (isDpad.right) { ChangeSettingPackage(1, _seNumber); }
                                break;


                            case SettingEnum.BGM_VOL:

                                //����́E�ݒ��ʂ̔w�i�ύX�ESE_VOLenum�ɕύX
                                if (isDpad.up) { BackImageEnabled(_bgmNumber, _seNumber); _settingEnum = SettingEnum.SE_VOL; }
                                //�����́E�ݒ��ʂ̔w�i�ύX�ELIGHTenum�ɕύX
                                else if (isDpad.down) { BackImageEnabled(_bgmNumber, _lightNumber); _settingEnum = SettingEnum.LIGHT; }
                                //�����́E�X���C�_�[�̌��Z
                                else if (isDpad.left) { ChangeSettingPackage(-1, _bgmNumber); }
                                //�E���́E�X���C�_�[�̉��Z
                                else if (isDpad.right) { ChangeSettingPackage(1, _bgmNumber); }
                                break;


                            case SettingEnum.LIGHT:

                                //����́E�ݒ��ʂ̔w�i�ύX�EBGM_VOLenum�ɕύX
                                if (isDpad.up) { BackImageEnabled(_lightNumber, _bgmNumber); _settingEnum = SettingEnum.BGM_VOL; }
                                //�����́E�X���C�_�[�̌��Z
                                else if (isDpad.left) { ChangeSettingPackage(-1, _lightNumber); }
                                //�E���́E�X���C�_�[�̉��Z
                                else if (isDpad.right) { ChangeSettingPackage(1, _lightNumber); }
                                break;


                            default:

                                print("SettingEnum��case������܂���");
                                break;
                        }
                        break;


                    default:

                        print("OptionSettingEnum��case������܂���");
                        break;
                }
                return true;


            case MotionEnum.EVENT:

                //���͎��ɃQ�[���I���̊J�n
                if (isEnd)
                {

                    //�t�F�[�h�C��UI���A�N�e�B�u
                    if (_fadeInImage) _fadeInImage.SetActive(true);
                    //�Q�[���ċN���̌v���J�n
                    StartCoroutine("ResetGame"); 
                }
                return false;


            case MotionEnum.END:
                return false;


            default:

                print("MotionEnum��case������܂���");
                return false;
        }
    }
    /// <summary>
    /// Hp�o�[�̃X���C�_�[��ύX
    /// </summary>
    public void HpSlider(float maxHp, float nowStamina) { _hpSlider.fillAmount = nowStamina / maxHp; }
    /// <summary>
    /// �X�^�~�i�o�[�̃X���C�_�[��ύX
    /// </summary>
    public void StaminaSlider(float maxStamina, float nowStamina) { _staminaSlider.fillAmount = nowStamina / maxStamina; }
    /// <summary>
    /// ���S����UI
    /// </summary>
    public void StartDiedUI() 
    { 
        
        //�C�x���genum�ɑJ��
        _motionEnum = MotionEnum.EVENT;
        //�I�v�V������ʂ��A�N�e�B�u
        _optionImage.SetActive(false);
        //���SUI���A�N�e�B�u
        _diedImage.SetActive(true); 
    }
    /// <summary>
    /// ���b�N�I���摜�̍X�V
    /// </summary>
    public void StartLockOn(bool isLock, GameObject obj)
    {

        if (isLock)
        { 
            
            //���b�N�I���Ώۂ̕ύX
            _lockOnObj = obj;
            //���b�N�I�����łȂ����
            if (!_isLook)
            {

                //���b�N�I���̊J�n
                _isLook = true;
                //���b�N�I��UI���A�N�e�B�u
                if (_lockOn) _lockOn.enabled = true;
            }
        }
        else
        {

            //���b�N�I���̏I��
            _isLook = false;
            //���b�N�I��UI���A�N�e�B�u
            if (_lockOn) _lockOn.enabled = false;
        }
    }

    //�v���C�x�[�g���\�b�h------------------------------------------------------------------------------
    /// <summary>
    /// SettingPackage�̕ύX
    /// </summary>
    private void ChangeSettingPackage(int addSubtract, int number)
    {

        //���ݎc�ƒ���SettingPackage��ύX
        _nowSettingPackage = _settingPackage[number];

        //�X���C�_�[�l�̕ύX
        if (addSubtract == -1) { _nowSettingPackage.SettingSliderBar--; if (_nowSettingPackage.SettingSliderBar <= _minValue) { _nowSettingPackage.SettingSliderBar = _minValue; } }//���Z
        if(addSubtract == 0) { _nowSettingPackage.SettingSliderBar = _defaultSlider; }//�����l
        if (addSubtract == 1) { _nowSettingPackage.SettingSliderBar++; if (_nowSettingPackage.SettingSliderBar >= _maxValue) { _nowSettingPackage.SettingSliderBar = _maxValue; } }//���Z
        
        //�\�L���l�̕ύX
        _nowSettingPackage.SettingText.text = _nowSettingPackage.SettingSliderBar.ToString();
        //�o�����[�̕ύX
        _nowSettingPackage.SettingSlider.value = _nowSettingPackage.SettingSliderBar / _offsetSlider;

        //�e�픽�f
        if (number == _cameraNumber) { if (_cameraData) _cameraData.ChangeInputSpeed(_nowSettingPackage.SettingSlider.value); }//�J�������x
        else if (number == _seNumber) { if (_audioManager) _audioManager.ChangeSeVolume(_nowSettingPackage.SettingSlider.value); }//SE
        else if (number == _bgmNumber) { if (_audioManager) _audioManager.ChangeBgmVolume(_nowSettingPackage.SettingSlider.value); }//BGM
        else if (number == _lightNumber) { if (_light) _light.intensity = _nowSettingPackage.SettingSlider.value; }//���C�g
    }
    /// <summary>
    /// �ݒ��ʂ̔w�i�ύX
    /// </summary>
    private void BackImageEnabled(int falseNumber,int trueNumber)
    {

        //�w�i��\��
        _settingPackage[falseNumber].SettingImageBack.enabled = false;
        //�w�i���\��
        _settingPackage[trueNumber].SettingImageBack.enabled = true;
    }

    /* [ �R���[�`���֘A ] */
    /// <summary>
    /// �Q�[���̍ċN��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetGame()
    {

        //�ҋ@
        yield return new WaitForSeconds(_gameEndTime);

        //�V�[���̍ēǂݍ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    /// <summary>
    /// �Q�[���̏I��
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndGame()
    {

        //�ҋ@
        yield return new WaitForSeconds(_gameEndTime);

        //�Q�[���̏I��
        Application.Quit();
        print("�Q�[���̏I��");
    }
}
