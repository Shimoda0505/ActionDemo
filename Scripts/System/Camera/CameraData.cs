using UnityEngine;


/// <summary>
/// �J�����Ɋւ���f�[�^���i�[����ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "CameraData_", menuName = "ScriptableObjects/CameraData")]
public class CameraData : ScriptableObject
{

    [Header("�J��������֘A")]
    [SerializeField, Tooltip("�J�����̑��쑬�x"), Range(0.1f, 1f)] private float _inputSpeed;
    [SerializeField, Tooltip("�J�����̒Ǐ]���x"), Range(0.1f, 20f)] private float _moveSpeed;
    [SerializeField, Tooltip("�Ǐ]�ΏۂƂ̋����I�t�Z�b�g"), Range(1, 10)] private int _targetRangeOffset;
    [SerializeField, Tooltip("�Ǐ]�ΏۂƂ̍����I�t�Z�b�g"), Range(1, 10)] private int _targetHeightOffset;
    [SerializeField, Tooltip("�Ǐ]�ΏۂƂ̍����I�t�Z�b�g(�R���C�_�[�q�b�g��)"), Range(1, 10)] private int _targetHitHeightOffset;
    [SerializeField, Tooltip("�J������X���̍ő吧��"), Range(1, 89)] private int _xMaxClamp;
    [SerializeField, Tooltip("�J������X���̍ŏ�����"), Range(1, 89)] private int _xMinClamp;

    [Header("�J�����R���C�_�[�֘A")]
    [SerializeField, Tooltip("�J�����R���C�_�[�̃}�X�N")] private LayerMask _layerMask;

    [Header("���b�N�I���֘A")]
    [SerializeField, Tooltip("���b�N�I����̃C���^�[�o��"),Range(0,10f)] private float _lockOnInterval;



    //���\�b�h��-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// �J�������x�̕ύX
    /// </summary>
    public void ChangeInputSpeed(float speed) { _inputSpeed = speed; }
    /// <summary>
    /// �J�����̑��쑬�x
    /// </summary>
    public float InputSpeed() { return _inputSpeed; }
    /// <summary>
    /// �J�����̒Ǐ]���x
    /// </summary>
    public float MoveSpeed() { return _moveSpeed; }
    /// <summary>
    /// �Ǐ]�ΏۂƂ̋����I�t�Z�b�g
    /// </summary>
    public int TargetRangeOffset() { return _targetRangeOffset; }
    /// <summary>
    /// �Ǐ]�ΏۂƂ̍����I�t�Z�b�g
    /// </summary>
    public (int nomal, int hit) TargetHeightOffset() { return (_targetHeightOffset, _targetHitHeightOffset); }
    /// <summary>
    /// �J������X���̐���
    /// </summary>
    public (int max,int min) XAxisClamp() { return (_xMaxClamp, _xMinClamp); }
    /// <summary>
    /// �J�����R���C�_�[�̃}�X�N
    /// </summary>
    public LayerMask CameraLayerMask() { return _layerMask; }
    /// <summary>
    /// ���b�N�I����̃C���^�[�o��
    /// </summary>
    /// <returns></returns>
    public float LockOnInterval() { return _lockOnInterval; }
}
