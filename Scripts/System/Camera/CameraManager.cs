using UnityEngine;
using System.Collections.Generic;
using System.Collections;




/// <summary>
/// �J�����̋����Ǘ��N���X
/// </summary>
public class CameraManager : MonoBehaviour
{

    [Header("�R���|�[�l���g�֘A")]
    [SerializeField, Tooltip("�J�����̕ϐ�")] private CameraData _cameraData;
    [SerializeField, Tooltip("Canvas����N���X")] private CanvasManager _canvasManager;


    [Header("�J��������֘A")]
    private Quaternion _rotation = default;//X����]�p�x�̕ۑ��p�ϐ�
    private RaycastHit hit;//�Փ˂����I�u�W�F�N�g

    [Header("���_�֘A")]
    private const string _playerTag = "Player";
    private Transform _targetTr;//�Ǐ]�Ώۂ�Transform


    [Header("���b�N�I���֘A")]
    private Vector2 _rectCenter = new Vector2(0.5f, 0.5f);//�X�N���[�����W�̒��S
    private GameObject _nowLockOnTarget = default;//���݂̃��b�N�I���Ώ�
    private float _centerRange = default;//���݂̃��b�N�I���Ώۂ̒��S����
    private List<GameObject> _lockOnTargets = new List<GameObject>();//���b�N�I���͈͂̃^�[�Q�b�g���i�[
    private bool _isChangeLock = false;//���b�N�I���Ώۂ̍X�V���s������

    private enum MotionEnum
    {
        [InspectorName("��~")] STOP,
        [InspectorName("�ړ�")] MOVE,
        [InspectorName("���b�N�I��")] LOCK_ON,
    }
    private MotionEnum _motionEnum = default;



    //������---------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //Player��Transform
        _targetTr = GameObject.FindWithTag(_playerTag).gameObject.transform;

        //�J�����������ʒu�Ɉړ�
        //���͂̐����������擾���ăJ�����𐅕��ɉ�]
        transform.RotateAround(_targetTr.position, Vector3.up, 0);
        //���͂̐����������擾���ăJ�������㉺�ɉ�]
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0f);
    }



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //�p�u���b�N----------------------------------------------------------------------------------------
    /// <summary>
    /// �J�����̓��͊Ǘ����\�b�h
    /// </summary>
    /// <param name="isMove"></param>
    /// <param name="isLock"></param>
    public void CameraInput(bool isMove, bool isLock, bool isRightStick, Vector2 rightStick)
    {

        try
        {

            switch (_motionEnum)
            {
                case MotionEnum.STOP:

                    //�ړ����͎��Ɉړ�enum�ɕύX
                    if (isMove) { _motionEnum = MotionEnum.MOVE; }
                    //���b�N�I�����͎��ɁA�Ώۂ�����΃��b�N�I��
                    if (isLock) { StartLockOn(); }
                    break;


                case MotionEnum.MOVE:

                    //�ړ������͎��ɒ�~enum�ɕύX
                    if (!isMove) { _motionEnum = MotionEnum.STOP; }
                    //���b�N�I�����͎��ɁA�Ώۂ�����΃��b�N�I��
                    if (isLock) { StartLockOn(); }
                    break;


                case MotionEnum.LOCK_ON:

                    //���b�N�I���̏I������
                    if (isLock) { EndLockOn(isMove); }

                    //�z��v�f����0�Ȃ烍�b�N�I���̏I������
                    if (_lockOnTargets.Count < 0) { EndLockOn(isMove); }

                    //�z���T�����āA���b�N�I�����̑Ώۂ����邩
                    for (int i = 0; i < _lockOnTargets.Count; i++)
                    {

                        //����ΒT���I��
                        if (_lockOnTargets[i] == _nowLockOnTarget) { break; }

                        //�Ȃ���΃��b�N�I���̏I������
                        if (i >= _lockOnTargets.Count - 1) { EndLockOn(isMove); }
                    }

                    //���͕����ɉ����āA���̃��b�N�I���Ώۂ�ݒ�
                    if (isRightStick) { NextCameraLockOn(rightStick); }
                    break;


                default:

                    print("MotionEnum��case�Ȃ�");
                    break;
            }
        }
        catch { print("CameraInput"); }
    }
    /// <summary>
    /// �J�����̈ړ��Ǘ����\�b�h
    /// </summary>
    /// <param name="moveInput"></param>
    public void CameraMove(Vector2 moveInput)
    {

        try
        {

            switch (_motionEnum)
            {

                case MotionEnum.STOP:


                    break;


                case MotionEnum.MOVE:

                    //���͂̐����������擾���ăJ�����𐅕��ɉ�]
                    _rotation.y = moveInput.x * _cameraData.InputSpeed();
                    transform.RotateAround(_targetTr.position, Vector3.up, _rotation.y);
                    //���͂̐����������擾���ăJ�������㉺�ɉ�]
                    _rotation.x -= moveInput.y * _cameraData.InputSpeed();
                    _rotation.x = Mathf.Clamp(_rotation.x, -_cameraData.XAxisClamp().min, _cameraData.XAxisClamp().max);

                    //�����̔��f
                    transform.rotation = Quaternion.Euler(_rotation.x, transform.eulerAngles.y, 0f);
                    break;


                case MotionEnum.LOCK_ON:

                    //�������^�[�Q�b�g������
                    _rotation = Quaternion.LookRotation(_nowLockOnTarget.transform.position - transform.position, Vector3.up);
                    _rotation.x = Mathf.Clamp(_rotation.x, -_cameraData.XAxisClamp().min, _cameraData.XAxisClamp().max);

                    ////�����̔��f
                    //transform.rotation = Quaternion.Euler(_rotation.x, transform.eulerAngles.y, 0f);

                    //�����̔��f
                    transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(_rotation.x,_rotation.y,_rotation.z,_rotation.w), _cameraData.MoveSpeed() * Time.deltaTime);
                    break;


                default:

                    print("MotionEnum��case�Ȃ�");
                    break;
            }


            // �J�������v���[���[�����葬�x�ň�苗���ɒǏ]
            transform.position = _targetTr.position - transform.forward * _cameraData.TargetRangeOffset() + transform.up * _cameraData.TargetHeightOffset().nomal;

            //�v���[���[�ƃJ�����̊Ԃɏ�Q�������������Q���̈ʒu�ɃJ�������ړ�������
            if (Physics.Linecast(_targetTr.position, transform.position, out hit, _cameraData.CameraLayerMask())) { transform.position = hit.point + transform.up * _cameraData.TargetHeightOffset().hit; }
        }
        catch { print("CameraMove"); }
    }
    /// <summary>
    /// �J�������t���Ȃ�i�[
    /// </summary>
    /// <param name="isView"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool CameraTargetList(bool isView, GameObject obj)
    {

        //�I�u�W�F�N�g��null�Ȃ珈�����Ȃ�
        if (!obj) { return false; }

        //�z��Ɋi�[
        if (isView) { _lockOnTargets.Add(obj); return true; }
        //�w��ԍ���List���폜
        else { for (int i = 0; i < _lockOnTargets.Count; i++) { if (_lockOnTargets[i] == obj) { _lockOnTargets.RemoveAt(i); } } return false; }
    }

    //�v���C�x�[�g----------------------------------------------------------------------------------------
    /// <summary>
    /// ���b�N�I���Ώۂ�T��
    /// </summary>
    /// <returns></returns>
    private GameObject CameraLockOn()
    {

        //0�Ԓn�̑Ώۂ��i�[
        _nowLockOnTarget = _lockOnTargets[0];
        //�X�N���[�����W���A�J����View���W(0~1,0~1)�ɕϊ����A���S����̋����ɕϊ�
        _centerRange = Vector2.Distance(Camera.main.WorldToViewportPoint(_nowLockOnTarget.transform.position), _rectCenter);

        for (int i = 1; i < _lockOnTargets.Count; i++)
        {

            //�X�N���[�����W���A�J����View���W(0~1,0~1)�ɕϊ����A���S����̋����ɕϊ�
            float nextDistance = Vector2.Distance(Camera.main.WorldToViewportPoint(_lockOnTargets[i].transform.position), _rectCenter);
            //���݃��b�N�I�����̑Ώۂ������S�ɋ߂��I�u�W�F�N�g���������ꍇ�X�V
            if (_centerRange > nextDistance) { _nowLockOnTarget = _lockOnTargets[i]; _centerRange = nextDistance; }
        }

        return _nowLockOnTarget;
    }
    /// <summary>
    /// ���͕����ɉ����āA���̃��b�N�I���Ώۂ�ݒ�
    /// </summary>
    private void NextCameraLockOn(Vector2 rightStick)
    {

        //���b�N�I���Ώۂ̍X�V����͏��������Ȃ�
        if (_isChangeLock) { return; }

        //���݂̃��b�N�I���Ώۂ̉��X���W
        float nowLockOnTargetRectX = Camera.main.WorldToViewportPoint(_nowLockOnTarget.transform.position).x;
        //���̃��b�N�I���ΏۂƉ�ʍ��W�̊i�[�ϐ�������(��ʍ��W��0~1�ׁ̈A1�ȏ�̐��l�Ȃ牽�ł��悢)
        float nextLockOnTargetRectX = 1;
        //�V�������b�N�I���Ώ�
        GameObject newLockOnTarget = _nowLockOnTarget;

        for (int i = 0; i < _lockOnTargets.Count; i++)
        {

            //���݂̃��b�N�I���ΏۂłȂ����
            if(_lockOnTargets[i] != _nowLockOnTarget)
            {

                //���݂̃��b�N�I���Ώۂ���݂āA���E�ǂ���ɂ��邩
                float newLockOnTargetRange =  Camera.main.WorldToViewportPoint(_lockOnTargets[i].transform.position).x - nowLockOnTargetRectX;

                //��ʉE
                if (rightStick.x > 0 && newLockOnTargetRange > 0)
                {

                    //���S�ɋ߂��̉E�����Ȃ�
                    if(nextLockOnTargetRectX > newLockOnTargetRange) 
                    {

                        //1�ԋ߂��Ώۂ��X�V
                        newLockOnTarget = _lockOnTargets[i]; 
                        //1�ԋ߂��Ώۂ̒������X�V
                        nextLockOnTargetRectX = newLockOnTargetRange;
                    }
                }
                //��ʍ�
                else if(rightStick.x < 0 && newLockOnTargetRange < 0)
                {

                    //���S�ɋ߂��̍������Ȃ�
                    if (nextLockOnTargetRectX > newLockOnTargetRange * -1) 
                    {

                        //1�ԋ߂��Ώۂ��X�V
                        newLockOnTarget = _lockOnTargets[i];
                        //1�ԋ߂��Ώۂ̒������X�V
                        nextLockOnTargetRectX = newLockOnTargetRange * -1;
                    }
                }
            }
        }

        //���b�N�I���Ώۂ̕ύX
        _nowLockOnTarget = newLockOnTarget;

        //���b�N�I���X�V
        _isChangeLock = true;
        //���b�N�I���摜�̍X�V
        _canvasManager.StartLockOn(true, _nowLockOnTarget);
        //���b�N�I���X�V�I���R���[�`���J�n
        StartCoroutine("EndLockOnInterval");
    }
    /// <summary>
    /// ���b�N�I���̊J�n����
    /// </summary>
    private void StartLockOn()
    {

        if (CameraLockOn() != null)
        {

            //���b�N�I��enum�ɑJ��
            _motionEnum = MotionEnum.LOCK_ON;
            //���b�N�I���摜�̍X�V
            _canvasManager.StartLockOn(true, _nowLockOnTarget);
        }
    }
    /// <summary>
    /// ���b�N�I���̏I������
    /// </summary>
    /// <param name="isMove"></param>
    private void EndLockOn(bool isMove)
    {

        //���݂̃��b�N�I���Ώۂ�null
        _nowLockOnTarget = null;

        //���b�N�I���摜�̍X�V
        _canvasManager.StartLockOn(false, null);

        //�ړ����͎��Ɉړ�enum�ɕύX
        if (isMove) { _motionEnum = MotionEnum.MOVE; }
        //�ړ������͎��ɒ�~enum�ɕύX
        else { _motionEnum = MotionEnum.STOP; }
    }

    /// <summary>
    /// ���b�N�I���X�V�I��
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndLockOnInterval()
    {

        //�ҋ@
        yield return new WaitForSeconds(_cameraData.LockOnInterval());

        //���b�N�I���X�V�I��
        _isChangeLock = false;
    }
}

