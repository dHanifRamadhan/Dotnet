using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Project01.Models {
    public class OptionReponse {
        [Required]
        public string Label {get;set;}
        [Required]
        public string Value {get;set;}
        public string? Description {get;set;}
    }
}