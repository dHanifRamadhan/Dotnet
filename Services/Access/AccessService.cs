using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project01.Database;
using Project01.Entities;
using Project01.Helpers;
using Project01.Models;

namespace Project01.Services {
    public class AccessService : IAccessService {
        private readonly MysqlContext context;
        private readonly ConfigApp configApp;
        public AccessService(MysqlContext context, IOptions<ConfigApp> options) {
            this.context = context;
            configApp = options.Value;
        }

        public async Task<int> Add(List<Access> items) {
            context.Accesses.AddRange(items);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Remove(List<Access> items) {
            context.Accesses.RemoveRange(items);
            return await context.SaveChangesAsync();
        }

        public async Task<List<Access>> GetByRoleCode(string code) {
            return await context.Accesses.Where(x => x.RoleCode.Equals(code)).ToListAsync();
        }

        public async Task<bool> CheckExistRoleCode(string code) {
            return await context.Accesses.AnyAsync(x => x.RoleCode.Equals(code));
        }

        public async Task<bool> CheckExistPageCode(string code) {
            return await context.Accesses.AnyAsync(x => x.PageCode.Equals(code));
        }
    }
}