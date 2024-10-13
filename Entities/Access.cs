using System.ComponentModel.DataAnnotations.Schema;

namespace Project01.Entities {
    [Table("role_page_access")]
    public class Access {
        [Column("role_code")]
        public string RoleCode { get; set; }

        [Column("page_code")]
        public string PageCode {get;set;}

        [Column("actions")]
        public string Actions {get;set;}
    }
}