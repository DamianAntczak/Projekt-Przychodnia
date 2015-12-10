using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_BD.Models
{
    public class Recepta
    {
        public Recepta()
        {
            Leki = new List<Lek>();
        }

        [Key]
        public Guid IdRecepty { get; set; }
        //[ForeignKey("IdLeku")]
        public Guid IdLeku { get; set; }
        public String Dawka { get; set; }
        public String Przyjmowanie { get; set; }
        public DateTime CzasWystawienia { get; set; }
        public virtual ICollection<Lek> Leki { get; set; }
    }
}
