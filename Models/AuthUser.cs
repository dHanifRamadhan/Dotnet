using Project01.Entities;

namespace Project01.Models {
    public class AuthUser {
        public string UserCode {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public Role? Role {get;set;}
        public List<PageAction>? PageAction {get;set;}
    }
}