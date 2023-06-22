using UnityEngine;



/// <summary>
/// 看板の管理クラス
/// </summary>
public class GimmickSignboard : MonoBehaviour
{

    private GimmickSignboardController _gimmickSignboardController;//GimmickSignboard管理クラス
    private int _thisNumber = default;//この看板の配列番号
    private string _playerTag = "Player";//プレイヤータグ



    //外部参照----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// GimmickSignboard管理クラス
    /// </summary>
    public GimmickSignboardController SignboardController { set { _gimmickSignboardController = value; } }
    /// <summary>
    /// 看板の配列番号
    /// </summary>
    public int SignboardNumber { set { _thisNumber = value; } }


    //メソッド部----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {

        //衝突対象がプレイヤーなら攻撃
        if (other.gameObject.tag == _playerTag) { _gimmickSignboardController.ViewText(true, _thisNumber); }
    }

    private void OnTriggerExit(Collider other)
    {

        //衝突対象がプレイヤーなら攻撃
        if (other.gameObject.tag == _playerTag) { _gimmickSignboardController.ViewText(false, _thisNumber); }
    }
}
