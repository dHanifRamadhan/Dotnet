using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project01.Entities {
    [Table("pages")]
    public class Page {
        [Key]
        [Column("page_code")]
        public string PageCode { get; set; }

        [Column("page_name")]
        public string PageName { get; set; }
    }
}