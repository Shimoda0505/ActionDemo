using UnityEngine;


/// <summary>
/// カメラに関するデータを格納するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "CameraData_", menuName = "ScriptableObjects/CameraData")]
public class CameraData : ScriptableObject
{

    [Header("カメラ操作関連")]
    [SerializeField, Tooltip("カメラの操作速度"), Range(0.1f, 1f)] private float _inputSpeed;
    [SerializeField, Tooltip("カメラの追従速度"), Range(0.1f, 20f)] private float _moveSpeed;
    [SerializeField, Tooltip("追従対象との距離オフセット"), Range(1, 10)] private int _targetRangeOffset;
    [SerializeField, Tooltip("追従対象との高さオフセット"), Range(1, 10)] private int _targetHeightOffset;
    [SerializeField, Tooltip("追従対象との高さオフセット(コライダーヒット時)"), Range(1, 10)] private int _targetHitHeightOffset;
    [SerializeField, Tooltip("カメラのX軸の最大制限"), Range(1, 89)] private int _xMaxClamp;
    [SerializeField, Tooltip("カメラのX軸の最小制限"), Range(1, 89)] private int _xMinClamp;

    [Header("カメラコライダー関連")]
    [SerializeField, Tooltip("カメラコライダーのマスク")] private LayerMask _layerMask;

    [Header("ロックオン関連")]
    [SerializeField, Tooltip("ロックオン後のインターバル"),Range(0,10f)] private float _lockOnInterval;



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// カメラ速度の変更
    /// </summary>
    public void ChangeInputSpeed(float speed) { _inputSpeed = speed; }
    /// <summary>
    /// カメラの操作速度
    /// </summary>
    public float InputSpeed() { return _inputSpeed; }
    /// <summary>
    /// カメラの追従速度
    /// </summary>
    public float MoveSpeed() { return _moveSpeed; }
    /// <summary>
    /// 追従対象との距離オフセット
    /// </summary>
    public int TargetRangeOffset() { return _targetRangeOffset; }
    /// <summary>
    /// 追従対象との高さオフセット
    /// </summary>
    public (int nomal, int hit) TargetHeightOffset() { return (_targetHeightOffset, _targetHitHeightOffset); }
    /// <summary>
    /// カメラのX軸の制限
    /// </summary>
    public (int max,int min) XAxisClamp() { return (_xMaxClamp, _xMinClamp); }
    /// <summary>
    /// カメラコライダーのマスク
    /// </summary>
    public LayerMask CameraLayerMask() { return _layerMask; }
    /// <summary>
    /// ロックオン後のインターバル
    /// </summary>
    /// <returns></returns>
    public float LockOnInterval() { return _lockOnInterval; }
}
