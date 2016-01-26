using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_BD.Models
{
    public class DniPrzyjec
    {
        public int ID { get; set; }
        public virtual Lekarz Lekarz { get; set; }
        public DateTime CzasDataRozpoczecia { get; set; }
        public DateTime CzasZakonczenia { get; set; }
    }
}
