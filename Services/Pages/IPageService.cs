using Project01.Entities;
using Project01.Models;

namespace Project01.Services {
    public interface IPageService {
        Task<PagedResponse<Page>> GetAll(int page, int pageSize);
        Task<PagedResponse<OptionReponse>> GetOptions();
        Task<Page> GetByCode(string code);
        Task<int> Add(Page item);
        Task<int> Update(Page item);
        Task<int> Remove(Page item); 
    }
}