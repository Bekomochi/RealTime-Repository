using MagicOnion;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Interfaces.StreamingHubs
{
    public interface IRoomHub : IStreamingHub<IRoomHub, IRoomHubReciver>
    {
        Task<JoinedUser[]>JoinAsync(string roomName,int userId);

        Task<LeavedUser> LeaveAsync(string roomName, int userId);
    }
}
