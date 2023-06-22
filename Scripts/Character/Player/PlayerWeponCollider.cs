using UnityEngine;
using System.Collections;



public class PlayerWeponCollider : MonoBehaviour
{

    
    private const string _playerTag = "Player";//プレイヤーのタグ
    private int _attack = default;//攻撃力
    private float _hitInterval = default;//連続ヒットさせないためのインターバル
    private GameObject _hitObj = default;//ヒットした対象


    //メソッド部----------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// 判定の有無,攻撃力の変更
    /// </summary>
    public void ChangeCollider(bool isCollider, int attack)
    {

        try
        {

            //攻撃力の変更
            _attack = attack;

            //判定無し
            if (!isCollider) { gameObject.GetComponent<Collider>().enabled = false; }
            //判定有り
            else if (isCollider) { gameObject.GetComponent<Collider>().enabled = true; }
        }
        catch { print("PlayerWeponColliderでエラー"); }
    }

    private void OnTriggerEnter(Collider other)
    {
        try
        {

            //衝突先がプレイヤーなら処理しない
            if (other.gameObject.tag == _playerTag) { return; }
            //前回と同じ対象なら処理しない
            if (_hitObj != null) { if (_hitObj == other.gameObject) { return; } }

            //CharacterStateを保持しているオブジェクトなら処理
            if (other.gameObject.GetComponent<CharacterState>())
            {

                print("プレイヤーが" + other.gameObject + "に" + _attack + "のダメージ");

                //攻撃処理
                other.gameObject.GetComponent<CharacterState>().Damage(_attack);
                //ヒットした対象
                _hitObj = other.gameObject;
            }
        }
        catch { print("PlayerWeponColliderでエラー"); }
    }

    public IEnumerator EndAttack()
    {

        //待機
        yield return new WaitForSeconds(_hitInterval);

        //ヒットした対象
        _hitObj = null;
    }
}
