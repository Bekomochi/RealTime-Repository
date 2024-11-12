using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.StreamingHubs
{
    [MessagePackObject]
    public class JoinedUser
    {

        [Key(0)]
        public Guid ConnectionID { get; set; } //接続ID7
        [Key(1)]
        public User UserData { get; set; } //ユーザー情報
        [Key(2)]
        public int JoinOrder {  get; set; }
    }
}
