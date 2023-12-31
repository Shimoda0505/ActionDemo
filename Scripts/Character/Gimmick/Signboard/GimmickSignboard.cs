using UnityEngine;



/// <summary>
/// ΕΒΜΗNX
/// </summary>
public class GimmickSignboard : MonoBehaviour
{

    private GimmickSignboardController _gimmickSignboardController;//GimmickSignboardΗNX
    private int _thisNumber = default;//±ΜΕΒΜzρΤ
    private string _playerTag = "Player";//vC[^O



    //OQΖ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// GimmickSignboardΗNX
    /// </summary>
    public GimmickSignboardController SignboardController { set { _gimmickSignboardController = value; } }
    /// <summary>
    /// ΕΒΜzρΤ
    /// </summary>
    public int SignboardNumber { set { _thisNumber = value; } }


    //\bh----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {

        //ΥΛΞΫͺvC[ΘηU
        if (other.gameObject.tag == _playerTag) { _gimmickSignboardController.ViewText(true, _thisNumber); }
    }

    private void OnTriggerExit(Collider other)
    {

        //ΥΛΞΫͺvC[ΘηU
        if (other.gameObject.tag == _playerTag) { _gimmickSignboardController.ViewText(false, _thisNumber); }
    }
}
