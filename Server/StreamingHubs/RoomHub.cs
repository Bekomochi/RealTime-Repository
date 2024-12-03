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
            this.BroadcastExceptSelf(room).OnJoin(joinedUser);//自分以外の参加者のOnJoinを呼び出す。Broadcastのみだと自分も含める。
            RoomData[] roomDataList = roomStrage.AllValues.ToArray<RoomData>();//AllValues.ToArrayで、格納データを配列で取得する。

            //参加中のユーザー情報を返す
            JoinedUser[] joinUserList = new JoinedUser[roomDataList.Length];

            for (int i = 0;i< roomDataList.Length ; i++)
            {
                joinUserList[i] = roomDataList[i].JoinedUser;
            }

            return joinUserList;
        }
        
        //退出
        public async Task<LeavedUser> LeaveAsync()
        {
            var leavedUser = new LeavedUser() { ConnectionID=this.ConnectionId };
            
            //グループデータから削除
            this.room.GetInMemoryStorage<RoomData>().Remove(this.ConnectionId);

            //ルーム内のメンバーから自分を削除
            await room.RemoveAsync(this.Context);

            //退室したことをメンバーに通知
            this.BroadcastExceptSelf(room).OnLeave(leavedUser);

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

        ////突然切断した場合
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