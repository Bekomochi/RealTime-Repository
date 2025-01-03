﻿using MagicOnion;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHub : IStreamingHub<IRoomHub, IRoomHubReciver>
    {
        Task<JoinedUser[]>JoinAsync(string roomName,int userId);

        Task<JoinedUser[]> JoinLobbyAsync(int userID);//ルーム名は固定なので指定不要。最初にjoinasyncを呼ぶ。

        Task<LeavedUser> LeaveAsync();

        //位置、回転をサーバーに送信する
        Task MoveAsync(MovedUser movedUser);

        //ゲーム開始をサーバーに送信する
        Task ReadyAsync();

        //ゲーム終了をサーバーに送信する
        Task FinishAsync();
    }
}
