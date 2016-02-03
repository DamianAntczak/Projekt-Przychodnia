using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_BD.Models {
    [Table("SpisLekow")]
    public class SpisLekow {
       public SpisLekow() {
            Leki = new List<Lek>();
        }
        [Key]
        public string NazwaLeku { get; set; }
        public string NazwaPolskaLeku { get; set; }
        public ICollection<Lek> Leki { get; set; }
    }
}
