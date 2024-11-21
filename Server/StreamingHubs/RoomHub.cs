using MagicOnion.Server.Hubs;
using Server.Model.Context;
using Shared.Interfaces.StreamingHubs;

namespace Server.StreamingHubs
{
    public class RoomHub : StreamingHubBase<IRoomHub, IRoomHubReciver>, IRoomHub
    {
        private IGroup room;

        public async Task<JoinedUser[]>JoinAsync(string roomName,int userID)
        {
            //ルームに参加&ルームを保持する
            this.room = await this.Group.AddAsync(roomName);//指定の部屋に入室

            //DBからユーザーの情報を取得する
            GameDBConnect connect = new GameDBConnect();
            var user=connect.Users.Where(user=>user.Id==userID).First();

            //グループストレージにユーザーデータを格納
            var roomStrage = this.room.GetInMemoryStorage<RoomData>();//ルームには参加者全員が参照可能な共有の保存領域がある(Memory-メモリ)
            var joinedUser = new JoinedUser() { ConnectionID = this.ConnectionId,UserData=user};
            var roomData = new RoomData() { JoinedUser = joinedUser };
            roomStrage.Set(this.ConnectionId, roomData);

            //ルーム参加者全員に、ユーザーの入室通知を送信
            this.BroadcastExceptSelf(room).OnJoin(joinedUser);//自分以外の参加者のOnJoinを呼び出す。Broadcastのみだと自分も含める。
            RoomData[] roomDataList=roomStrage.AllValues.ToArray<RoomData>();//AllValues.ToArrayで、格納データを配列で取得する。

            //参加中のユーザー情報を返す
            JoinedUser[] joinUserList = new JoinedUser[roomDataList.Length];

            for (int i = 0; i < roomDataList.Length; i++)
            {
                joinedUser = joinUserList[i];
            }

            return joinUserList; 
        }
    }
}
