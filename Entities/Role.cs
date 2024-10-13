using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project01.Entities {
    [Table("roles")]
    public class Role {
        [Key]
        [Column("role_code")]
        public string RoleCode {get;set;}

        [Column("role_name")]
        public string RoleName { get; set; }
    }
}