using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;



public class GimmickSignboardController : MonoBehaviour
{

    [Header("参照関連")]
    [SerializeField, Tooltip("テキスト背景")] private GameObject _textBackImage;
    [SerializeField, Tooltip("テキスト表示キャンバス")] private Text _text;


    [Header("看板のパッケージ関連")]
    [SerializeField] private List<SignboardPackage> _signboardPackage = new List<SignboardPackage>();
    [Serializable, Tooltip("看板のパッケージ")]
    public class SignboardPackage
    {
        [field: SerializeField, Tooltip("看板のオブジェクト")] private GameObject _signboardObj;
        [field: SerializeField, Tooltip("表示するテキスト"), TextArea(1, 2)] private string _viewText;


        /// <summary>
        /// 看板のオブジェクト
        /// </summary>
        public GameObject SignboardObj { get { return _signboardObj; } }
        /// <summary>
        /// 表示するテキスト
        /// </summary>
        public string ViewText { get { return _viewText; } }
    }
    private SignboardPackage _signboard = default;//使用中のSignboardPackage



    private void Start()
    {

        //テキスト背景の非表示
        _textBackImage.SetActive(false);

        for (int i = 0; i < _signboardPackage.Count; i++)
        {

            //看板の管理クラスを取得
            GimmickSignboard gimmickSignboard = _signboardPackage[i].SignboardObj.GetComponent<GimmickSignboard>();

            //このクラス
            gimmickSignboard.SignboardController = this.gameObject.GetComponent<GimmickSignboardController>();
            //各々の箱番号
            gimmickSignboard.SignboardNumber = i;
        }
    }



    //メソッド部----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// テキストの表示
    /// </summary>
    public void ViewText(bool isView,int number)
    {

        //使用中のSignboardPackageを更新
        _signboard = _signboardPackage[number];

        //テキストの表示・テキストの設定
        if (isView) { _textBackImage.SetActive(true); _text.text = _signboard.ViewText; }
        //テキストの非表示
        else { _textBackImage.SetActive(false); }
    }
}
