using System;
using System.Collections.Generic;
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

        //ユーザーの退室を通知
        void OnLeave(LeavedUser user);

        //位置、回転をクライアントに通知する
        //void OnMove(/*メッセージパックオブジェクト作成の可能性あり*/);
    }
}