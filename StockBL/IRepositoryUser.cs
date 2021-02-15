using StockPlaygroundMVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockBL
{
    public interface IRepositoryUser
    {
        IEnumerable<UserAdv> UsersWithAccountRole();
        string RetrieveUserWatchlist();
    }

    public class RepoUser : IRepositoryUser
    {
        private readonly StockUsersDBContext context;
        public RepoUser(StockUsersDBContext context)
        {
            this.context = context;
        }

        public IEnumerable<UserAdv> UsersWithAccountRole()
        {
            var model = context.Users.Select(user => new UserAdv()
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserPassword = user.UserPassword,
                UserRole = user.UserRole
            });
            model = model.Where(u => u.UserRole.Equals(2));
            var ret = model.ToArray();
            return ret;
        }

        public string RetrieveUserWatchlist()
        {
            var model = context.UserWatchlists.Where(w => w.UserId.Equals(2)).Select(w => w.WatchItems);
            return "TME;BA";
        }
    }
}
