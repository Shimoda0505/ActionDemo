using UnityEngine;


/// <summary>
/// �G�̊��N���X
/// </summary>
public class EnemyState : CharacterState
{

    [Header("�X�e�[�^�X�֘A(EnemyState)")]
    [SerializeField, Tooltip("�U����")] protected int _attack;
    private const string _playerManagerTag = "PlayerManager";//PlayerManager�̃^�O
    protected PlayerManager _playerManager = default;//CharacterState�N���X
    private const string _playerTag = "Player";//�v���C���[�̃^�O
    protected GameObject _playerObj = default;//�v���[���[�I�u�W�F�N�g
    private CameraManager _cameraManager = default;//�J�����̋����Ǘ��N���X


    [Header("���b�N�I���֘A(EnemyState)")]
    [SerializeField, Tooltip("���b�N�I���Ԃ�j�ޏ�Q��Layer")] private LayerMask _layerMask = default;
    [SerializeField, Tooltip("���b�N�I���͈͂̍ő勗��"),Range(0,100)] private float _lockOnMaxDistance = default;
    [SerializeField, Tooltip("���b�N�I���͈͂̍ŏ�����"),Range(0,100)] private float _lockOnMinDistance = default;
    private Vector3 _rectTransform = default;//���̃I�u�W�F�N�g�̃J�������W
    private bool _isLock = false;//���b�N�I������
    private bool  _isHit = false;//�Փ˂������ǂ���
    private bool _isRect;//�J�����̉�p��
    private float _distancePlayer = default;//�v���C���[�Ƃ̋���


    //������---------------------------------------------------------------------------------------------------------------------------------------------------------------

    protected override void Start()
    {

        //���
        base.Start();

        //�擾�֘A
        _playerManager = GameObject.FindWithTag(_playerManagerTag).GetComponent<PlayerManager>();//PlayerManager�N���X
        _playerObj = GameObject.FindWithTag(_playerTag).gameObject;//�v���[���[�I�u�W�F�N�g
        _cameraManager = Camera.main.GetComponent<CameraManager>();//�J�����̋����Ǘ��N���X
    }

    private void Update()
    {

        //�v���C���[�Ƃ̋���
        _distancePlayer = Vector3.Distance(_playerObj.transform.position, transform.position);
        //�������͈͓��Ȃ�
        if (_lockOnMaxDistance >= _distancePlayer && _distancePlayer >= _lockOnMinDistance)
        {

            //�v���C���[�Ƃ̊Ԃɑ��I�u�W�F�N�g�����݂��Ȃ����true
            _isHit = !Physics.Linecast(_playerObj.transform.position + transform.up, transform.position + transform.up, _layerMask);
            //Linecast��`��
            Debug.DrawLine(_playerObj.transform.position + transform.up, transform.position + transform.up, _isHit ? Color.red : Color.green);

            //���̃I�u�W�F�N�g�̃J�������W
            _rectTransform = Camera.main.WorldToViewportPoint(transform.position);
            //�J�����̉�p���Ȃ�true
            _isRect = 1 > _rectTransform.x && _rectTransform.x > 0 && 1 > _rectTransform.y && _rectTransform.y > 0 && _rectTransform.z > 0;

            //ray��hit���E�J�����̉�p���E���b�N�I�����ł͂Ȃ� �� List�Ɋi�[
            if (_isHit && _isRect && !_isLock) { _isLock = _cameraManager.CameraTargetList(true, gameObject); }
            //���b�N�I�����E!ray��hit���E!�J�����̉�p�� �� List�������
            if (_isLock) { if (!_isHit || !_isRect) { _isLock = _cameraManager.CameraTargetList(false, gameObject); } }
        }
        //�������͈͊O�Ȃ�
        else
        {

            //���b�N�I�����E!ray��hit���E!�J�����̉�p�� �� List�������
            if (_isLock) { _isLock = _cameraManager.CameraTargetList(false, gameObject); }
        }
    }
}
