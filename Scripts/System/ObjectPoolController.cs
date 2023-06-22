using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// オブジェクトプール
/// </summary>
public class ObjectPoolController : MonoBehaviour
{


    [Header("オブジェクトプール関連")]
    [SerializeField, Tooltip("プールに生成したいオブジェクトを代入")] private GameObject _poolObj;
    [SerializeField, Tooltip("最初に生成するオブジェクトの最大数 ")] private int _maxPool;
    private List<GameObject> _poolObjList = new List<GameObject>();//生成したオブジェクト用のリスト



    //処理部-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void Awake()
    {

        //最初に固定数プレハブオブジェクトをインスタント
        //オブジェクトを最大数まで生成
        for (int i = 0; i < _maxPool; i++)
        {

            // オブジェクトを生成して
            GameObject newObj = CreateNewObj();
            //生成したオブジェクトの物理挙動をfalse
            newObj.SetActive(false);
            // リストに保存しておく
            _poolObjList.Add(newObj);
        }
    }



    //メソッド部----------------------------------------------------------------------------------------------------------------------------------------------------------------------
    //パブリックメソッド---------------------------------------------------
    /// <summary>
    /// オブジェクトの配置と返却
    /// </summary>
    /// <param name="tr"></param>
    /// <returns></returns>
    public GameObject GetSetObj(Transform tr)
    {

        //オブジェクトプールから弾を生成
        GameObject obj = GetObj();

        //オブジェクトの配置
        obj.transform.position = tr.position;//位置
        obj.transform.rotation = tr.rotation;//角度

        //呼び出し元のクラスにこのオブジェクトを返す
        return obj;
    }
    /// <summary>
    /// オブジェクトプールから使用するプレハブオブジェクトを取得
    /// </summary>
    /// <returns>プレハブオブジェクト</returns>
    public GameObject GetObj()
    {

        //未使用があれば使用,なければ生成
        //使用中でないものを探して返す
        //リスト内にあるオブジェクトをobjとして返す
        foreach (var obj in _poolObjList)
        {

            //オブジェクトが非アクティブのものを探す
            if (!obj.activeSelf)
            {

                //オブジェクトをアクティブ
                obj.SetActive(true);

                //呼び出し元のクラスにこのオブジェクトを返す
                return obj;
            }
        }

        // 全て使用中だったら新しく作る
        GameObject newObj = CreateNewObj();

        //リストに保存しておく
        _poolObjList.Add(newObj);
        //新しく作ったオブジェクトをアクティブ
        newObj.SetActive(true);

        //呼び出し元のクラスにこのオブジェクトを返す
        return newObj;
    }

    //プライベートメソッド---------------------------------------------------
    /// <summary>
    /// 新しくプレハブオブジェクトをインスタント
    /// </summary>
    /// <returns>新しくプレハブオブジェクト</returns>
    private GameObject CreateNewObj()
    {

        //画面外に生成
        Vector3 pos = this.gameObject.transform.position;
        //新しいオブジェクトを生成(生成したいオブジェクトを,画面外に,親になるオブジェクトと同じ所に)
        GameObject newObj = Instantiate(_poolObj, pos, Quaternion.identity);
        //名前に連番付け(リストに追加された順)
        newObj.name = _poolObj.name + (_poolObjList.Count + 1);

        //呼び出し元のスクリプトにこのオブジェクトを返す
        return newObj;
    }
}
