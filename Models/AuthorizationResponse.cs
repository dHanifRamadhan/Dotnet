using Project01.Entities;

namespace Project01.Models {
    public class AuthorizationResponse {
        public string Email {get;}
        public string Token {get;}
        public AuthorizationResponse(AuthUser item, string token) {
            this.Email = item.Email;
            this.Token = token; 
        }
    }
}