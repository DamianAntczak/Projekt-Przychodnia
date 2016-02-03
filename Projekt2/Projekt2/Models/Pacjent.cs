using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_BD.Models
{
   // public enum plec { Kobieta, Mężczyzna };
    [Table("Pacjenci")]
    public class Pacjent
    {
        

        public Pacjent()
        {
            HistoriaChoroby = new List<HistoriaChoroby>();
        }
        [Key]
        public String Pesel { get; set; }
        public String Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime  DataUrodzenie { get; set; }
        public String MiejsceUrodzenia { get; set; }
        public Plec Plec { get; set; }
        public String NrTelefonu { get; set; }
        public String Mail { get; set; }

        public virtual ICollection<HistoriaChoroby> HistoriaChoroby { get; set; }
    }

    
}
