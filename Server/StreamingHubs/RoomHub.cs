using MagicOnion.Server.Hubs;
using Server.Model.Context;
using Server.Model.Entity;
using Shared.Interfaces.StreamingHubs;
using UnityEngine;

namespace Server.StreamingHubs
{
    public class RoomHub : StreamingHubBase<IRoomHub, IRoomHubReciver>, IRoomHub
    {
        private IGroup room;

        //入室
        public async Task<JoinedUser[]> JoinAsync(string roomName, int userID)
        {
            //ルームに参加&ルームを保持する
            this.room = await this.Group.AddAsync(roomName);//AddAsyncで指定の部屋に入室

            //DBからユーザーの情報を取得する
            GameDBConnect connect = new GameDBConnect();
            var user = connect.Users.Where(user => user.Id == userID).First();

            //グループストレージにユーザーデータを格納
            var roomStrage = this.room.GetInMemoryStorage<RoomData>();
            var joinedUser = new JoinedUser() { ConnectionID = this.ConnectionId, UserData = user };
            var roomData = new RoomData() { JoinedUser = joinedUser };
            roomStrage.Set(this.ConnectionId, roomData);//接続IDをキーにしてデータを格納

            //ルーム参加者全員に、ユーザーの入室通知を送信
            this.BroadcastExceptSelf(room).OnJoin(joinedUser);//自分以外の参加者のOnJoinを呼び出す。Broadcastのみだと自分も含める
            RoomData[] roomDataList = roomStrage.AllValues.ToArray<RoomData>(); //AllValues.ToArrayで、格納データを配列で取得する

            //参加中のユーザー情報を返す
            JoinedUser[] joinUserList = new JoinedUser[roomDataList.Length];

            //if (roomDataList.Length >= 3)
            //{//ユーザーが3人集まったら

            //    this.Broadcast(room).OnPreparation(); //準備完了関数を呼ぶ
            //}

            for (int i = 0;i< roomDataList.Length ; i++)
            {//ユーザーをルームデータに追加
                joinUserList[i] = roomDataList[i].JoinedUser;
            }

            return joinUserList;
        }

        //マッチング
        public async Task<JoinedUser[]> JoinLobbyAsync(int userID)
        {
            JoinedUser[] joinedUserList = await JoinAsync("Lobby", userID);


            /*同じマッチング条件の人がいたらOnmatchingを呼び出す
            12/11時点では、「人数が集まったら」という仮条件にする*/
            if (joinedUserList.Length >= 3)
            {
                this.Broadcast(room).OnMatching(Guid.NewGuid().ToString());
            }

            return joinedUserList;
        }

        //退出
        public async Task<LeavedUser> LeaveAsync()
        {
            var leavedUser = new LeavedUser() { ConnectionID=this.ConnectionId };
            
            //グループデータから削除
            this.room.GetInMemoryStorage<RoomData>().Remove(this.ConnectionId);

            //退室したことをメンバーに通知
            this.Broadcast(room).OnLeave(leavedUser);


            //ルーム内のメンバーから自分を削除
            await room.RemoveAsync(this.Context);

            return leavedUser;
        }

        //位置、回転をクライアントに追加する
        public async Task MoveAsync(MovedUser movedUser) 
        {
            //グループストレージからRoomDataを取得する
            var roomStrage = this.room.GetInMemoryStorage<RoomData>();
            var roomData=roomStrage.Get(this.ConnectionId);

            //位置と回転を、それぞれroomDataに保存
            roomData.pos=movedUser.pos;
            roomData.rot=movedUser.rot;

            //ルーム内のユーザーに位置・回転の変更を送信
            this.BroadcastExceptSelf(room).OnMove(movedUser);
        }

        //準備完了
        public async Task ReadyAsync()
        {
            //準備ができたことを、自分のルームデータに保存する
            var roomStrage = this.room.GetInMemoryStorage<RoomData>();
            var roomData = roomStrage.Get(this.ConnectionId);

            lock(roomStrage)
            {//同時に実行した際、二回通知しないようにロック(排他制御)する

                /* もし同時にアクセスした時に、排他制御をしていないと二回実行してしまう可能性がある。
                 * 今回の場合だと、二回開始の通知をする可能性がある。
                 * その対策として[lock]内で処理をすることで、一人ずつ処理を行い、他の人は待機させる。
                 * 一人目が終わったら、待機していた二人目もlockを取得して、終わったら次の人...という風にして、全員終わったら通知する。
                 * 複数人が同時に実行しても大丈夫な関数をスレッドセーフという。
                 * 
                 * 書き方
                 * lock(共通の変数)
                 * {
                 *   処理
                 * }
                 */

                if (roomData != null)
                {//RoomDataのreserveDataに準備完了を保存する

                    roomData.reserveData = true;
                }

                //全員準備できたか判定
                bool isReady = true;
                var roomDataList = roomStrage.AllValues.ToArray<RoomData>();

                foreach (var data in roomDataList)
                {//roomDataに保存した準備完了状態を確認
                    if (data.reserveData == false)
                    {
                        isReady = false;
                    }
                }

                //全員準備完了していたら、全員にゲーム開始を通知
                if (isReady == true)
                {
                    this.Broadcast(room).OnReady();
                }

            }

        }

        //終了
        public async Task FinishAsync()
        {
            //全員にゲーム終了を通知
            this.Broadcast(room).OnFinish();
        }

        //クライアントの切断
        //protected override ValueTask OnDisconnected()
        //{
        //    //ルームデータを削除
        //    this.room.GetInMemoryStorage<RoomData>().Remove(this.ConnectionId);

        //    //退室したことを全メンバーに通知
        //    this.Broadcast(room).OnLeave(this.ConnectionId);

        //    //ルーム内のメンバーから削除
        //    room.RemoveAsync(this.Context);

        //    return CompletedTask;
        //}
    }
}