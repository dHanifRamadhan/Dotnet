using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project01.Database;
using Project01.Entities;
using Project01.Helpers;
using Project01.Models;

namespace Project01.Services {
    public class RoleService : IRoleService {
        private readonly MysqlContext context;
        private readonly ConfigApp configApp;
        public RoleService(
            MysqlContext context, 
            IOptions<ConfigApp> options
        ) {
            this.context = context;
            configApp = options.Value;
        }

        public async Task<PagedResponse<RolePageRequest>> GetAll(int page, int pageSize) {
            IQueryable<Role> roles = context.Roles;

            var subJoin = context.Accesses.GroupJoin(context.Pages, acc => acc.PageCode, page => page.PageCode, (acc, page) => new {acc, page})
                .SelectMany(x => x.page.DefaultIfEmpty(), (acc, page) => new {acc.acc, page});

            var query = roles.GroupJoin(subJoin, role => role.RoleCode, acc => acc.acc.RoleCode, (role, acc) => new {role, acc})
                .SelectMany(x => x.acc.DefaultIfEmpty(), (role, acc) => new {role.role, acc.acc, acc.page});

            if (page != -1)
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

            var q = query.GroupBy(x => x.role)
                .Select(x => new RolePageRequest {
                    RoleCode = x.Key.RoleCode,
                    RoleName = x.Key.RoleName,
                    PageActions = x.Select(y => new PageAction{
                        PageCode = y.acc.PageCode,
                        PageName = y.page.PageName,
                        Actions = y.acc.Actions
                    }).ToList()
                });

            return new PagedResponse<RolePageRequest>(await q.ToListAsync());
        }

        public async Task<PagedResponse<OptionReponse>> GetOptions() {
            IQueryable<Role> query = context.Roles;
            var options = query.Select(x => new OptionReponse {
                Label = x.RoleName,
                Value = x.RoleCode
            });
            return new PagedResponse<OptionReponse>(await options.ToListAsync());
        }

        public async Task<Role> GetByCode(string code) {
            return await context.Roles.SingleOrDefaultAsync(x => x.RoleCode.Equals(code));
        }

        public async Task<string> Add(Role item) {
            item.RoleCode = Guid.NewGuid().ToString();
            context.Roles.Add(item);
            await context.SaveChangesAsync();
            return item.RoleCode;
        }

        public async Task<string> Update(Role item){
            var result = await GetByCode(item.RoleCode);
            if (result == null)
                throw new ApplicationException(string.Format(Constant.SERVICE_MESSAGE_NOT_FOUND, item.RoleCode));

            result.RoleName = result.RoleName;

            context.Roles.Update(result);
            await context.SaveChangesAsync();
            return result.RoleCode;
        }

        public async Task<string> Remove(Role item) {
            context.Roles.Remove(item);
            await context.SaveChangesAsync();
            return item.RoleCode;
        } 
    }
}