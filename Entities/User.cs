using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project01.Entities {
    [Table("users")]
    public class User {
        [Key]
        [Column("user_code")]
        public string UserCode {get;set;}

        [Column("email")]
        public string Email {get;set;}

        [Column("password")]
        public string Password {get;set;}

        [Column("role_code")]
        public string RoleCode {get;set;}

        [Column("player_code")]
        public string? PlayerCode {get;set;}
    }
}