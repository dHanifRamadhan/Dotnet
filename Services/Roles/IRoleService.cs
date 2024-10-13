using Project01.Entities;
using Project01.Models;

namespace Project01.Services {
    public interface IRoleService {
        Task<PagedResponse<RolePageRequest>> GetAll(int page, int pageSize);
        Task<PagedResponse<OptionReponse>> GetOptions();
        Task<Role> GetByCode(string code);
        Task<string> Add(Role item);
        Task<string> Update(Role item);
        Task<string> Remove(Role item);
    }
}