using Cysharp.Net.Http;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using Shared.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    internal class UserModel : BaseModel
    {
        const string ServerURL = "http://localhost:7000";
        private int userID; //登録ユーザーID

        public async UniTask<bool> RegistAsync(string name)
        {//UserModelのRegistAsyncを呼び出すスクリプトは別途作成する
            var handler = new YetAnotherHttpHandler() { Http2Only = true };
            var channel = GrpcChannel.ForAddress(ServerURL, new GrpcChannelOptions() { HttpHandler = handler });
            var cliant = MagicOnionClient.Create<IUserService>(channel);

            try
            {//登録成功
                userID = await cliant.RegistUserAsync(name);
                return true;
            }
            catch(RpcException e)
            {//登録失敗
                //Debug.Log(e);
                return false;
            }
        }
    }
}
