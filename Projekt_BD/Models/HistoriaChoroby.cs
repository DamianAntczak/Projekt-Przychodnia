using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_BD.Models;

namespace Projekt_BD {
    public class HistoriaChoroby {

        HistoriaChoroby()
        {
            Wizyty = new List<Wizyta>();
        }

        [Key]
        [Column(Order = 0)]
        public String Pesel { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid IdLekarza { get; set; }

        [ForeignKey("Pesel")]
        public virtual Pacjent Pacjent { get; set; }

        [ForeignKey("IdLekarza")]
        public virtual Lekarz Lekarz { get; set; }

        public Guid IdWizyty;
        public virtual ICollection<Wizyta> Wizyty { get; set; }

        public string OpisChoroby { get; set; }
        public DateTime OstatniaModyfikacjaOpisuChoroby { get; set; }
        public SpisChorb ChorobaZeSpisu { get; set; }
    }
}
