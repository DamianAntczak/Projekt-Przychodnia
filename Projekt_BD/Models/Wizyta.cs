using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projekt_BD.Models
{
    public class Wizyta
    {
        public Wizyta()
        {
            Recepty = new List<Recepta>();
        }

        [Key]
        public Guid IdWizyty { get; set; }
        public DateTime Data { get; set; }
        //ma być typu DateTime, gdzie będziemy określać ile ma trwać wizyta, np. 1 h 20 min czy Int32
        //do określenia ilości minut -> 80 min?
        public TimeSpan CzasWizyty { get; set; }
        public virtual Lekarz Lekarz { get; set; }
        public virtual Pacjent Pacjent { get; set; }
        public ICollection<Recepta> Recepty { get; set; }

        //Pacjent u danego specjalisty może mieć zdjagnozowaną jedną chorobę
        //Jeżeli uwzględnimy więcej chorób można stworzyć nową klasę Diagnoza, która będzie miała
        //Kolekcję wykrytych chorób w danej wizycie
        //[ForeignKey("IdChoroby")]
        //public Choroba Choroba { get; set; }
        //Wystawiona będzie jedna recepta na daną wizytę
        //public  Recepta Recepta { get; set; }
    }
}
