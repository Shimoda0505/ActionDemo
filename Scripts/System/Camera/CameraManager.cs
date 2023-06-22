using UnityEngine;
using System.Collections.Generic;
using System.Collections;




/// <summary>
/// カメラの挙動管理クラス
/// </summary>
public class CameraManager : MonoBehaviour
{

    [Header("コンポーネント関連")]
    [SerializeField, Tooltip("カメラの変数")] private CameraData _cameraData;
    [SerializeField, Tooltip("Canvas制御クラス")] private CanvasManager _canvasManager;


    [Header("カメラ操作関連")]
    private Quaternion _rotation = default;//X軸回転角度の保存用変数
    private RaycastHit hit;//衝突したオブジェクト

    [Header("視点関連")]
    private const string _playerTag = "Player";
    private Transform _targetTr;//追従対象のTransform


    [Header("ロックオン関連")]
    private Vector2 _rectCenter = new Vector2(0.5f, 0.5f);//スクリーン座標の中心
    private GameObject _nowLockOnTarget = default;//現在のロックオン対象
    private float _centerRange = default;//現在のロックオン対象の中心距離
    private List<GameObject> _lockOnTargets = new List<GameObject>();//ロックオン範囲のターゲットを格納
    private bool _isChangeLock = false;//ロックオン対象の更新を行ったか

    private enum MotionEnum
    {
        [InspectorName("停止")] STOP,
        [InspectorName("移動")] MOVE,
        [InspectorName("ロックオン")] LOCK_ON,
    }
    private MotionEnum _motionEnum = default;



    //処理部---------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {

        //PlayerのTransform
        _targetTr = GameObject.FindWithTag(_playerTag).gameObject.transform;

        //カメラを初期位置に移動
        //入力の水平方向を取得してカメラを水平に回転
        transform.RotateAround(_targetTr.position, Vector3.up, 0);
        //入力の垂直方向を取得してカメラを上下に回転
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0f);
    }



    //メソッド部-----------------------------------------------------------------------------------------------------------------------------------------------------------
    //パブリック----------------------------------------------------------------------------------------
    /// <summary>
    /// カメラの入力管理メソッド
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

                    //移動入力時に移動enumに変更
                    if (isMove) { _motionEnum = MotionEnum.MOVE; }
                    //ロックオン入力時に、対象がいればロックオン
                    if (isLock) { StartLockOn(); }
                    break;


                case MotionEnum.MOVE:

                    //移動未入力時に停止enumに変更
                    if (!isMove) { _motionEnum = MotionEnum.STOP; }
                    //ロックオン入力時に、対象がいればロックオン
                    if (isLock) { StartLockOn(); }
                    break;


                case MotionEnum.LOCK_ON:

                    //ロックオンの終了処理
                    if (isLock) { EndLockOn(isMove); }

                    //配列要素数が0ならロックオンの終了処理
                    if (_lockOnTargets.Count < 0) { EndLockOn(isMove); }

                    //配列を探索して、ロックオン中の対象があるか
                    for (int i = 0; i < _lockOnTargets.Count; i++)
                    {

                        //あれば探索終了
                        if (_lockOnTargets[i] == _nowLockOnTarget) { break; }

                        //なければロックオンの終了処理
                        if (i >= _lockOnTargets.Count - 1) { EndLockOn(isMove); }
                    }

                    //入力方向に応じて、次のロックオン対象を設定
                    if (isRightStick) { NextCameraLockOn(rightStick); }
                    break;


                default:

                    print("MotionEnumのcaseなし");
                    break;
            }
        }
        catch { print("CameraInput"); }
    }
    /// <summary>
    /// カメラの移動管理メソッド
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

                    //入力の水平方向を取得してカメラを水平に回転
                    _rotation.y = moveInput.x * _cameraData.InputSpeed();
                    transform.RotateAround(_targetTr.position, Vector3.up, _rotation.y);
                    //入力の垂直方向を取得してカメラを上下に回転
                    _rotation.x -= moveInput.y * _cameraData.InputSpeed();
                    _rotation.x = Mathf.Clamp(_rotation.x, -_cameraData.XAxisClamp().min, _cameraData.XAxisClamp().max);

                    //向きの反映
                    transform.rotation = Quaternion.Euler(_rotation.x, transform.eulerAngles.y, 0f);
                    break;


                case MotionEnum.LOCK_ON:

                    //向きをターゲット方向に
                    _rotation = Quaternion.LookRotation(_nowLockOnTarget.transform.position - transform.position, Vector3.up);
                    _rotation.x = Mathf.Clamp(_rotation.x, -_cameraData.XAxisClamp().min, _cameraData.XAxisClamp().max);

                    ////向きの反映
                    //transform.rotation = Quaternion.Euler(_rotation.x, transform.eulerAngles.y, 0f);

                    //向きの反映
                    transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(_rotation.x,_rotation.y,_rotation.z,_rotation.w), _cameraData.MoveSpeed() * Time.deltaTime);
                    break;


                default:

                    print("MotionEnumのcaseなし");
                    break;
            }


            // カメラをプレーヤーから一定速度で一定距離に追従
            transform.position = _targetTr.position - transform.forward * _cameraData.TargetRangeOffset() + transform.up * _cameraData.TargetHeightOffset().nomal;

            //プレーヤーとカメラの間に障害物があったら障害物の位置にカメラを移動させる
            if (Physics.Linecast(_targetTr.position, transform.position, out hit, _cameraData.CameraLayerMask())) { transform.position = hit.point + transform.up * _cameraData.TargetHeightOffset().hit; }
        }
        catch { print("CameraMove"); }
    }
    /// <summary>
    /// カメラが閣内なら格納
    /// </summary>
    /// <param name="isView"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool CameraTargetList(bool isView, GameObject obj)
    {

        //オブジェクトがnullなら処理しない
        if (!obj) { return false; }

        //配列に格納
        if (isView) { _lockOnTargets.Add(obj); return true; }
        //指定番号のListを削除
        else { for (int i = 0; i < _lockOnTargets.Count; i++) { if (_lockOnTargets[i] == obj) { _lockOnTargets.RemoveAt(i); } } return false; }
    }

    //プライベート----------------------------------------------------------------------------------------
    /// <summary>
    /// ロックオン対象を探索
    /// </summary>
    /// <returns></returns>
    private GameObject CameraLockOn()
    {

        //0番地の対象を格納
        _nowLockOnTarget = _lockOnTargets[0];
        //スクリーン座標を、カメラView座標(0~1,0~1)に変換し、中心からの距離に変換
        _centerRange = Vector2.Distance(Camera.main.WorldToViewportPoint(_nowLockOnTarget.transform.position), _rectCenter);

        for (int i = 1; i < _lockOnTargets.Count; i++)
        {

            //スクリーン座標を、カメラView座標(0~1,0~1)に変換し、中心からの距離に変換
            float nextDistance = Vector2.Distance(Camera.main.WorldToViewportPoint(_lockOnTargets[i].transform.position), _rectCenter);
            //現在ロックオン中の対象よりも中心に近いオブジェクトがあった場合更新
            if (_centerRange > nextDistance) { _nowLockOnTarget = _lockOnTargets[i]; _centerRange = nextDistance; }
        }

        return _nowLockOnTarget;
    }
    /// <summary>
    /// 入力方向に応じて、次のロックオン対象を設定
    /// </summary>
    private void NextCameraLockOn(Vector2 rightStick)
    {

        //ロックオン対象の更新直後は処理をしない
        if (_isChangeLock) { return; }

        //現在のロックオン対象の画面X座標
        float nowLockOnTargetRectX = Camera.main.WorldToViewportPoint(_nowLockOnTarget.transform.position).x;
        //次のロックオン対象と画面座標の格納変数初期化(画面座標が0~1の為、1以上の数値なら何でもよい)
        float nextLockOnTargetRectX = 1;
        //新しいロックオン対象
        GameObject newLockOnTarget = _nowLockOnTarget;

        for (int i = 0; i < _lockOnTargets.Count; i++)
        {

            //現在のロックオン対象でなければ
            if(_lockOnTargets[i] != _nowLockOnTarget)
            {

                //現在のロックオン対象からみて、左右どちらにあるか
                float newLockOnTargetRange =  Camera.main.WorldToViewportPoint(_lockOnTargets[i].transform.position).x - nowLockOnTargetRectX;

                //画面右
                if (rightStick.x > 0 && newLockOnTargetRange > 0)
                {

                    //中心に近いの右方向なら
                    if(nextLockOnTargetRectX > newLockOnTargetRange) 
                    {

                        //1番近い対象を更新
                        newLockOnTarget = _lockOnTargets[i]; 
                        //1番近い対象の長さを更新
                        nextLockOnTargetRectX = newLockOnTargetRange;
                    }
                }
                //画面左
                else if(rightStick.x < 0 && newLockOnTargetRange < 0)
                {

                    //中心に近いの左方向なら
                    if (nextLockOnTargetRectX > newLockOnTargetRange * -1) 
                    {

                        //1番近い対象を更新
                        newLockOnTarget = _lockOnTargets[i];
                        //1番近い対象の長さを更新
                        nextLockOnTargetRectX = newLockOnTargetRange * -1;
                    }
                }
            }
        }

        //ロックオン対象の変更
        _nowLockOnTarget = newLockOnTarget;

        //ロックオン更新
        _isChangeLock = true;
        //ロックオン画像の更新
        _canvasManager.StartLockOn(true, _nowLockOnTarget);
        //ロックオン更新終了コルーチン開始
        StartCoroutine("EndLockOnInterval");
    }
    /// <summary>
    /// ロックオンの開始処理
    /// </summary>
    private void StartLockOn()
    {

        if (CameraLockOn() != null)
        {

            //ロックオンenumに遷移
            _motionEnum = MotionEnum.LOCK_ON;
            //ロックオン画像の更新
            _canvasManager.StartLockOn(true, _nowLockOnTarget);
        }
    }
    /// <summary>
    /// ロックオンの終了処理
    /// </summary>
    /// <param name="isMove"></param>
    private void EndLockOn(bool isMove)
    {

        //現在のロックオン対象をnull
        _nowLockOnTarget = null;

        //ロックオン画像の更新
        _canvasManager.StartLockOn(false, null);

        //移動入力時に移動enumに変更
        if (isMove) { _motionEnum = MotionEnum.MOVE; }
        //移動未入力時に停止enumに変更
        else { _motionEnum = MotionEnum.STOP; }
    }

    /// <summary>
    /// ロックオン更新終了
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndLockOnInterval()
    {

        //待機
        yield return new WaitForSeconds(_cameraData.LockOnInterval());

        //ロックオン更新終了
        _isChangeLock = false;
    }
}

