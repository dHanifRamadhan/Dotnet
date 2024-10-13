using Project01.Entities;
using Project01.Helpers;
using Project01.Models;

namespace Project01.Services {
    public interface IUserService {
        Task<PagedResponse<User>> GetAll(int page, int pageSize);
        Task<User> GetByCode(string code);
        Task<User> GetByEmail(string email);
        Task<int> Add(User item);
        Task<int> Update(User item);
        Task<int> Remove(User item);
        Task<bool> CheckExistEmail(string email);
        Task<bool> CheckExistRole(string roleCode);
        Task<bool> CheckExistPlayerCode(string playerCode);
        Task<AuthorizationResponse> Auth(AuthorizationRequest item);
    }
}