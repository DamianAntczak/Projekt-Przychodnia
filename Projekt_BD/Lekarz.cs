using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_BD
{
    public class Lekarz
    {
        [Key]
        public Guid IdLekarza { get; set; }
        public String Imie { get; set; }
        public String Nazwisko { get; set; }
        public String Adres { get; set; }
        //lekarz ma jedną specjalizację
        public Specjalizacja specjalizacja { get; set; }

    }
}
