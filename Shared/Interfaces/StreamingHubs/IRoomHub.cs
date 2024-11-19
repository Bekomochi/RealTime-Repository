using MagicOnion;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHub:IStreamingHub<IRoomHub,IRoomHubReciver>
    {
        //===============================================//
        // ここにクライアント側からサーバーを呼び出す関数を定義する //
        //==============================================//

        //ユーザー入室
        Task<JoinedUser[]>JoinAsync(string roomName,int userID);

        /*解説とか
         *JoinedUser[](JoinedUser配列)...入室済みユーザーの一覧を返す
         *第一引数・roomName...入室するルーム名
         *第二引数・userID...ユーザーID指定で入室
         *第二引数・userID...ユーザーID指定で入室
         */
    }
}
