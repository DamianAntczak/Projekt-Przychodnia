using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_BD
{
    public class Pacjent
    {
        [Key]
        public Guid IdPacjenta { get; set; }
        public String Imie { get; set; }
        public DateTime  DataUrodzenie { get; set; }
        public String MiejsceUrodzenia { get; set; }
        public String Plec { get; set; }
        public String NrTelefonu { get; set; }
        public String Mail { get; set; }

    }

    
}
