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
            using var context = new GameDBConnect();

            /*if文チェック。
             条件のレコード数が0より大きいなら、ReturnStatusExceptionでエラーを返す*/
            if (context.Users.Where(user=>user.Name==name).Count()>0)
            {
                throw new ReturnStatusException(Grpc.Core.StatusCode.InvalidArgument, "");
            }

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
