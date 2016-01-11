using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_BD.Models;

namespace Projekt_BD {
    public class Choroba {
        [Key]
        // Id przypadku choroby?
        public Guid IdChoroby { get; set; }
        public virtual String Pesel { get; set; }
        public virtual Pacjent Pacjent { get; set; }
        public string OpisChoroby { get; set; }
        public DateTime OstatniaModyfikacjaOpisuChoroby { get; set; }
        public SpisChorb ChorobaZeSpisu { get; set; }
    }
}
