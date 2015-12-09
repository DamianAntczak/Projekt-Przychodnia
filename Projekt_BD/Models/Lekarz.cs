using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Projekt_BD.Models;

namespace Projekt_BD
{
    public class Lekarz 
    {
        public Lekarz()
        {
            Wizyty = new List<Wizyta>();
        }

        [Key]
        public Guid IdLekarza { get; set; }
        public String Imie { get; set; }
        public String Nazwisko { get; set; }
        public virtual Specjalizacja Specjalizacja { get; set; }
        public String Adres { get; set; }

        public virtual  ICollection<Wizyta> Wizyty { get; set; }
    }
}
