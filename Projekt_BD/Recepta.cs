using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_BD
{
    public class Recepta
    {
        [Key]
        public Guid IdRecepty { get; set; }
        //Recepta może zawierać wiele leków
        public ICollection<Lek> Leki { get; set; }
        //Dawka i Przyjmowanie powinno być słownie?
        public String Dawka { get; set; }
        public String Przyjmowanie { get; set; }
        public DateTime CzasWystawieniaRecepty { get; set; }
    }
}
