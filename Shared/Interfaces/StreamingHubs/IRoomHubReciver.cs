using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHubReciver
    {
        //サーバー側からクライアント側を呼び出す関数を定義する

        //ユーザーの入室を通知
        void OnJoin(JoinedUser user);

        //ユーザー入室
        Task<JoinedUser[]>JoinAsync(string roomName,int userID);

        
    }
}
