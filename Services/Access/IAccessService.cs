using Project01.Entities;

namespace Project01.Services {
    public interface IAccessService {
        Task<int> Add(List<Access> items);
        Task<int> Remove(List<Access> items);
        Task<List<Access>> GetByRoleCode(string code);
        Task<bool> CheckExistRoleCode(string code);
        Task<bool> CheckExistPageCode(string code);
    }
}