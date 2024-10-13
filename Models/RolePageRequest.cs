using Project01.Entities;

namespace Project01.Models {
    public class RolePageRequest {
        public string RoleCode {get;set;}
        public string RoleName {get;set;}
        public List<PageAction>? PageActions {get;set;}
    }
}