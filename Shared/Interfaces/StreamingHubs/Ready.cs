using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.StreamingHubs
{
    [MessagePackObject]
    public class Ready
    {
        [Key(0)]
        bool isStart;
    }
}
