using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project01.Database;
using Project01.Entities;
using Project01.Helpers;
using Project01.Models;
using Serilog.Core;

namespace Project01.Services {
    public class UserService : IUserService {
        private readonly MysqlContext context;
        private readonly ConfigApp configApp;
        public UserService(
            MysqlContext context, 
            IOptions<ConfigApp> options
        ) {
            this.context = context;
            configApp = options.Value;
        }

        public async Task<PagedResponse<User>> GetAll(int page, int pageSize) {
            IQueryable<User> query = context.Users;

            if (page != -1)
                query = query.Skip((page -1) * pageSize).Take(pageSize);

            return new PagedResponse<User>(await query.ToListAsync());
        }

        public async Task<User> GetByCode(string code) {
            return await context.Users.SingleOrDefaultAsync(x => x.UserCode.Equals(code));
        }

        public async Task<User> GetByEmail(string email) {
            return await context.Users.SingleOrDefaultAsync(x => x.Email.Equals(email));
        }

        public async Task<int> Add(User item) {
            item.UserCode = Guid.NewGuid().ToString();
            context.Users.Add(item);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update(User item) {
            var result = await GetByCode(item.UserCode);
            if (result == null)
                throw new ApplicationException(string.Format(Constant.SERVICE_MESSAGE_NOT_FOUND, item.UserCode));

            if (await CheckExistEmail(item.Email))
                result.Email = item.Email;

            result.Password = item.Password;
            result.RoleCode = item.RoleCode; 
            result.PlayerCode = item.PlayerCode;

            context.Users.Update(result);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Remove(User item) {
            context.Users.Remove(item);
            return await context.SaveChangesAsync();
        }

        public async Task<bool> CheckExistEmail(string email) {
            return await context.Users.AnyAsync(x => x.Email.Equals(email));
        }

        public async Task<bool> CheckExistRole(string roleCode) {
            return await context.Users.AnyAsync(x => x.RoleCode.Equals(roleCode));
        }

        public async Task<bool> CheckExistPlayerCode(string playerCode) {
            return await context.Users.AnyAsync(x => x.PlayerCode.Equals(playerCode));
        }

        public async Task<AuthorizationResponse> Auth(AuthorizationRequest item) {
            var data = await GetByAuthEmail(item.Email);
            if (data != null && AuthPassword.Verify(item.Password, data.Password)) {
                var token = Utils.GenerateToken(data, configApp.KeyApp);
                return new AuthorizationResponse(data, token);
            }
            return null;
        }

        private async Task<AuthUser> GetByAuthEmail(string email) {
            var users = context.Users.Where(x => x.Email.Equals(email));
            IQueryable<Access> accesses = context.Accesses;

            var mainJoin = users.GroupJoin(
                context.Roles, 
                user => user.RoleCode, 
                role => role.RoleCode, 
                (user, role) => new {user, role})
            .SelectMany(x =>
                x.role.DefaultIfEmpty(),
                (user, role) => new {user.user, role});

            var subJoin = accesses.GroupJoin(
                context.Pages, 
                acc => acc.PageCode, 
                page => page.PageCode, 
                (acc, page) => new {acc, page})
            .SelectMany(x =>
                x.page.DefaultIfEmpty(),
                (acc, page) => new {acc.acc, page});

            var user =  await mainJoin.Select(x => new AuthUser {
                UserCode = x.user.UserCode,
                Email = x.user.Email,
                Password = x.user.Password,
                Role = x.role,
                PageAction = subJoin.Where(y => 
                    y.acc.RoleCode.Equals(x.role.RoleCode))
                .Select(y => new PageAction {
                    PageCode = y.page.PageCode,
                    PageName = y.page.PageName,
                    Actions = y.acc.Actions })
                .ToList() ?? null,
            }).SingleOrDefaultAsync();

            return user;
        }
    }
}