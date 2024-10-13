using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Project01.Database;
using Project01.Entities;
using Project01.Helpers;
using Project01.Models;
using Serilog.Core;

namespace Project01.Services {
    public class PageService : IPageService {
        private readonly MysqlContext context;
        private readonly ConfigApp configApp;
        private readonly ModifyFile modify = new ModifyFile();
        public PageService(
            MysqlContext context, 
            IOptions<ConfigApp> options
        ) {
            this.context = context;
            configApp = options.Value;
        }

        public async Task<PagedResponse<Page>> GetAll(int page, int pageSize) {
            IQueryable<Page> query = context.Pages;

            if (page != -1)
                query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return new PagedResponse<Page>(await query.ToListAsync());
        }

        public async Task<PagedResponse<OptionReponse>> GetOptions() {
            IQueryable<Page> query = context.Pages;
            var option = query.Select(x => new OptionReponse {
                Label = x.PageName,
                Value = x.PageCode
            });
            return new PagedResponse<OptionReponse>(await option.ToListAsync());
        }

        public async Task<Page> GetByCode(string code) {
            return await context.Pages.SingleOrDefaultAsync(x => x.PageCode.Equals(code));
        }

        public async Task<int> Add(Page item) {
            item.PageCode = Guid.NewGuid().ToString();
            context.Pages.Add(item);
            modify.AddEnumPage(item);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Update(Page item) {
            var result = await GetByCode(item.PageCode);
            if (result == null) 
                throw new ApplicationException(string.Format(Constant.SERVICE_MESSAGE_NOT_FOUND, item.PageCode));

            string oldName = result.PageName;
            result.PageName = item.PageName;

            context.Pages.Update(result);
            modify.UpdateEnumPage(result, oldName);
            return await context.SaveChangesAsync();
        }

        public async Task<int> Remove(Page item) {
            context.Pages.Remove(item);
            modify.DeleteEnumPage(item);
            return await context.SaveChangesAsync();
        }
    }
}