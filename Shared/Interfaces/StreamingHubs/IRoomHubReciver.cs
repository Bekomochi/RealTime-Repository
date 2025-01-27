using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MagicOnion;
using Server.Model.Entity;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHubReciver
    {
        //==============================================//
        //ここにサーバー側からクライアントを呼び出す関数を定義する//
        //============================================//
       
        //ユーザーの入室を通知
        void OnJoin(JoinedUser user);

        //マッチング通知
        void OnMatching(string roomName); //ルーム名をクライアントに渡す

        //ユーザーの退室を通知
        void OnLeave(LeavedUser user);

        //public enum CharacterState
        //{
        //    Idle=0,
        //    Walk=1,
        //    Attack=2
        //}

        //位置、回転、状態をクライアントに通知する
        void OnMove(MovedUser movedUser/*,CharacterState state*/);//MovedUserクラスに接続ID、位置、回転の情報が入っている

        //アニメーターのstateを、列挙型で定義

        //ゲーム開始を全員に通知
        void OnReady();

        //ゲーム準備を通知
        void OnPreparation();

        //ゲーム終了を通知
        void OnFinish();

        //HPの更新を通知
        void OnHPValue(int hp);

        //水鉄砲の発射を通知
        void OnShot();
    }
}