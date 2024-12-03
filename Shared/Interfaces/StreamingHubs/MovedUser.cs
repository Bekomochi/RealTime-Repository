using MessagePack;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

namespace Shared.Interfaces.StreamingHubs
{
    [MessagePackObject]

    public class MovedUser
    {
        [Key(0)]
        public Guid ConnectionID { get; set; }
        [Key(1)]
        public Vector3 pos { get; set; }
        [Key(2)]
        public Quaternion rot { get; set; }
    }
}
