using Shared.Interfaces.StreamingHubs;

namespace Server.StreamingHubs
{
    public class RoomData
    {//ルーム内に保存するデータクラス
        public JoinedUser JoinedUser { get; set; }

        //座標、回転を追加
        public float pos;//位置
        public float rot;//回転
    }
}