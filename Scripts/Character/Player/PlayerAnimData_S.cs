using UnityEngine;
using System;


/// <summary>
/// Playerのアニメーション管理構造体
/// </summary>
public  struct PlayerAnimData_S
{

    /// <summary>
    /// アニメーションパラメーター一覧
    /// </summary>
    public enum AnimBoolMode
    {
        [InspectorName("なし")] NULL,
        [InspectorName("待機")] IDLE,
        [InspectorName("歩き")] WALK,
        [InspectorName("走り")] RUN,
        [InspectorName("スキル")] SKILL,
        [InspectorName("回避")] ROLL,
        [InspectorName("ステップ")] STEP,
        [InspectorName("弱攻撃")] ATTACK_WEAK,
        [InspectorName("強攻撃")] ATTACK_STRONG,
        [InspectorName("ガード")] GUARD,
        [InspectorName("ダメージ")] DAMAGE,
        [InspectorName("小ダウン")] DOWN_WEAK,
        [InspectorName("大ダウン")] DOWN_STRONG,
        [InspectorName("死亡")] DEATH,
    }

    public enum AnimComboMode
    {
        [InspectorName("弱コンボ")] COMBO_WEAK,
        [InspectorName("強コンボ")] COMBO_STRONG,
    }


    public enum AnimatorUpdateModeEnum
    {
        [InspectorName("AnimatePhysics")] ANIMATE_PHYSICS,
        [InspectorName("Normal")] NOMAL,
    }


    //メソッド部---------------------------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// BoolAnimationの変更
    /// </summary>
    public void ChangeBoolAnimation(Animator anim, AnimBoolMode animBoolMode )
    {

        //AnimModeのenum種類をカウント
        int enumCount = Enum.GetValues(typeof(AnimBoolMode)).Length;

        //AnimModeの種類分処理
        for (int i = 0; i < enumCount; i++)
        {

            //i番のAnimModeをstring型に変換
            string animName = Enum.GetValues(typeof(AnimBoolMode)).GetValue(i).ToString();

            //animNameのアニメーションを非アクティブ
            anim.SetBool(animName, false);
        }

        //animModeのアニメーションをアクティブ
        anim.SetBool(animBoolMode.ToString(), true);
    }
    /// <summary>
    /// TriggerAnimationの変更
    /// </summary>
    public void ChangeTriggerAnimation(Animator anim, AnimComboMode animCombo)
    {

        //AnimModeのenum種類をカウント
        int enumCount = Enum.GetValues(typeof(AnimBoolMode)).Length;

        //AnimModeの種類分処理
        for (int i = 0; i < enumCount; i++)
        {

            //i番のAnimModeをstring型に変換
            string animName = Enum.GetValues(typeof(AnimBoolMode)).GetValue(i).ToString();

            //animNameのアニメーションを非アクティブ
            anim.SetBool(animName, false);
        }

        //animComboのアニメーションをアクティブ
        anim.SetTrigger(animCombo.ToString());
    }
    /// <summary>
    /// AnimatorのUpdateModeとApplyModeの変更
    /// </summary>
    public void ChangeAnimatorApplyMode(Animator anim, AnimatorUpdateModeEnum animatorUpdateModeEnum)
    {

        //AnimatorのUpdateModeに対応して切り替え
        if (animatorUpdateModeEnum == AnimatorUpdateModeEnum.NOMAL)
        {
            //AnimatorのUpdateModeを切り替え
            //Normal = タイムスケール主体の物理挙動
            anim.updateMode = AnimatorUpdateMode.Normal;
            //AnimatorのApplyRootMotionを切り替え
            //キャラクターの位置と回転をスクリプトから制御するか
            anim.applyRootMotion = false;

        }
        else if(animatorUpdateModeEnum == AnimatorUpdateModeEnum.ANIMATE_PHYSICS)
        {
            //AnimatePhysics = アニメーション主体の物理挙動
            anim.updateMode = AnimatorUpdateMode.AnimatePhysics;
            //AnimatorのApplyRootMotionを切り替え
            //キャラクターの位置と回転をアニメーション自体から制御
            anim.applyRootMotion = true;
        }
    }
}