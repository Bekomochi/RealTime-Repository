using MagicOnion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Interfaces.Services
{
    public interface IUserService : IService<IUserService>
    {
        UnaryResult<int> RegistUserAsync(string name);
    }
}