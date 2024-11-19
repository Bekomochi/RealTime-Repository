﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MagicOnion;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHubReciver
    {
        //===============================================//
        // ここにサーバー側からクライアントを呼び出す関数を定義する //
        //==============================================//
       
        //ユーザーの入室を通知
        void OnJoin(JoinedUser user);
    }
}
