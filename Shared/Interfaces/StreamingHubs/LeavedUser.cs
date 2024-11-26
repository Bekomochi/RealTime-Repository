using MessagePack;
using Server.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Shared.Interfaces.StreamingHubs
{
    [MessagePackObject]
    public class LeavedUser
    {
        [Key(0)]
        public Guid ConnectionID { get; set; } //接続ID
        [Key(1)]
        public User UserData { get; set; }//ユーザー情報
    }
}