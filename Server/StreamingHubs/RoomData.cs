using Shared.Interfaces.StreamingHubs;
using UnityEngine;

namespace Server.StreamingHubs
{
    public class RoomData
    {//ルーム内に保存するデータクラス

        public JoinedUser JoinedUser { get; set; }

       //座標、回転を追加
        public Vector3 pos { get; set; } //座標
        public Quaternion rot { get; set; } //回転

        public bool reserveData {  get; set; } //準備完了を保存するbool型変数
    }
}