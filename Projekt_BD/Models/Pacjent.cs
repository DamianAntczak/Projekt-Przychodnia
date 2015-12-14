using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Projekt_BD.Models
{
    public class Pacjent
    {
        public Pacjent()
        {
            Wizyty = new List<Wizyta>();
            Choroby = new List<Choroba>();
        }

        [Key]
        public Guid IdPacjenta { get; set; }
        public String Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime  DataUrodzenie { get; set; }
        public String MiejsceUrodzenia { get; set; }
        public String Plec { get; set; }
        public String NrTelefonu { get; set; }
        public String Mail { get; set; }

        public virtual ICollection<Wizyta> Wizyty { get; set; }
        public virtual ICollection<Choroba> Choroby { get; set; }
    }

    
}
