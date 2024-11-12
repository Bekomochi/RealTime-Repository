using MagicOnion;
using MagicOnion.Server;
using Server.Model.Context;
using Server.Model.Entity;
using Shared.Interfaces.Services;

namespace Server.Services
{
    public class UserService : ServiceBase<IUserService>, IUserService
    {
        public async UnaryResult<int> RegistUserAsync(string name)
        {
            using var context = new GameDB();

            //テーブルにレコードを追加
            User user = new User();
            user.Name = name;
            user.Created_at = DateTime.Now;
            user.Updated_at = DateTime.Now;
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user.Id;
        }
    }
}
